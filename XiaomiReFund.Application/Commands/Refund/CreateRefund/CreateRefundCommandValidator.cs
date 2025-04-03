using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaomiReFund.Application.Commands.Refund.CreateRefund
{
    /// <summary>
    /// ตัวตรวจสอบความถูกต้องของคำสั่งสร้างคำขอคืนเงิน
    /// </summary>
    public class CreateRefundCommandValidator : AbstractValidator<CreateRefundCommand>
    {
        /// <summary>
        /// สร้าง CreateRefundCommandValidator ใหม่
        /// </summary>
        public CreateRefundCommandValidator()
        {
            RuleFor(v => v.TerminalID)
                .NotEmpty().WithMessage("Terminal ID is required")
                .MaximumLength(8).WithMessage("Terminal ID must not exceed 8 characters");

            RuleFor(v => v.TransactionDate)
                .NotEmpty().WithMessage("Transaction Date is required")
                .MaximumLength(10).WithMessage("Transaction Date must not exceed 10 characters")
                .Matches(@"^\d{4}-\d{2}-\d{2}$").WithMessage("Transaction Date must be in format YYYY-MM-DD")
                .Must(BeValidDate).WithMessage("Transaction Date must be a valid date");

            RuleFor(v => v.TransactionID)
                .NotEmpty().WithMessage("Transaction ID is required")
                .MaximumLength(50).WithMessage("Transaction ID must not exceed 50 characters");

            RuleFor(v => v.RefundAmount)
                .GreaterThan(0).WithMessage("Refund Amount must be greater than 0");

            RuleFor(v => v.RequestID)
                .NotEmpty().WithMessage("Request ID is required")
                .MaximumLength(30).WithMessage("Request ID must not exceed 30 characters");

            RuleFor(v => v.PaymentType)
                .NotEmpty().WithMessage("Payment Type is required")
                .MaximumLength(20).WithMessage("Payment Type must not exceed 20 characters");
        }

        /// <summary>
        /// ตรวจสอบว่าวันที่ถูกต้องหรือไม่
        /// </summary>
        /// <param name="dateString">วันที่ในรูปแบบข้อความ</param>
        /// <returns>true ถ้าวันที่ถูกต้อง, false ถ้าไม่ถูกต้อง</returns>
        private bool BeValidDate(string dateString)
        {
            if (string.IsNullOrEmpty(dateString))
                return false;

            return DateTime.TryParse(dateString, out _);
        }
    }
}
