using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XiaomiReFund.Application.Common.Interfaces;
using XiaomiReFund.Application.Common.Models;
using XiaomiReFund.Application.DTOs.Refund;
using XiaomiReFund.Domain.Interfaces.Repositories;

namespace XiaomiReFund.Application.Queries.Refund.GetRefundsByDateRange
{
    /// <summary>
    /// ตัวจัดการคำสั่งดึงข้อมูลการคืนเงินตามช่วงวันที่
    /// </summary>
    public class GetRefundsByDateRangeQueryHandler : IRequestHandler<GetRefundsByDateRangeQuery, RefundListResponse>
    {
        private readonly IRefundRepository _refundRepository;
        private readonly IMapper _mapper;
        private readonly ILoggerService _logger;
        private readonly ICurrentUserService _currentUserService;

        /// <summary>
        /// สร้าง GetRefundsByDateRangeQueryHandler ใหม่
        /// </summary>
        /// <param name="refundRepository">คลังข้อมูลการคืนเงิน</param>
        /// <param name="mapper">ตัวแปลงข้อมูล</param>
        /// <param name="logger">บริการบันทึกข้อมูล</param>
        /// <param name="currentUserService">บริการข้อมูลผู้ใช้ปัจจุบัน</param>
        public GetRefundsByDateRangeQueryHandler(
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
        /// ดำเนินการดึงข้อมูลการคืนเงินตามช่วงวันที่
        /// </summary>
        /// <param name="request">คำขอดึงข้อมูลการคืนเงินตามช่วงวันที่</param>
        /// <param name="cancellationToken">โทเค็นการยกเลิก</param>
        /// <returns>รายการการคืนเงิน</returns>
        public async Task<RefundListResponse> Handle(GetRefundsByDateRangeQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogRequest("GetRefundsByDateRangeQuery", request, _currentUserService.UserId ?? 0);

                // ดึงข้อมูลการคืนเงินตามช่วงวันที่
                IReadOnlyList<Domain.Entities.rms_OrderRefund> refunds;

                // ถ้ามีการระบุสถานะ
                if (request.TxnStatus.HasValue)
                {
                    refunds = await _refundRepository.GetRefundsByDateRangeAndStatusAsync(
                        request.StartDate,
                        request.EndDate,
                        request.TxnStatus.Value);
                }
                else
                {
                    refunds = await _refundRepository.GetRefundsByDateRangeAsync(
                        request.StartDate,
                        request.EndDate);
                }

                // แปลงข้อมูลเป็น DTO
                var refundDtos = _mapper.Map<List<RefundStatusDto>>(refunds);

                // จัดทำการแบ่งหน้า
                var paginatedRefunds = PaginatedList<RefundStatusDto>.Create(
                    refundDtos,
                    request.PageNumber,
                    request.PageSize);

                // สร้างผลลัพธ์
                var response = new RefundListResponse
                {
                    Result = new ResultData
                    {
                        StatusCode = 200,
                        Status = "Success",
                        Msg = "Refunds retrieved successfully"
                    },
                    Refunds = paginatedRefunds.Items
                };

                _logger.LogResponse("GetRefundsByDateRangeQuery", response, _currentUserService.UserId ?? 0);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError("GetRefundsByDateRangeQuery", ex, _currentUserService.UserId ?? 0);

                return new RefundListResponse
                {
                    Result = new ResultData
                    {
                        StatusCode = 500,
                        Status = "Error",
                        Msg = "An error occurred while retrieving refunds"
                    },
                    Refunds = new List<RefundStatusDto>()
                };
            }
        }
    }
}
