using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XiaomiReFund.Application.DTOs.Auth;

namespace XiaomiReFund.Application.Interfaces.Services
{
    /// <summary>
    /// อินเทอร์เฟซสำหรับบริการยืนยันตัวตน
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// ยืนยันตัวตนผู้ใช้
        /// </summary>
        /// <param name="request">ข้อมูลคำขอยืนยันตัวตน</param>
        /// <returns>ผลลัพธ์การยืนยันตัวตน</returns>
        Task<AuthenticateResponse> AuthenticateAsync(AuthenticateRequest request);

        /// <summary>
        /// รีเฟรชโทเค็น
        /// </summary>
        /// <param name="request">ข้อมูลคำขอรีเฟรชโทเค็น</param>
        /// <returns>ผลลัพธ์การรีเฟรชโทเค็น</returns>
        Task<RefreshTokenResponse> RefreshTokenAsync(RefreshTokenRequest request);

        /// <summary>
        /// ตรวจสอบความถูกต้องของโทเค็น
        /// </summary>
        /// <param name="userId">รหัสผู้ใช้</param>
        /// <param name="token">โทเค็น</param>
        /// <returns>ผลการตรวจสอบความถูกต้อง</returns>
        Task<bool> ValidateTokenAsync(int userId, string token);
    }
}
