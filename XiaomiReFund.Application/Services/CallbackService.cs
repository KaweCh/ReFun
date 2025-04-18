using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using XiaomiReFund.Application.Common.Interfaces;
using XiaomiReFund.Application.Common.Models;
using XiaomiReFund.Application.DTOs.Callback;
using XiaomiReFund.Application.Interfaces.External;
using XiaomiReFund.Application.Interfaces.Services;
using XiaomiReFund.Domain.Interfaces.Repositories;

namespace XiaomiReFund.Application.Services
{
    /// <summary>
    /// บริการ callback
    /// </summary>
    public class CallbackService : ICallbackService
    {
        private readonly IHttpClientService _httpClient;
        private readonly IRefundRepository _refundRepository;
        private readonly ILoggerService _logger;
        private readonly IDateTime _dateTime;
        private readonly CallbackSettings _callbackSettings;

        /// <summary>
        /// สร้าง CallbackService ใหม่
        /// </summary>
        /// <param name="httpClient">บริการ HTTP client</param>
        /// <param name="refundRepository">คลังข้อมูลการคืนเงิน</param>
        /// <param name="logger">บริการบันทึกข้อมูล</param>
        /// <param name="dateTime">บริการวันเวลา</param>
        /// <param name="callbackSettings">การตั้งค่า callback</param>
        public CallbackService(
            IHttpClientService httpClient,
            IRefundRepository refundRepository,
            ILoggerService logger,
            IDateTime dateTime,
            IOptions<CallbackSettings> callbackSettings)
        {
            _httpClient = httpClient;
            _refundRepository = refundRepository;
            _logger = logger;
            _dateTime = dateTime;
            _callbackSettings = callbackSettings.Value;
        }

