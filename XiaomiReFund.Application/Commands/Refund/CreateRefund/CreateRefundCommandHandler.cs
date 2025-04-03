using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XiaomiReFund.Application.Common.Exceptions;
using XiaomiReFund.Application.Common.Interfaces;
using XiaomiReFund.Application.Common.Models;
using XiaomiReFund.Application.DTOs.Refund;
using XiaomiReFund.Domain.Interfaces.Repositories;

namespace XiaomiReFund.Application.Commands.Refund.CreateRefund
{
    public class CreateRefundCommandHandler : IRequestHandler<CreateRefundCommand, CreateRefundResponse>
    {
        private readonly IRefundRepository _refundRepository;
        private readonly IPaymentTypeRepository _paymentTypeRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILoggerService _loggerService;

        /// <summary>
        /// สร้าง CreateRefundCommandHandler ใหม่
        /// </summary>
        /// <param name="refundRepository">คลังข้อมูลการคืนเงิน</param>
        /// <param name="paymentTypeRepository">คลังข้อมูลประเภทการชำระเงิน</param>
        /// <param name="currentUserService">บริการข้อมูลผู้ใช้ปัจจุบัน</param>
        /// <param name="logger">บริการบันทึกข้อมูล</param>
        public CreateRefundCommandHandler(IRefundRepository refundRepository, IPaymentTypeRepository paymentTypeRepository, ICurrentUserService currentUserService, ILoggerService loggerService)
        {
            _refundRepository = refundRepository;
            _paymentTypeRepository = paymentTypeRepository;
            _currentUserService = currentUserService;
            _loggerService = loggerService;
        }

        /// <summary>
        /// ดำเนินการสร้างคำขอคืนเงิน
        /// </summary>
        /// <param name="request">คำขอสร้างการคืนเงิน</param>
        /// <param name="cancellationToken">โทเค็นการยกเลิก</param>
        /// <returns>ผลลัพธ์การสร้างคำขอคืนเงิน</returns>
        public async Task<CreateRefundResponse> Handle(CreateRefundCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _loggerService.LogRequest("CreateRefundCommand", request, _currentUserService.UserId ?? 0);

                // ตรวจสอบว่ามีคำขอนี้อยู่แล้วหรือไม่
                var existingRefund = await _refundRepository.GetByTerminalAndRequestIdAsync(request.TerminalID, request.RequestID);

                if (existingRefund != null)
                {
                    // คำขอนี้มีอยู่แล้ว
                    return new CreateRefundResponse
                    {
                        Result = new ResultData
                        {
                            StatusCode = 409,
                            Status = "Existed",
                            Msg = "Already Requested"
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

                // ตรวจสอบว่าประเภทการชำระเงินถูกต้องหรือไม่
                var paymentType = await _paymentTypeRepository.GetPaymentTypeByCodeAsync(request.PaymentType);
                if (paymentType == null)
                {
                    throw new ApiException("Invalid Payment Type", 400, "Bad Request");
                }

                // ตรวจสอบว่าประเภทการชำระเงินเปิดใช้งานสำหรับเทอร์มินัลนี้หรือไม่
                var isAllowed = await _paymentTypeRepository.IsPaymentTypeAllowedForTerminalAsync(
                    request.TerminalID, request.PaymentType);
                if (!isAllowed)
                {
                    throw new ApiException("Payment Type not allowed for this terminal", 400, "Bad Request");
                }

                // แปลงวันที่ทำรายการ
                if (!DateTime.TryParse(request.TransactionDate, out DateTime transactionDate))
                {
                    throw new ApiException("Invalid Transaction Date format", 400, "Bad Request");
                }

                // สร้างรายการคืนเงินใหม่
                var refundId = await _refundRepository.CreateRefundAsync(
                    request.TerminalID,
                    transactionDate,
                    request.TransactionID,
                    request.PaymentType,
                    request.RefundAmount,
                    request.RequestID,
                    _currentUserService.ClientId ?? 0,
                    _currentUserService.UserId ?? 0);

                // สร้างผลลัพธ์
                var response = new CreateRefundResponse
                {
                    Result = new ResultData
                    {
                        StatusCode = 200,
                        Status = "Accepted",
                        Msg = "Your request has been accepted"
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

                _loggerService.LogResponse("CreateRefundCommand", response, _currentUserService.UserId ?? 0);

                return response;
            }
            catch (ApiException ex)
            {
                _loggerService.LogError("CreateRefundCommand", ex, _currentUserService.UserId ?? 0);

                return new CreateRefundResponse
                {
                    Result = new ResultData
                    {
                        StatusCode = ex.StatusCode,
                        Status = ex.Status,
                        Msg = ex.Message
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
            catch (Exception ex)
            {
                _loggerService.LogError("CreateRefundCommand", ex, _currentUserService.UserId ?? 0);
                throw;
            }
        }
    }
}
