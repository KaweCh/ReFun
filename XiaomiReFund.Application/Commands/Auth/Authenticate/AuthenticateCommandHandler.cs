using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XiaomiReFund.Application.Common.Interfaces;
using XiaomiReFund.Application.DTOs.Auth;
using XiaomiReFund.Application.Interfaces.Services;

namespace XiaomiReFund.Application.Commands.Auth.Authenticate
{
    /// <summary>
    /// ตัวจัดการคำสั่งยืนยันตัวตน
    /// </summary
    public class AuthenticateCommandHandler : IRequestHandler<AuthenticateCommand, AuthenticateResponse>
    {
        private readonly IAuthService _authService;
        private readonly ILoggerService _loggerService;

        /// <summary>
        /// สร้าง AuthenticateCommandHandler ใหม่
        /// </summary>
        /// <param name="authService">บริการยืนยันตัวตน</param>
        /// <param name="logger">บริการบันทึกข้อมูล</param>
        public AuthenticateCommandHandler(IAuthService authService, ILoggerService loggerService)
        {
            _authService = authService;
            _loggerService = loggerService;
        }

        /// <summary>
        /// ดำเนินการยืนยันตัวตน
        /// </summary>
        /// <param name="request">คำขอยืนยันตัวตน</param>
        /// <param name="cancellationToken">โทเค็นการยกเลิก</param>
        /// <returns>ผลลัพธ์การยืนยันตัวตน</returns>
        public async Task<AuthenticateResponse> Handle(AuthenticateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _loggerService.LogRequest("AuthenticateCommand", request, 0);

                // สร้างคำขอยืนยันตัวตน
                var authRequest = new AuthenticateRequest
                {
                    Username = request.Username,
                    Password = request.Password
                };

                // เรียกใช้บริการยืนยันตัวตน
                var response = await _authService.AuthenticateAsync(authRequest);

                _loggerService.LogResponse("AuthenticateCommand", response, 0);

                return response;
            }
            catch (Exception ex)
            {
                _loggerService.LogError("AuthenticateCommand", ex, 0);
                throw;
            }
        }
    }
}
