using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using XiaomiReFund.Application.Common.Models;
using XiaomiReFund.Application.DTOs.Auth;

namespace XiaomiReFund.Application.Commands.Auth.Authenticate
{
    /// <summary>
    /// คำสั่งสำหรับการยืนยันตัวตน
    /// </summary>
    public class AuthenticateCommand : IRequest<AuthenticateResponse>
    {
        /// <summary>
        /// ชื่อผู้ใช้
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// รหัสผ่าน
        /// </summary>
        public string Password { get; set; }
    }
}
