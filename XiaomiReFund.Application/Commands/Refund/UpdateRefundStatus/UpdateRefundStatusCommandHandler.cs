using MediatR;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XiaomiReFund.Application.Common.Exceptions;
using XiaomiReFund.Application.Common.Interfaces;
using XiaomiReFund.Application.Common.Models;
using XiaomiReFund.Application.DTOs.Callback;
using XiaomiReFund.Application.Interfaces.Services;
using XiaomiReFund.Domain.Constants;
using XiaomiReFund.Domain.Interfaces.Repositories;

namespace XiaomiReFund.Application.Commands.Refund.UpdateRefundStatus
{
    /// <summary>
    /// ตัวจัดการคำสั่งอัพเดตสถานะการคืนเงิน
    /// </summary>
    public class UpdateRefundStatusCommandHandler : IRequestHandler<UpdateRefundStatusCommand, ApiResponse<bool>>
    {
        private readonly IRefundRepository _refundRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILoggerService _logger;
        private readonly ICallbackService _callbackService;
        private readonly CallbackSettings _callbackSettings;

        /// <summary>
        /// สร้าง UpdateRefundStatusCommandHandler ใหม่
        /// </summary>
        /// <param name="refundRepository">คลังข้อมูลการคืนเงิน</param>
        /// <param name="currentUserService">บริการข้อมูลผู้ใช้ปัจจุบัน</param>
        /// <param name="logger">บริการบันทึกข้อมูล</param>
        /// <param name="callbackService">บริการ callback</param>
        /// <param name="callbackSettings">การตั้งค่า callback</param>
        public UpdateRefundStatusCommandHandler(
            IRefundRepository refundRepository,
            ICurrentUserService currentUserService,
            ILoggerService logger,
            ICallbackService callbackService,
            IOptions<CallbackSettings> callbackSettings)
        {
            _refundRepository = refundRepository;
            _currentUserService = currentUserService;
            _logger = logger;
            _callbackService = callbackService;
            _callbackSettings = callbackSettings.Value;
        }

        /// <summary>
        /// ดำเนินการอัพเดตสถานะการคืนเงิน
        /// </summary>
        /// <param name="request">คำขออัพเดตสถานะการคืนเงิน</param>
        /// <param name="cancellationToken">โทเค็นการยกเลิก</param>
        /// <returns>ผลลัพธ์การอัพเดตสถานะการคืนเงิน</returns>
        public async Task<ApiResponse<bool>> Handle(UpdateRefundStatusCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogRequest("UpdateRefundStatusCommand", request, _currentUserService.UserId ?? 0);

                // ดึงข้อมูลรายการคืนเงิน
                var refund = await _refundRepository.GetByIdAsync(request.RefundID);
                if (refund == null)
                {
                    throw new ApiException("Refund not found", 404, "Not Found");
                }

                // อัพเดตสถานะ
                var result = await _refundRepository.UpdateRefundStatusAsync(
                    request.RefundID,
                    request.Status,
                    _currentUserService.UserId ?? 0);

                if (!result)
                {
                    throw new ApiException("Failed to update refund status", 500, "Internal Server Error");
                }

                // ถ้าสถานะเป็น Approved หรือ Rejected ให้ส่ง callback
                // สร้างคำขอ callback
                var callbackRequest = new SendCallbackRequest
                {
                    Status = request.Status == RefundConstants.TransactionStatus.Approved
                           ? RefundConstants.CallbackStatus.Approved
                           : RefundConstants.CallbackStatus.Rejected,
                    Msg = request.StatusRemark ?? (request.Status == RefundConstants.TransactionStatus.Approved
                           ? "Refund Approved"
                           : "Refund Rejected"),
                    TerminalID = refund.TerminalID,
                    TransactionDate = refund.TransactionDate.ToString("yyyy-MM-dd"),
                    TransactionID = refund.TransactionID,
                    RefundAmount = refund.RefundAmount,
                    RequestID = refund.RequestID,
                    PaymentType = refund.PaymentType
                };

                // ลองส่ง callback ก่อน
                var sendResult = await _callbackService.SendCallbackAsync(callbackRequest);

                // ถ้าส่งไม่สำเร็จ จึงเพิ่มเข้าคิว
                if (sendResult.StatusCode != 200 || sendResult.Status != "Accepted")
                {
                    var enqueueRequest = new EnqueueCallbackRequest
                    {
                        RefundID = refund.RefundID,
                        TerminalID = refund.TerminalID,
                        TransactionDate = refund.TransactionDate,
                        TransactionID = refund.TransactionID,
                        RefundAmount = refund.RefundAmount,
                        RequestID = refund.RequestID,
                        Status = callbackRequest.Status,
                        StatusMessage = callbackRequest.Msg,
                        PaymentType = refund.PaymentType,
                        RetryCount = 1, // เริ่มนับ retry เป็น 1 เพราะครั้งแรกลองส่งไปแล้ว
                        ScheduledTime = DateTime.Now.AddMinutes(_callbackSettings.RetryDelayMinutes)
                    };

                    await _callbackService.EnqueueCallbackAsync(enqueueRequest);
                }

                var response = ApiResponse<bool>.Success("Refund status updated successfully", true);
                _logger.LogResponse("UpdateRefundStatusCommand", response, _currentUserService.UserId ?? 0);

                return response;
            }
            catch (ApiException ex)
            {
                _logger.LogError("UpdateRefundStatusCommand", ex, _currentUserService.UserId ?? 0);

                return new ApiResponse<bool>
                {
                    Result = new ResultData
                    {
                        StatusCode = ex.StatusCode,
                        Status = ex.Status,
                        Msg = ex.Message
                    },
                    Data = false
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("UpdateRefundStatusCommand", ex, _currentUserService.UserId ?? 0);
                throw;
            }
        }
    }
}