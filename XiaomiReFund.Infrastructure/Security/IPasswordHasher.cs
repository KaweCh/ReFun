using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using XiaomiReFund.Application.Interfaces.Security;
using XiaomiReFund.Domain.Constants;

namespace XiaomiReFund.Infrastructure.Security
{
    /// <summary>
    /// บริการเข้ารหัสรหัสผ่าน
    /// </summary>
    public class PasswordHasher : IPasswordHasher
    {
        private readonly ILogger<PasswordHasher> _logger;

        /// <summary>
        /// สร้าง PasswordHasher ใหม่
        /// </summary>
        /// <param name="logger">บริการบันทึกข้อมูล</param>
        public PasswordHasher(ILogger<PasswordHasher> logger)
        {
            _logger = logger;
        }

        /// <inheritdoc/>
        public string HashPassword(string password)
        {
            try
            {
                // สร้าง salt ใหม่
                string salt = GenerateSalt();

                // เข้ารหัสรหัสผ่านด้วย salt ที่สร้างขึ้น
                return HashPassword(password, salt);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error hashing password");
                throw;
            }
        }

        /// <inheritdoc/>
        public string HashPassword(string password, string salt)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException(nameof(password));

            if (string.IsNullOrEmpty(salt))
                throw new ArgumentNullException(nameof(salt));

            try
            {
                // แปลง salt เป็น byte array
                byte[] saltBytes = Convert.FromBase64String(salt);

                // สร้าง key derivation function (PBKDF2)
                using var pbkdf2 = new Rfc2898DeriveBytes(
                    password,
                    saltBytes,
                    SecurityConstants.PasswordHashing.Iterations,
                    HashAlgorithmName.SHA256);

                // สร้าง hash
                byte[] hash = pbkdf2.GetBytes(SecurityConstants.PasswordHashing.HashSize);

                // รวม salt และ hash เข้าด้วยกัน
                byte[] hashBytes = new byte[SecurityConstants.PasswordHashing.SaltSize + SecurityConstants.PasswordHashing.HashSize];
                Array.Copy(saltBytes, 0, hashBytes, 0, SecurityConstants.PasswordHashing.SaltSize);
                Array.Copy(hash, 0, hashBytes, SecurityConstants.PasswordHashing.SaltSize, SecurityConstants.PasswordHashing.HashSize);

                // แปลงเป็น base64 string
                return Convert.ToBase64String(hashBytes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error hashing password with salt");
                throw;
            }
        }

        /// <inheritdoc/>
        public bool VerifyPassword(string hashedPassword, string providedPassword)
        {
            if (string.IsNullOrEmpty(hashedPassword))
                throw new ArgumentNullException(nameof(hashedPassword));

            if (string.IsNullOrEmpty(providedPassword))
                throw new ArgumentNullException(nameof(providedPassword));

            try
            {
                // แปลง hash password เป็น byte array
                byte[] hashBytes = Convert.FromBase64String(hashedPassword);

                // แยก salt จาก hash
                byte[] salt = new byte[SecurityConstants.PasswordHashing.SaltSize];
                Array.Copy(hashBytes, 0, salt, 0, SecurityConstants.PasswordHashing.SaltSize);

                // แปลง salt เป็น base64 string
                string saltString = Convert.ToBase64String(salt);

                // สร้าง hash ใหม่จากรหัสผ่านที่ให้มา
                string newHashedPassword = HashPassword(providedPassword, saltString);

                // เปรียบเทียบ hash
                return newHashedPassword == hashedPassword;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error verifying password");
                return false;
            }
        }

        /// <inheritdoc/>
        public bool VerifyPassword(string hashedPassword, string providedPassword, string salt)
        {
            if (string.IsNullOrEmpty(hashedPassword))
                throw new ArgumentNullException(nameof(hashedPassword));

            if (string.IsNullOrEmpty(providedPassword))
                throw new ArgumentNullException(nameof(providedPassword));

            if (string.IsNullOrEmpty(salt))
                throw new ArgumentNullException(nameof(salt));

            try
            {
                // สร้าง hash ใหม่จากรหัสผ่านที่ให้มาและ salt ที่กำหนด
                string newHashedPassword = HashPassword(providedPassword, salt);

                // เปรียบเทียบ hash
                return newHashedPassword == hashedPassword;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error verifying password with salt");
                return false;
            }
        }

        /// <inheritdoc/>
        public string GenerateSalt()
        {
            try
            {
                // สร้าง salt ใหม่
                byte[] salt = new byte[SecurityConstants.PasswordHashing.SaltSize];
                using (var rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(salt);
                }

                // แปลงเป็น base64 string
                return Convert.ToBase64String(salt);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating salt");
                throw;
            }
        }
    }
}
