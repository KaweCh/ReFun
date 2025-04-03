using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XiaomiReFund.Application.DTOs.Auth;

namespace XiaomiReFund.Application.Commands.Auth.RefreshToken
{
    /// <summary>
    /// คำสั่งสำหรับการรีเฟรชโทเค็น
    /// </summary>
    public class RefreshTokenCommand : IRequest<RefreshTokenResponse>
    {
        /// <summary>
        /// รหัสผู้ใช้
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// โทเค็นปัจจุบัน
        /// </summary>
        public string Token { get; set; }
    }
}
