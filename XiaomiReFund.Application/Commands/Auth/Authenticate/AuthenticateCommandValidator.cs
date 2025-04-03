using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaomiReFund.Application.Commands.Auth.Authenticate
{
    /// <summary>
    /// ตัวตรวจสอบความถูกต้องของคำสั่งยืนยันตัวตน
    /// </summary>
    public class AuthenticateCommandValidator : AbstractValidator<AuthenticateCommand>
    {
        /// <summary>
        /// สร้าง AuthenticateCommandValidator ใหม่
        /// </summary>
        public AuthenticateCommandValidator() {
            RuleFor(v => v.Username)
                    .NotEmpty().WithMessage("Username is required")
                    .MaximumLength(20).WithMessage("Username must not exceed 20 characters");

            RuleFor(v => v.Password)
                .NotEmpty().WithMessage("Password is required")
                .MaximumLength(50).WithMessage("Password must not exceed 50 characters");
        }
    }
}
