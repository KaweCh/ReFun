using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaomiReFund.Application.Commands.Auth.RefreshToken
{
    /// <summary>
    /// ตัวตรวจสอบความถูกต้องของคำสั่งรีเฟรชโทเค็น
    /// </summary>
    public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
    {
        /// <summary>
        /// สร้าง RefreshTokenCommandValidator ใหม่
        /// </summary>
        public RefreshTokenCommandValidator()
        {
            RuleFor(v => v.UserID)
                .GreaterThan(0).WithMessage("UserID must be greater than 0");

            RuleFor(v => v.Token)
                .NotEmpty().WithMessage("Token is required");
        }
    }
}
