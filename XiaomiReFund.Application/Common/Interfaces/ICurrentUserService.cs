using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaomiReFund.Application.Common.Interfaces
{
    // Interface for accessing current user information
    public interface ICurrentUserService
    {
        // User ID (nullable in case no user is logged in)
        int? UserId { get; }

        // Username of the current user
        string Username { get; }

        // Client ID (nullable, might be different from User ID)
        int? ClientId { get; }

        // Flag indicating whether the user is authenticated
        bool IsAuthenticated { get; }
    }
}

// Example implementation:
// public class CurrentUserService : ICurrentUserService
// {
//     private readonly IHttpContextAccessor _httpContextAccessor;
//
//     public CurrentUserService(IHttpContextAccessor httpContextAccessor)
//     {
//         _httpContextAccessor = httpContextAccessor;
//     }
//
//     public int? UserId => 
//         _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier) is Claim claim 
//             ? int.Parse(claim.Value) 
//             : null;
//
//     public string Username => 
//         _httpContextAccessor.HttpContext?.User.Identity?.Name;
//
//     public int? ClientId => 
//         _httpContextAccessor.HttpContext?.User.FindFirst("ClientId") is Claim claim
//             ? int.Parse(claim.Value)
//             : null;
//
//     public bool IsAuthenticated => 
//         _httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;
// }