using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaomiReFund.Application.Common.Models
{
    public class JwtSettings
    {
        /// <summary>
        /// Secret Key สำหรับ JWT
        /// </summary>
        public string SecretKey { get; set; }

        /// <summary>
        /// Issuer สำหรับ JWT
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// Audience สำหรับ JWT
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// เวลาหมดอายุของโทเค็น (วินาที)
        /// </summary>
        public int ExpirationInSeconds { get; set; } = 14400; // 4 ชั่วโมง

        /// <summary>
        /// ต้องการตรวจสอบเวลาหมดอายุหรือไม่
        /// </summary>
        public bool ValidateLifetime { get; set; } = true;

        /// <summary>
        /// ต้องการตรวจสอบ issuer หรือไม่
        /// </summary>
        public bool ValidateIssuer { get; set; } = true;

        /// <summary>
        /// ต้องการตรวจสอบ audience หรือไม่
        /// </summary>
        public bool ValidateAudience { get; set; } = true;

        /// <summary>
        /// ต้องการตรวจสอบ signing key หรือไม่
        /// </summary>
        public bool ValidateIssuerSigningKey { get; set; } = true;

        /// <summary>
        /// ระยะเวลาผ่อนผันหลังหมดอายุ (วินาที)
        /// </summary>
        public int ClockSkew { get; set; } = 0;
    }
}
