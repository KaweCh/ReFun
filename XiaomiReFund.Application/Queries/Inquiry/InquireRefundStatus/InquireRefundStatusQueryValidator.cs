using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaomiReFund.Application.Queries.Inquiry.InquireRefundStatus
{
    /// <summary>
    /// ตัวตรวจสอบความถูกต้องของคำสั่งสอบถามสถานะการคืนเงิน
    /// </summary>
    public class InquireRefundStatusQueryValidator : AbstractValidator<InquireRefundStatusQuery>
    {
        /// <summary>
        /// สร้าง InquireRefundStatusQueryValidator ใหม่
        /// </summary>
        public InquireRefundStatusQueryValidator()
        {
            RuleFor(x => x.TerminalID)
                .NotEmpty().WithMessage("Terminal ID is required")
                .MaximumLength(8).WithMessage("Terminal ID must not exceed 8 characters");

            RuleFor(x => x.TransactionDate)
                .NotEmpty().WithMessage("Transaction Date is required")
                .MaximumLength(10).WithMessage("Transaction Date must not exceed 10 characters")
                .Matches(@"^\d{4}-\d{2}-\d{2}$").WithMessage("Transaction Date must be in format YYYY-MM-DD")
                .Must(BeValidDate).WithMessage("Transaction Date must be a valid date");

            RuleFor(x => x.TransactionID)
                .NotEmpty().WithMessage("Transaction ID is required")
                .MaximumLength(50).WithMessage("Transaction ID must not exceed 50 characters");

            RuleFor(x => x.RefundAmount)
                .GreaterThan(0).WithMessage("Refund Amount must be greater than 0");

            RuleFor(x => x.RequestID)
                .NotEmpty().WithMessage("Request ID is required")
                .MaximumLength(30).WithMessage("Request ID must not exceed 30 characters");

            RuleFor(x => x.PaymentType)
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
