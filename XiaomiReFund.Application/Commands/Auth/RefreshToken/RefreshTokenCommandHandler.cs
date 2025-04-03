using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XiaomiReFund.Application.Common.Interfaces;
using XiaomiReFund.Application.DTOs.Auth;
using XiaomiReFund.Application.Interfaces.Services;

namespace XiaomiReFund.Application.Commands.Auth.RefreshToken
{
    /// <summary>
    /// ตัวจัดการคำสั่งรีเฟรชโทเค็น
    /// </summary>
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, RefreshTokenResponse>
    {
        private readonly IAuthService _authService;
        private readonly ILoggerService _logger;

        /// <summary>
        /// สร้าง RefreshTokenCommandHandler ใหม่
        /// </summary>
        /// <param name="authService">บริการยืนยันตัวตน</param>
        /// <param name="logger">บริการบันทึกข้อมูล</param>
        public RefreshTokenCommandHandler(
            IAuthService authService,
            ILoggerService logger)
        {
            _authService = authService;
            _logger = logger;
        }

        /// <summary>
        /// ดำเนินการรีเฟรชโทเค็น
        /// </summary>
        /// <param name="request">คำขอรีเฟรชโทเค็น</param>
        /// <param name="cancellationToken">โทเค็นการยกเลิก</param>
        /// <returns>ผลลัพธ์การรีเฟรชโทเค็น</returns>
        public async Task<RefreshTokenResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogRequest("RefreshTokenCommand", request, request.UserID);

                // สร้างคำขอรีเฟรชโทเค็น
                var refreshTokenRequest = new RefreshTokenRequest
                {
                    UserID = request.UserID,
                    Token = request.Token
                };

                // เรียกใช้บริการรีเฟรชโทเค็น
                var response = await _authService.RefreshTokenAsync(refreshTokenRequest);

                _logger.LogResponse("RefreshTokenCommand", response, request.UserID);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError("RefreshTokenCommand", ex, request.UserID);
                throw;
            }
        }
    }
}
