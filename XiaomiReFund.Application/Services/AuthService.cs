using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XiaomiReFund.Application.Common.Exceptions;
using XiaomiReFund.Application.Common.Interfaces;
using XiaomiReFund.Application.Common.Models;
using XiaomiReFund.Application.DTOs.Auth;
using XiaomiReFund.Application.Interfaces.Security;
using XiaomiReFund.Application.Interfaces.Services;
using XiaomiReFund.Domain.Interfaces.Repositories;

namespace XiaomiReFund.Application.Services
{
    /// <summary>
    /// บริการยืนยันตัวตน
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenService _tokenService;
        private readonly ILoggerService _logger;

        /// <summary>
        /// สร้าง AuthService ใหม่
        /// </summary>
        /// <param name="clientRepository">คลังข้อมูลลูกค้า</param>
        /// <param name="passwordHasher">บริการเข้ารหัสรหัสผ่าน</param>
        /// <param name="tokenService">บริการจัดการโทเค็น</param>
        /// <param name="logger">บริการบันทึกข้อมูล</param>
        public AuthService(
            IClientRepository clientRepository,
            IPasswordHasher passwordHasher,
            ITokenService tokenService,
            ILoggerService logger)
        {
            _clientRepository = clientRepository;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
            _logger = logger;
        }

        /// <summary>
        /// ยืนยันตัวตนผู้ใช้
        /// </summary>
        /// <param name="request">ข้อมูลคำขอยืนยันตัวตน</param>
        /// <returns>ผลลัพธ์การยืนยันตัวตน</returns>
        public async Task<AuthenticateResponse> AuthenticateAsync(AuthenticateRequest request)
        {
            try
            {
                // ตรวจสอบว่ามีรหัสผู้ใช้และรหัสผ่านหรือไม่
                if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
                {
                    _logger.LogAuthenticationFailure(request.Username, "Missing username or password");

                    return new AuthenticateResponse
                    {
                        Result = new ResultData
                        {
                            StatusCode = 400,
                            Status = "Bad Request",
                            Msg = "Username and password are required"
                        }
                    };
                }

                // ดึงข้อมูลลูกค้าจากฐานข้อมูล
                var client = await _clientRepository.GetByUsernameAsync(request.Username);
                if (client == null)
                {
                    _logger.LogAuthenticationFailure(request.Username, "User not found");

                    return new AuthenticateResponse
                    {
                        Result = new ResultData
                        {
                            StatusCode = 401,
                            Status = "Unauthorized",
                            Msg = "Invalid Username or Password"
                        }
                    };
                }

                // ตรวจสอบรหัสผ่าน - เราต้องใช้เมธอดที่เหมาะสมตามวิธีการเก็บรหัสผ่านในระบบ
                var isPasswordValid = await _clientRepository.ValidatePasswordAsync(
                    client.ClientID,
                    request.Password);

                if (!isPasswordValid)
                {
                    _logger.LogAuthenticationFailure(request.Username, "Invalid password");

                    return new AuthenticateResponse
                    {
                        Result = new ResultData
                        {
                            StatusCode = 401,
                            Status = "Unauthorized",
                            Msg = "Invalid Username or Password"
                        }
                    };
                }

                // สร้างโทเค็นใหม่
                var token = await _tokenService.GenerateTokenAsync(client.ClientID);

                // บันทึกการยืนยันตัวตนสำเร็จ
                _logger.LogAuthenticationSuccess(request.Username, client.ClientID);

                // สร้างผลลัพธ์
                return new AuthenticateResponse
                {
                    Result = new ResultData
                    {
                        StatusCode = 200,
                        Status = "Success",
                        Msg = "Authentication Successful"
                    },
                    UserID = client.ClientID,
                    Token = token,
                    Expire = _tokenService.GetTokenExpirationTime()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("Authentication", ex, 0);

                return new AuthenticateResponse
                {
                    Result = new ResultData
                    {
                        StatusCode = 500,
                        Status = "Internal Server Error",
                        Msg = "An error occurred during authentication"
                    }
                };
            }
        }

        /// <summary>
        /// รีเฟรชโทเค็น
        /// </summary>
        /// <param name="request">ข้อมูลคำขอรีเฟรชโทเค็น</param>
        /// <returns>ผลลัพธ์การรีเฟรชโทเค็น</returns>
        public async Task<RefreshTokenResponse> RefreshTokenAsync(RefreshTokenRequest request)
        {
            try
            {
                // ตรวจสอบความถูกต้องของโทเค็นปัจจุบัน
                var isValidToken = await _tokenService.ValidateTokenAsync(request.UserID, request.Token);
                if (!isValidToken)
                {
                    return new RefreshTokenResponse
                    {
                        Result = new ResultData
                        {
                            StatusCode = 401,
                            Status = "Unauthorized",
                            Msg = "Invalid Token or Token Expired"
                        }
                    };
                }

                // รีเฟรชโทเค็น
                var newToken = await _tokenService.RefreshTokenAsync(request.UserID, request.Token);

                // สร้างผลลัพธ์
                return new RefreshTokenResponse
                {
                    Result = new ResultData
                    {
                        StatusCode = 200,
                        Status = "Success",
                        Msg = "Token Refreshed Successfully"
                    },
                    UserID = request.UserID,
                    Token = newToken,
                    Expire = _tokenService.GetTokenExpirationTime()
                };
            }
            catch (ApiException ex)
            {
                return new RefreshTokenResponse
                {
                    Result = new ResultData
                    {
                        StatusCode = ex.StatusCode,
                        Status = ex.Status,
                        Msg = ex.Message
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("RefreshToken", ex, 0);

                return new RefreshTokenResponse
                {
                    Result = new ResultData
                    {
                        StatusCode = 500,
                        Status = "Internal Server Error",
                        Msg = "An error occurred during token refresh"
                    }
                };
            }
        }

        /// <summary>
        /// ตรวจสอบความถูกต้องของโทเค็น
        /// </summary>
        /// <param name="userId">รหัสผู้ใช้</param>
        /// <param name="token">โทเค็น</param>
        /// <returns>ผลการตรวจสอบความถูกต้อง</returns>
        public async Task<bool> ValidateTokenAsync(int userId, string token)
        {
            return await _tokenService.ValidateTokenAsync(userId, token);
        }
    }
}
