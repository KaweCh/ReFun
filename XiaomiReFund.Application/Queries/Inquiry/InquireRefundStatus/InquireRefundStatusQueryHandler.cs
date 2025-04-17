using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XiaomiReFund.Application.Common.Interfaces;
using XiaomiReFund.Application.Common.Models;
using XiaomiReFund.Application.DTOs.Inquiry;
using XiaomiReFund.Application.DTOs.Refund;
using XiaomiReFund.Domain.Constants;
using XiaomiReFund.Domain.Interfaces.Repositories;

namespace XiaomiReFund.Application.Queries.Inquiry.InquireRefundStatus
{
    /// <summary>
    /// ตัวจัดการคำสั่งสอบถามสถานะการคืนเงิน
    /// </summary>
    public class InquireRefundStatusQueryHandler : IRequestHandler<InquireRefundStatusQuery, InquireRefundStatusResponse>
    {
        private readonly IRefundRepository _refundRepository;
        private readonly IMapper _mapper;
        private readonly ILoggerService _logger;
        private readonly ICurrentUserService _currentUserService;

        /// <summary>
        /// สร้าง InquireRefundStatusQueryHandler ใหม่
        /// </summary>
        /// <param name="refundRepository">คลังข้อมูลการคืนเงิน</param>
        /// <param name="mapper">ตัวแปลงข้อมูล</param>
        /// <param name="logger">บริการบันทึกข้อมูล</param>
        /// <param name="currentUserService">บริการข้อมูลผู้ใช้ปัจจุบัน</param>
        public InquireRefundStatusQueryHandler(
            IRefundRepository refundRepository,
            IMapper mapper,
            ILoggerService logger,
            ICurrentUserService currentUserService)
        {
            _refundRepository = refundRepository;
            _mapper = mapper;
            _logger = logger;
            _currentUserService = currentUserService;
        }

        /// <summary>
        /// ดำเนินการสอบถามสถานะการคืนเงิน
        /// </summary>
        /// <param name="request">คำขอสอบถามสถานะการคืนเงิน</param>
        /// <param name="cancellationToken">โทเค็นการยกเลิก</param>
        /// <returns>ผลลัพธ์การสอบถามสถานะการคืนเงิน</returns>
        public async Task<InquireRefundStatusResponse> Handle(InquireRefundStatusQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogRequest("InquireRefundStatusQuery", request, _currentUserService.UserId ?? 0);

                // ดึงข้อมูลการคืนเงินตาม Terminal และ Request ID
                var refund = await _refundRepository.GetByTerminalAndRequestIdAsync(request.TerminalID, request.RequestID);

                // ถ้าไม่พบข้อมูล
                if (refund == null)
                {
                    return new InquireRefundStatusResponse
                    {
                        Result = new ResultData
                        {
                            StatusCode = 404,
                            Status = "Not Found",
                            Msg = "Refund not found"
                        },
                        Request = new RefundDto
                        {
                            TerminalID = request.TerminalID,
                            TransactionDate = request.TransactionDate,
                            TransactionID = request.TransactionID,
                            RefundAmount = request.RefundAmount,
                            RequestID = request.RequestID,
                            PaymentType = request.PaymentType
                        }
                    };
                }

                // สร้างข้อความตามสถานะ
                string statusMessage;
                switch (refund.TxnStatus)
                {
                    case RefundConstants.TransactionStatus.Approved:
                        statusMessage = RefundConstants.CallbackStatus.Approved;
                        break;
                    case RefundConstants.TransactionStatus.Rejected:
                        statusMessage = RefundConstants.CallbackStatus.Rejected;
                        break;
                    default:
                        statusMessage = RefundConstants.CallbackStatus.Processing;
                        break;
                }

                // สร้างผลลัพธ์
                var response = new InquireRefundStatusResponse
                {
                    Result = new ResultData
                    {
                        StatusCode = 200,
                        Status = "Accepted",
                        Msg = statusMessage
                    },
                    Request = new RefundDto
                    {
                        TerminalID = request.TerminalID,
                        TransactionDate = request.TransactionDate,
                        TransactionID = request.TransactionID,
                        RefundAmount = request.RefundAmount,
                        RequestID = request.RequestID,
                        PaymentType = request.PaymentType
                    }
                };

                _logger.LogResponse("InquireRefundStatusQuery", response, _currentUserService.UserId ?? 0);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError("InquireRefundStatusQuery", ex, _currentUserService.UserId ?? 0);

                return new InquireRefundStatusResponse
                {
                    Result = new ResultData
                    {
                        StatusCode = 500,
                        Status = "Error",
                        Msg = "An error occurred while inquiring refund status"
                    },
                    Request = new RefundDto
                    {
                        TerminalID = request.TerminalID,
                        TransactionDate = request.TransactionDate,
                        TransactionID = request.TransactionID,
                        RefundAmount = request.RefundAmount,
                        RequestID = request.RequestID,
                        PaymentType = request.PaymentType
                    }
                };
            }
        }
    }
}