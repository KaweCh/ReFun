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

namespace XiaomiReFund.Application.Queries.PaymentType.GetPaymentTypes
{
    /// <summary>
    /// ตัวจัดการคำสั่งดึงประเภทการชำระเงินทั้งหมด
    /// </summary>
    public class GetPaymentTypesQueryHandler : IRequestHandler<GetPaymentTypesQuery, PaymentTypeListResponse>
    {
        private readonly IPaymentTypeRepository _paymentTypeRepository;
        private readonly IMapper _mapper;
        private readonly ILoggerService _logger;

        /// <summary>
        /// สร้าง GetPaymentTypesQueryHandler ใหม่
        /// </summary>
        /// <param name="paymentTypeRepository">คลังข้อมูลประเภทการชำระเงิน</param>
        /// <param name="mapper">ตัวแปลงข้อมูล</param>
        /// <param name="logger">บริการบันทึกข้อมูล</param>
        public GetPaymentTypesQueryHandler(
            IPaymentTypeRepository paymentTypeRepository,
            IMapper mapper,
            ILoggerService logger)
        {
            _paymentTypeRepository = paymentTypeRepository;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// ดำเนินการดึงประเภทการชำระเงินทั้งหมด
        /// </summary>
        /// <param name="request">คำขอดึงประเภทการชำระเงินทั้งหมด</param>
        /// <param name="cancellationToken">โทเค็นการยกเลิก</param>
        /// <returns>รายการประเภทการชำระเงินทั้งหมด</returns>
        public async Task<PaymentTypeListResponse> Handle(GetPaymentTypesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogRequest("GetPaymentTypesQuery", request, 0);

                // ดึงข้อมูลประเภทการชำระเงินที่เปิดใช้งาน
                var paymentTypes = await _paymentTypeRepository.GetActivePaymentTypesAsync();

                // แปลงข้อมูลเป็น DTO
                var paymentTypeDtos = _mapper.Map<List<PaymentTypeResponse>>(paymentTypes);

                // สร้างผลลัพธ์
                var response = new PaymentTypeListResponse
                {
                    Result = new ResultData
                    {
                        StatusCode = 200,
                        Status = "Success",
                        Msg = "Payment types retrieved successfully"
                    },
                    PaymentType = paymentTypeDtos
                };

                _logger.LogResponse("GetPaymentTypesQuery", response, 0);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError("GetPaymentTypesQuery", ex, 0);

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
