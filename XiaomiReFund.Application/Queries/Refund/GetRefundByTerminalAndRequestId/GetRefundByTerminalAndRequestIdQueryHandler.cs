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

namespace XiaomiReFund.Application.Queries.Refund.GetRefundByTerminalAndRequestId
{
    /// <summary>
    /// ตัวจัดการคำสั่งดึงข้อมูลการคืนเงินตาม Terminal และ Request ID
    /// </summary>
    public class GetRefundByTerminalAndRequestIdQueryHandler : IRequestHandler<GetRefundByTerminalAndRequestIdQuery, ApiResponse<RefundStatusDto>>
    {
        private readonly IRefundRepository _refundRepository;
        private readonly IMapper _mapper;
        private readonly ILoggerService _logger;
        private readonly ICurrentUserService _currentUserService;

        /// <summary>
        /// สร้าง GetRefundByTerminalAndRequestIdQueryHandler ใหม่
        /// </summary>
        /// <param name="refundRepository">คลังข้อมูลการคืนเงิน</param>
        /// <param name="mapper">ตัวแปลงข้อมูล</param>
        /// <param name="logger">บริการบันทึกข้อมูล</param>
        /// <param name="currentUserService">บริการข้อมูลผู้ใช้ปัจจุบัน</param>
        public GetRefundByTerminalAndRequestIdQueryHandler(
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
        /// ดำเนินการดึงข้อมูลการคืนเงินตาม Terminal และ Request ID
        /// </summary>
        /// <param name="request">คำขอดึงข้อมูลการคืนเงินตาม Terminal และ Request ID</param>
        /// <param name="cancellationToken">โทเค็นการยกเลิก</param>
        /// <returns>ข้อมูลการคืนเงิน</returns>
        public async Task<ApiResponse<RefundStatusDto>> Handle(GetRefundByTerminalAndRequestIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogRequest("GetRefundByTerminalAndRequestIdQuery", request, _currentUserService.UserId ?? 0);

                // ดึงข้อมูลการคืนเงินตาม Terminal และ Request ID
                var refund = await _refundRepository.GetByTerminalAndRequestIdAsync(request.TerminalID, request.RequestID);
                if (refund == null)
                {
                    return ApiResponse<RefundStatusDto>.NotFound($"Refund with Terminal ID {request.TerminalID} and Request ID {request.RequestID} not found");
                }

                // แปลงข้อมูลเป็น DTO
                var refundDto = _mapper.Map<RefundStatusDto>(refund);

                var response = ApiResponse<RefundStatusDto>.Success("Refund found successfully", refundDto);
                _logger.LogResponse("GetRefundByTerminalAndRequestIdQuery", response, _currentUserService.UserId ?? 0);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError("GetRefundByTerminalAndRequestIdQuery", ex, _currentUserService.UserId ?? 0);
                return ApiResponse<RefundStatusDto>.InternalServerError("An error occurred while retrieving refund");
            }
        }
    }
}
