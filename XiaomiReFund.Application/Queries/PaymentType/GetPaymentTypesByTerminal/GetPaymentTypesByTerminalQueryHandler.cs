using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XiaomiReFund.Application.Common.Interfaces;
using XiaomiReFund.Application.Common.Models;
using XiaomiReFund.Application.DTOs.PaymentType;
using XiaomiReFund.Domain.Interfaces.Repositories;

namespace XiaomiReFund.Application.Queries.PaymentType.GetPaymentTypesByTerminal
{
    /// <summary>
    /// ตัวจัดการคำสั่งดึงประเภทการชำระเงินตาม Terminal
    /// </summary>
    public class GetPaymentTypesByTerminalQueryHandler : IRequestHandler<GetPaymentTypesByTerminalQuery, PaymentTypeListResponse>
    {
        private readonly IPaymentTypeRepository _paymentTypeRepository;
        private readonly IMapper _mapper;
        private readonly ILoggerService _logger;
        private readonly ICurrentUserService _currentUserService;

        /// <summary>
        /// สร้าง GetPaymentTypesByTerminalQueryHandler ใหม่
        /// </summary>
        /// <param name="paymentTypeRepository">คลังข้อมูลประเภทการชำระเงิน</param>
        /// <param name="mapper">ตัวแปลงข้อมูล</param>
        /// <param name="logger">บริการบันทึกข้อมูล</param>
        /// <param name="currentUserService">บริการข้อมูลผู้ใช้ปัจจุบัน</param>
        public GetPaymentTypesByTerminalQueryHandler(
            IPaymentTypeRepository paymentTypeRepository,
            IMapper mapper,
            ILoggerService logger,
            ICurrentUserService currentUserService)
        {
            _paymentTypeRepository = paymentTypeRepository;
            _mapper = mapper;
            _logger = logger;
            _currentUserService = currentUserService;
        }

        /// <summary>
        /// ดำเนินการดึงประเภทการชำระเงินตาม Terminal
        /// </summary>
        /// <param name="request">คำขอดึงประเภทการชำระเงินตาม Terminal</param>
        /// <param name="cancellationToken">โทเค็นการยกเลิก</param>
        /// <returns>รายการประเภทการชำระเงินตาม Terminal</returns>
        public async Task<PaymentTypeListResponse> Handle(GetPaymentTypesByTerminalQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogRequest("GetPaymentTypesByTerminalQuery", request, _currentUserService.UserId ?? 0);

                // ตรวจสอบว่ามีการระบุเทอร์มินัลหรือไม่
                if (string.IsNullOrEmpty(request.TerminalID))
                {
                    return new PaymentTypeListResponse
                    {
                        Result = new ResultData
                        {
                            StatusCode = 400,
                            Status = "Bad Request",
                            Msg = "Terminal ID is required"
                        },
                        PaymentType = new List<PaymentTypeResponse>()
                    };
                }

                // ดึงข้อมูลประเภทการชำระเงินตามเทอร์มินัล
                var paymentTypes = await _paymentTypeRepository.GetPaymentTypesByTerminalAsync(request.TerminalID);

                // แปลงข้อมูลเป็น DTO
                var paymentTypeDtos = _mapper.Map<List<PaymentTypeResponse>>(paymentTypes);

                // สร้างผลลัพธ์
                var response = new PaymentTypeListResponse
                {
                    Result = new ResultData
                    {
                        StatusCode = 200,
                        Status = "Accepted",
                        Msg = "Your request has been accepted"
                    },
                    PaymentType = paymentTypeDtos
                };

                _logger.LogResponse("GetPaymentTypesByTerminalQuery", response, _currentUserService.UserId ?? 0);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError("GetPaymentTypesByTerminalQuery", ex, _currentUserService.UserId ?? 0);

                return new PaymentTypeListResponse
                {
                    Result = new ResultData
                    {
                        StatusCode = 500,
                        Status = "Error",
                        Msg = "An error occurred while retrieving payment types"
                    },
                    PaymentType = new List<PaymentTypeResponse>()
                };
            }
        }
    }
}
