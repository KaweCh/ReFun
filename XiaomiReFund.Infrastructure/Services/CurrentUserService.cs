using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using XiaomiReFund.Application.Common.Interfaces;

namespace XiaomiReFund.Infrastructure.Services
{
    /// <summary>
    /// บริการข้อมูลผู้ใช้ปัจจุบัน
    /// </summary>
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// สร้าง CurrentUserService ใหม่
        /// </summary>
        /// <param name="httpContextAccessor">ตัวเข้าถึง HTTP context</param>
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <inheritdoc/>
        public int? UserId
        {
            get
            {
                // ดึงรหัสผู้ใช้จาก claim NameIdentifier
                var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int id))
                {
                    return id;
                }
                return null;
            }
        }

        /// <inheritdoc/>
        public string Username => _httpContextAccessor.HttpContext?.User.Identity?.Name;

        /// <inheritdoc/>
        public int? ClientId
        {
            get
            {
                // ดึงรหัสลูกค้าจาก claim ClientId
                var clientIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst("ClientId");
                if (clientIdClaim != null && int.TryParse(clientIdClaim.Value, out int id))
                {
                    return id;
                }
                return UserId; // ถ้าไม่มี claim ClientId ให้ใช้ UserId แทน
            }
        }

        /// <inheritdoc/>
        public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;
    }
}
