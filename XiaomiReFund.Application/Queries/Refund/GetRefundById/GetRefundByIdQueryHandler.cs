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

namespace XiaomiReFund.Application.Queries.Refund.GetRefundById
{
    /// <summary>
    /// ตัวจัดการคำสั่งดึงข้อมูลการคืนเงินตาม ID
    /// </summary>
    public class GetRefundByIdQueryHandler : IRequestHandler<GetRefundByIdQuery, ApiResponse<RefundStatusDto>>
    {
        private readonly IRefundRepository _refundRepository;
        private readonly IMapper _mapper;
        private readonly ILoggerService _logger;
        private readonly ICurrentUserService _currentUserService;

        /// <summary>
        /// สร้าง GetRefundByIdQueryHandler ใหม่
        /// </summary>
        /// <param name="refundRepository">คลังข้อมูลการคืนเงิน</param>
        /// <param name="mapper">ตัวแปลงข้อมูล</param>
        /// <param name="logger">บริการบันทึกข้อมูล</param>
        /// <param name="currentUserService">บริการข้อมูลผู้ใช้ปัจจุบัน</param>
        public GetRefundByIdQueryHandler(
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
        /// ดำเนินการดึงข้อมูลการคืนเงินตาม ID
        /// </summary>
        /// <param name="request">คำขอดึงข้อมูลการคืนเงินตาม ID</param>
        /// <param name="cancellationToken">โทเค็นการยกเลิก</param>
        /// <returns>ข้อมูลการคืนเงิน</returns>
        public async Task<ApiResponse<RefundStatusDto>> Handle(GetRefundByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogRequest("GetRefundByIdQuery", request, _currentUserService.UserId ?? 0);

                // ดึงข้อมูลการคืนเงินตาม ID
                var refund = await _refundRepository.GetByIdAsync(request.RefundID);
                if (refund == null)
                {
                    return ApiResponse<RefundStatusDto>.NotFound($"Refund with ID {request.RefundID} not found");
                }

                // แปลงข้อมูลเป็น DTO
                var refundDto = _mapper.Map<RefundStatusDto>(refund);

                var response = ApiResponse<RefundStatusDto>.Success("Refund found successfully", refundDto);
                _logger.LogResponse("GetRefundByIdQuery", response, _currentUserService.UserId ?? 0);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError("GetRefundByIdQuery", ex, _currentUserService.UserId ?? 0);
                return ApiResponse<RefundStatusDto>.InternalServerError("An error occurred while retrieving refund");
            }
        }
    }
}