        /// <summary>
        /// ส่ง callback ไปยังปลายทาง
        /// </summary>
        /// <param name="request">ข้อมูลคำขอส่ง callback</param>
        /// <returns>ผลลัพธ์การส่ง callback</returns>
        public async Task<CallbackResponse> SendCallbackAsync(SendCallbackRequest request)
        {
            try
            {
                // ดึงข้อมูลจากฐานข้อมูล
                var refund = await _refundRepository.GetByTerminalAndRequestIdAsync(
                    request.TerminalID, request.RequestID);

                if (refund == null)
                {
                    _logger.LogError("Callback", new Exception("Refund not found"), 0);
                    return new CallbackResponse(404, "Not Found");
                }

                // ดึง URL ปลายทางของ callback จากตารางเทอร์มินัล
                var callbackUrl = await _refundRepository.GetCallbackUrlAsync(request.TerminalID);
                if (string.IsNullOrEmpty(callbackUrl))
                {
                    _logger.LogError("Callback", new Exception("Callback URL not found"), 0);
                    return new CallbackResponse(400, "Bad Request");
                }

                // ส่ง HTTP request
                try
                {
                    var headers = new Dictionary<string, string>
                    {
                        { "Content-Type", "application/json" }
                    };

                    var response = await _httpClient.PostAsync<SendCallbackRequest, CallbackResponse>(
                        callbackUrl, request, headers);

                    // บันทึก log
                    _logger.LogCallback(
                        callbackUrl,
                        request,
                        response,
                        response?.StatusCode == 200,
                        0);

                    // ตรวจสอบผลลัพธ์
                    if (response != null && response.StatusCode == 200 && response.Status == "Accepted")
                    {
                        // อัพเดตสถานะในฐานข้อมูลว่าได้ส่ง callback สำเร็จแล้ว
                        await _refundRepository.UpdateCallbackStatusAsync(refund.RefundID, true, 0);
                        return response;
                    }

                    // ถ้าไม่สำเร็จ จะเพิ่มเข้าคิวเพื่อลองใหม่ภายหลัง
                    await EnqueueCallbackForRetryAsync(request, 0);
                    return new CallbackResponse(500, "Remote server error");
                }
                catch (Exception ex)
                {
                    _logger.LogError("Callback", ex, 0);

                    // เพิ่มเข้าคิวเพื่อลองใหม่
                    await EnqueueCallbackForRetryAsync(request, 0);

                    return new CallbackResponse(500, "Internal Server Error");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Callback", ex, 0);
                return new CallbackResponse(500, "Internal Server Error");
            }
        }

        /// <summary>
        /// เพิ่ม callback เข้าคิวสำหรับส่งในภายหลัง
        /// </summary>
        /// <param name="request">ข้อมูลคำขอเพิ่ม callback เข้าคิว</param>
        /// <returns>ผลลัพธ์การเพิ่ม callback เข้าคิว</returns>
        public async Task<EnqueueCallbackResponse> EnqueueCallbackAsync(EnqueueCallbackRequest request)
        {
            try
            {
                // บันทึกลงในระบบคิว
                int callbackId = await _refundRepository.SaveCallbackToQueueAsync(
                    request.RefundID,
                    request.TerminalID,
                    request.TransactionDate,
                    request.TransactionID,
                    request.RefundAmount,
                    request.RequestID,
                    request.Status,
                    request.StatusMessage,
                    request.PaymentType,
                    request.RetryCount,
                    request.ScheduledTime);

                return new EnqueueCallbackResponse
                {
                    Result = new ResultData
                    {
                        StatusCode = 200,
                        Status = "Success",
                        Msg = "Callback queued successfully"
                    },
                    IsQueued = true,
                    ScheduledTime = request.ScheduledTime
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("EnqueueCallback", ex, 0);

                return new EnqueueCallbackResponse
                {
                    Result = new ResultData
                    {
                        StatusCode = 500,
                        Status = "Error",
                        Msg = $"Failed to enqueue callback: {ex.Message}"
                    },
                    IsQueued = false
                };
            }
        }

        /// <summary>
        /// ประมวลผล callback ที่อยู่ในคิว
        /// </summary>
        /// <returns>จำนวน callback ที่ประมวลผลสำเร็จ</returns>
        public async Task<int> ProcessCallbackQueueAsync()
        {
            var successCount = 0;
            var pendingCallbacks = await GetPendingCallbacksAsync(_callbackSettings.BatchSize);

            foreach (var callback in pendingCallbacks)
            {
                var result = await SendCallbackAsync(callback);

                if (result.StatusCode == 200 && result.Status == "Accepted")
                {
                    successCount++;

                    // ตรวจสอบว่าสถานะคืนเงินตรงกับที่กำหนดไว้ใน callback หรือไม่
                    var refund = await _refundRepository.GetByTerminalAndRequestIdAsync(
                        callback.TerminalID, callback.RequestID);

                    if (refund != null)
                    {
                        // อัปเดตสถานะในฐานข้อมูลว่าได้ส่ง callback สำเร็จแล้ว
                        await _refundRepository.UpdateCallbackStatusAsync(
                            refund.RefundID, true, 0);
                    }
                }
            }

            return successCount;
        }

        /// <summary>
        /// ดึงข้อมูล callback ที่ยังไม่ได้ส่ง
        /// </summary>
        /// <param name="count">จำนวนที่ต้องการดึง</param>
        /// <returns>คำขอส่ง callback ที่ยังไม่ได้ส่ง</returns>
        public async Task<SendCallbackRequest[]> GetPendingCallbacksAsync(int count = 10)
        {
            try
            {
                // ดึงข้อมูลจากระบบคิวที่มีอยู่
                var pendingCallbacks = await _refundRepository.GetPendingCallbacksForProcessingAsync(count);

                // แปลงจาก Domain Model ไปเป็น DTO
                return pendingCallbacks.Select(c => new SendCallbackRequest
                {
                    Status = c.Status,
                    Msg = c.StatusMessage,
                    TerminalID = c.TerminalID,
                    TransactionDate = c.TransactionDate.ToString("yyyy-MM-dd"),
                    TransactionID = c.TransactionID,
                    RefundAmount = c.RefundAmount,
                    RequestID = c.RequestID,
                    PaymentType = c.PaymentType
                }).ToArray();
            }
            catch (Exception ex)
            {
                _logger.LogError("GetPendingCallbacks", ex, 0);
                return Array.Empty<SendCallbackRequest>();
            }
        }

        /// <summary>
        /// อัปเดตสถานะของ callback
        /// </summary>
        /// <param name="callbackId">รหัส callback</param>
        /// <param name="isSuccess">สถานะความสำเร็จ</param>
        /// <param name="retryCount">จำนวนครั้งที่ลองใหม่</param>
        /// <returns>ผลลัพธ์การอัปเดตสถานะ</returns>
        public async Task<bool> UpdateCallbackStatusAsync(int callbackId, bool isSuccess, int retryCount)
        {
            try
            {
                // อัพเดทสถานะในระบบคิว
                await _refundRepository.UpdateCallbackProcessingStatusAsync(callbackId, isSuccess, retryCount);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("UpdateCallbackStatus", ex, 0);
                return false;
            }
        }

        /// <summary>
        /// เพิ่ม callback เข้าคิวสำหรับลองใหม่
        /// </summary>
        /// <param name="request">ข้อมูลคำขอส่ง callback</param>
        /// <param name="retryCount">จำนวนครั้งที่ลองแล้ว</param>
        // This is the method inside CallbackService.cs that needs to be updated

        private async Task EnqueueCallbackForRetryAsync(SendCallbackRequest request, int retryCount)
        {
            // Calculate retry delay based on retry count using exponential backoff
            var delayInMinutes = retryCount switch
            {
                0 => _callbackSettings.RetryDelayMinutes,
                1 => _callbackSettings.RetryDelayMinutes * 2,
                2 => _callbackSettings.RetryDelayMinutes * 4,
                3 => _callbackSettings.RetryDelayMinutes * 8,
                _ => _callbackSettings.RetryDelayMinutes * 16
            };

            var scheduledTime = _dateTime.UtcNow.AddMinutes(delayInMinutes);

            // Find RefundID from TerminalID and RequestID
            var refund = await _refundRepository.GetByTerminalAndRequestIdAsync(
                request.TerminalID, request.RequestID);

            if (refund == null)
            {
                _logger.LogError("EnqueueCallbackForRetry",
                    new Exception($"Refund not found for Terminal: {request.TerminalID}, Request: {request.RequestID}"), 0);
                return;
            }

            // Create enqueue request
            var enqueueRequest = new EnqueueCallbackRequest
            {
                RefundID = refund.RefundID,
                TerminalID = request.TerminalID,
                TransactionDate = DateTime.Parse(request.TransactionDate),
                TransactionID = request.TransactionID,
                RefundAmount = request.RefundAmount,
                RequestID = request.RequestID,
                Status = request.Status,
                StatusMessage = request.Msg,
                PaymentType = request.PaymentType,
                RetryCount = retryCount + 1,
                ScheduledTime = scheduledTime
            };

            // Save to queue
            await EnqueueCallbackAsync(enqueueRequest);
        }
    }
}
