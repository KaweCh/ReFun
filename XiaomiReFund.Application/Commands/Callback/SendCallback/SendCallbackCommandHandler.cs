using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XiaomiReFund.Application.Common.Interfaces;
using XiaomiReFund.Application.DTOs.Callback;
using XiaomiReFund.Application.Interfaces.Services;

namespace XiaomiReFund.Application.Commands.Callback.SendCallback
{
    /// <summary>
    /// ตัวจัดการคำสั่งส่ง callback
    /// </summary>
    public class SendCallbackCommandHandler : IRequestHandler<SendCallbackCommand, CallbackResponse>
    {
        private readonly ICallbackService _callbackService;
        private readonly ILoggerService _logger;

        /// <summary>
        /// สร้าง SendCallbackCommandHandler ใหม่
        /// </summary>
        /// <param name="callbackService">บริการ callback</param>
        /// <param name="logger">บริการบันทึกข้อมูล</param>
        public SendCallbackCommandHandler(
            ICallbackService callbackService,
            ILoggerService logger)
        {
            _callbackService = callbackService;
            _logger = logger;
        }

        /// <summary>
        /// ดำเนินการส่ง callback
        /// </summary>
        /// <param name="request">คำขอส่ง callback</param>
        /// <param name="cancellationToken">โทเค็นการยกเลิก</param>
        /// <returns>ผลลัพธ์การส่ง callback</returns>
        public async Task<CallbackResponse> Handle(SendCallbackCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogRequest("SendCallbackCommand", request, 0);

                // สร้างคำขอส่ง callback
                var callbackRequest = new SendCallbackRequest
                {
                    Status = request.Status,
                    Msg = request.Msg,
                    TerminalID = request.TerminalID,
                    TransactionDate = request.TransactionDate,
                    TransactionID = request.TransactionID,
                    RefundAmount = request.RefundAmount,
                    RequestID = request.RequestID,
                    PaymentType = request.PaymentType
                };

                // เรียกใช้บริการส่ง callback
                var response = await _callbackService.SendCallbackAsync(callbackRequest);

                _logger.LogResponse("SendCallbackCommand", response, 0);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError("SendCallbackCommand", ex, 0);
                throw;
            }
        }
    }
}
