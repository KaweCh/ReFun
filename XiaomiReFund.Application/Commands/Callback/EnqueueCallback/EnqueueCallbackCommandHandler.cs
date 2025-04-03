using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XiaomiReFund.Application.Common.Interfaces;
using XiaomiReFund.Application.DTOs.Callback;
using XiaomiReFund.Application.Interfaces.Services;

namespace XiaomiReFund.Application.Commands.Callback.EnqueueCallback
{
    /// <summary>
    /// ตัวจัดการคำสั่งเพิ่ม callback เข้าคิว
    /// </summary>
    public class EnqueueCallbackCommandHandler : IRequestHandler<EnqueueCallbackCommand, EnqueueCallbackResponse>
    {
        private readonly ICallbackService _callbackService;
        private readonly ILoggerService _logger;

        /// <summary>
        /// สร้าง EnqueueCallbackCommandHandler ใหม่
        /// </summary>
        /// <param name="callbackService">บริการ callback</param>
        /// <param name="logger">บริการบันทึกข้อมูล</param>
        public EnqueueCallbackCommandHandler(
            ICallbackService callbackService,
            ILoggerService logger)
        {
            _callbackService = callbackService;
            _logger = logger;
        }

        /// <summary>
        /// ดำเนินการเพิ่ม callback เข้าคิว
        /// </summary>
        /// <param name="request">คำขอเพิ่ม callback เข้าคิว</param>
        /// <param name="cancellationToken">โทเค็นการยกเลิก</param>
        /// <returns>ผลลัพธ์การเพิ่ม callback เข้าคิว</returns>
        public async Task<EnqueueCallbackResponse> Handle(EnqueueCallbackCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogRequest("EnqueueCallbackCommand", request, 0);

                // สร้างคำขอเพิ่ม callback เข้าคิว
                var enqueueRequest = new EnqueueCallbackRequest
                {
                    RefundID = request.RefundID,
                    TerminalID = request.TerminalID,
                    TransactionDate = request.TransactionDate,
                    TransactionID = request.TransactionID,
                    RefundAmount = request.RefundAmount,
                    RequestID = request.RequestID,
                    Status = request.Status,
                    StatusMessage = request.StatusMessage,
                    PaymentType = request.PaymentType,
                    RetryCount = request.RetryCount,
                    ScheduledTime = request.ScheduledTime
                };

                // เรียกใช้บริการเพิ่ม callback เข้าคิว
                var response = await _callbackService.EnqueueCallbackAsync(enqueueRequest);

                _logger.LogResponse("EnqueueCallbackCommand", response, 0);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError("EnqueueCallbackCommand", ex, 0);
                throw;
            }
        }
    }
}
