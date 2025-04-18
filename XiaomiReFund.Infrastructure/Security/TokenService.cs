using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using XiaomiReFund.Application.Common.Interfaces;
using XiaomiReFund.Application.Interfaces.Services;
using XiaomiReFund.Domain.Interfaces.Repositories;
using XiaomiReFund.Infrastructure.Settings;

namespace XiaomiReFund.Infrastructure.Security
{
    /// <summary>
    /// บริการจัดการโทเค็น
    /// </summary>
    public class TokenService : ITokenService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly IClientRepository _clientRepository;
        private readonly IDateTime _dateTime;
        private readonly ILogger<TokenService> _logger;

        /// <summary>
        /// สร้าง TokenService ใหม่
        /// </summary>
        /// <param name="jwtSettings">การตั้งค่า JWT</param>
        /// <param name="clientRepository">คลังข้อมูลลูกค้า</param>
        /// <param name="dateTime">บริการวันเวลา</param>
        /// <param name="logger">บริการบันทึกข้อมูล</param>
        public TokenService(
            IOptions<JwtSettings> jwtSettings,
            IClientRepository clientRepository,
            IDateTime dateTime,
            ILogger<TokenService> logger)
        {
            _jwtSettings = jwtSettings.Value;
            _clientRepository = clientRepository;
            _dateTime = dateTime;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<string> GenerateTokenAsync(int userId)
        {
            try
            {
                // ตรวจสอบว่าพบลูกค้าหรือไม่
                var client = await _clientRepository.GetByIdAsync(userId);
                if (client == null)
                {
                    _logger.LogWarning("User not found when generating token: {UserId}", userId);
                    throw new InvalidOperationException($"User with ID {userId} not found");
                }

                // สร้างโทเค็นแบบสุ่ม
                var token = GenerateRandomToken();

                // อัพเดทโทเค็นผ่าน repository
                await _clientRepository.UpdateTokenAsync(userId, token);

                return token;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating token for user {UserId}", userId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<bool> ValidateTokenAsync(int userId, string token)
        {
            if (string.IsNullOrEmpty(token))
                return false;

            try
            {
                // ตรวจสอบว่าพบลูกค้าหรือไม่
                var client = await _clientRepository.GetByIdAsync(userId);
                if (client == null)
                {
                    _logger.LogWarning("User not found when validating token: {UserId}", userId);
                    return false;
                }

                // เรียกใช้เมธอดจากคลังข้อมูลลูกค้า
                return await _clientRepository.ValidateTokenAsync(userId, token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating token for user {UserId}", userId);
                return false;
            }
        }

        /// <inheritdoc/>
        public async Task<string> RefreshTokenAsync(int userId, string currentToken)
        {
            try
            {
                // ตรวจสอบว่าพบลูกค้าหรือไม่
                var client = await _clientRepository.GetByIdAsync(userId);
                if (client == null)
                {
                    _logger.LogWarning("User not found when refreshing token: {UserId}", userId);
                    throw new InvalidOperationException($"User with ID {userId} not found");
                }

                // ตรวจสอบโทเค็นปัจจุบัน
                var isValidToken = await ValidateTokenAsync(userId, currentToken);
                if (!isValidToken)
                {
                    _logger.LogWarning("Invalid token when refreshing for user {UserId}", userId);
                    throw new InvalidOperationException("Invalid token");
                }

                // สร้างโทเค็นใหม่
                var newToken = GenerateRandomToken();

                // อัพเดทโทเค็นผ่าน repository
                await _clientRepository.UpdateTokenAsync(userId, newToken);

                return newToken;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error refreshing token for user {UserId}", userId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<bool> RevokeTokenAsync(int userId, string token)
        {
            try
            {
                // ตรวจสอบว่าพบลูกค้าหรือไม่
                var client = await _clientRepository.GetByIdAsync(userId);
                if (client == null)
                {
                    _logger.LogWarning("User not found when revoking token: {UserId}", userId);
                    return false;
                }

                // ตรวจสอบโทเค็น
                var isValidToken = await ValidateTokenAsync(userId, token);
                if (!isValidToken)
                {
                    _logger.LogWarning("Invalid token when revoking for user {UserId}", userId);
                    return false;
                }

                // ยกเลิกโทเค็น (ส่ง null เพื่อยกเลิก)
                await _clientRepository.UpdateTokenAsync(userId, null);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error revoking token for user {UserId}", userId);
                return false;
            }
        }

        /// <inheritdoc/>
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
