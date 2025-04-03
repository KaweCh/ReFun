using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaomiReFund.Application.Interfaces.Services
{
    /// <summary>
    /// อินเทอร์เฟซสำหรับบริการจัดการโทเค็น
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// สร้างโทเค็นใหม่
        /// </summary>
        /// <param name="userId">รหัสผู้ใช้</param>
        /// <returns>โทเค็นที่สร้างขึ้น</returns>
        Task<string> GenerateTokenAsync(int userId);

        /// <summary>
        /// ตรวจสอบความถูกต้องของโทเค็น
        /// </summary>
        /// <param name="userId">รหัสผู้ใช้</param>
        /// <param name="token">โทเค็น</param>
        /// <returns>ผลการตรวจสอบความถูกต้อง</returns>
        Task<bool> ValidateTokenAsync(int userId, string token);

        /// <summary>
        /// รีเฟรชโทเค็น
        /// </summary>
        /// <param name="userId">รหัสผู้ใช้</param>
        /// <param name="currentToken">โทเค็นปัจจุบัน</param>
        /// <returns>โทเค็นใหม่</returns>
        Task<string> RefreshTokenAsync(int userId, string currentToken);

        /// <summary>
        /// ยกเลิกโทเค็น
        /// </summary>
        /// <param name="userId">รหัสผู้ใช้</param>
        /// <param name="token">โทเค็น</param>
        /// <returns>ผลการยกเลิกโทเค็น</returns>
        Task<bool> RevokeTokenAsync(int userId, string token);

        /// <summary>
        /// รับเวลาหมดอายุของโทเค็น (วินาที)
        /// </summary>
        /// <returns>เวลาหมดอายุในวินาที</returns>
        int GetTokenExpirationTime();
    }
}
