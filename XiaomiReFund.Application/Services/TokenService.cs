using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using XiaomiReFund.Application.Common.Exceptions;
using XiaomiReFund.Application.Common.Interfaces;
using XiaomiReFund.Application.Interfaces.Services;
using XiaomiReFund.Domain.Interfaces.Repositories;

namespace XiaomiReFund.Application.Services
{
    /// <summary>
    /// บริการจัดการโทเค็น
    /// </summary>
    public class TokenService : ITokenService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly IClientRepository _clientRepository;
        private readonly IDateTime _dateTime;

        /// <summary>
        /// สร้าง TokenService ใหม่
        /// </summary>
        /// <param name="jwtSettings">การตั้งค่า JWT</param>
        /// <param name="clientRepository">คลังข้อมูลลูกค้า</param>
        /// <param name="dateTime">บริการวันเวลา</param>
        public TokenService(
            IOptions<JwtSettings> jwtSettings,
            IClientRepository clientRepository,
            IDateTime dateTime)
        {
            _jwtSettings = jwtSettings.Value;
            _clientRepository = clientRepository;
            _dateTime = dateTime;
        }

        /// <summary>
        /// สร้างโทเค็นใหม่
        /// </summary>
        /// <param name="userId">รหัสผู้ใช้</param>
        /// <returns>โทเค็นที่สร้างขึ้น</returns>
        public async Task<string> GenerateTokenAsync(int userId)
        {
            var client = await _clientRepository.GetByIdAsync(userId);
            if (client == null)
            {
                throw new ApiException("User not found", 404, "Not Found");
            }

            // สร้างโทเค็นแบบสุ่ม
            var token = GenerateRandomToken();

            // อัพเดทโทเค็นผ่าน repository
            // เนื่องจากเราไม่รู้โครงสร้างของ entity ที่แน่นอน
            // จึงเรียกใช้เมธอด UpdateTokenAsync ที่เราคาดว่ามีใน IClientRepository
            await _clientRepository.UpdateTokenAsync(userId, token);

            return token;
        }

        /// <summary>
        /// ตรวจสอบความถูกต้องของโทเค็น
        /// </summary>
        /// <param name="userId">รหัสผู้ใช้</param>
        /// <param name="token">โทเค็น</param>
        /// <returns>ผลการตรวจสอบความถูกต้อง</returns>
        public async Task<bool> ValidateTokenAsync(int userId, string token)
        {
            var client = await _clientRepository.GetByIdAsync(userId);
            if (client == null)
            {
                return false;
            }

            // เนื่องจากเราไม่รู้โครงสร้างของ entity ที่แน่นอน
            // จึงเรียกใช้เมธอด ValidateTokenAsync ที่เราคาดว่ามีใน IClientRepository
            return await _clientRepository.ValidateTokenAsync(userId, token);
        }

        /// <summary>
        /// รีเฟรชโทเค็น
        /// </summary>
        /// <param name="userId">รหัสผู้ใช้</param>
        /// <param name="currentToken">โทเค็นปัจจุบัน</param>
        /// <returns>โทเค็นใหม่</returns>
        public async Task<string> RefreshTokenAsync(int userId, string currentToken)
        {
            var client = await _clientRepository.GetByIdAsync(userId);
            if (client == null)
            {
                throw new ApiException("User not found", 404, "Not Found");
            }

            // ตรวจสอบโทเค็นปัจจุบัน
            var isValidToken = await ValidateTokenAsync(userId, currentToken);
            if (!isValidToken)
            {
                throw new ApiException("Invalid token", 401, "Unauthorized");
            }

            // สร้างโทเค็นใหม่
            var newToken = GenerateRandomToken();

            // อัพเดทโทเค็นผ่าน repository
            await _clientRepository.UpdateTokenAsync(userId, newToken);

            return newToken;
        }

        /// <summary>
        /// ยกเลิกโทเค็น
        /// </summary>
        /// <param name="userId">รหัสผู้ใช้</param>
        /// <param name="token">โทเค็น</param>
        /// <returns>ผลการยกเลิกโทเค็น</returns>
        public async Task<bool> RevokeTokenAsync(int userId, string token)
        {
            var client = await _clientRepository.GetByIdAsync(userId);
            if (client == null)
            {
                return false;
            }

            // ตรวจสอบโทเค็น
            var isValidToken = await ValidateTokenAsync(userId, token);
            if (!isValidToken)
            {
                return false;
            }

            // ยกเลิกโทเค็น (ส่ง null เพื่อยกเลิก)
            await _clientRepository.UpdateTokenAsync(userId, null);

            return true;
        }

        /// <summary>
        /// รับเวลาหมดอายุของโทเค็น (วินาที)
        /// </summary>
        /// <returns>เวลาหมดอายุในวินาที</returns>
        public int GetTokenExpirationTime()
        {
            return _jwtSettings.ExpirationInSeconds;
        }

        /// <summary>
        /// สร้างโทเค็นแบบสุ่ม
        /// </summary>
        /// <returns>โทเค็นแบบสุ่ม</returns>
        private string GenerateRandomToken()
        {
            using var rng = RandomNumberGenerator.Create();
            var bytes = new byte[32]; // 256 bits
            rng.GetBytes(bytes);

            // เพิ่มส่วนของเวลาปัจจุบันเพื่อให้แน่ใจว่าโทเค็นไม่ซ้ำกัน
            var timestamp = _dateTime.UtcNow.Ticks.ToString();
            var combined = bytes.Concat(Encoding.UTF8.GetBytes(timestamp)).ToArray();

            // เข้ารหัส base64 และลบอักขระพิเศษ
            return Convert.ToBase64String(combined)
                .Replace("/", "_")
                .Replace("+", "-")
                .Replace("=", "");
        }
    }
}
