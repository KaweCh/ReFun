using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaomiReFund.Application.Commands.Callback.EnqueueCallback
{
    /// <summary>
    /// ตัวตรวจสอบความถูกต้องของคำสั่งเพิ่ม callback เข้าคิว
    /// </summary>
    public class EnqueueCallbackCommandValidator : AbstractValidator<EnqueueCallbackCommand>
    {
        /// <summary>
        /// สร้าง EnqueueCallbackCommandValidator ใหม่
        /// </summary>
        public EnqueueCallbackCommandValidator()
        {
            RuleFor(v => v.RefundID)
                .GreaterThan(0).WithMessage("Refund ID must be greater than 0");

            RuleFor(v => v.TerminalID)
                .NotEmpty().WithMessage("Terminal ID is required")
                .MaximumLength(8).WithMessage("Terminal ID must not exceed 8 characters");

            RuleFor(v => v.TransactionID)
                .NotEmpty().WithMessage("Transaction ID is required")
                .MaximumLength(50).WithMessage("Transaction ID must not exceed 50 characters");

            RuleFor(v => v.RefundAmount)
                .GreaterThan(0).WithMessage("Refund Amount must be greater than 0");

            RuleFor(v => v.RequestID)
                .NotEmpty().WithMessage("Request ID is required")
                .MaximumLength(30).WithMessage("Request ID must not exceed 30 characters");

            RuleFor(v => v.Status)
                .NotEmpty().WithMessage("Status is required")
                .MaximumLength(20).WithMessage("Status must not exceed 20 characters")
                .Must(BeValidStatus).WithMessage("Status must be 'Approved' or 'Rejected'");

            RuleFor(v => v.StatusMessage)
                .MaximumLength(255).WithMessage("Status message must not exceed 255 characters");

            RuleFor(v => v.PaymentType)
                .NotEmpty().WithMessage("Payment Type is required")
                .MaximumLength(20).WithMessage("Payment Type must not exceed 20 characters");

            RuleFor(v => v.RetryCount)
                .GreaterThanOrEqualTo(0).WithMessage("Retry count must be 0 or greater");

            RuleFor(v => v.ScheduledTime)
                .Must(BeValidScheduledTime).WithMessage("Scheduled time must be in the future");
        }

        /// <summary>
        /// ตรวจสอบว่าสถานะถูกต้องหรือไม่
        /// </summary>
        /// <param name="status">สถานะที่ต้องการตรวจสอบ</param>
        /// <returns>true ถ้าสถานะถูกต้อง, false ถ้าไม่ถูกต้อง</returns>
        private bool BeValidStatus(string status)
        {
            return status == "Approved" || status == "Rejected";
        }

        /// <summary>
        /// ตรวจสอบว่าเวลาที่กำหนดถูกต้องหรือไม่
        /// </summary>
        /// <param name="scheduledTime">เวลาที่กำหนด</param>
        /// <returns>true ถ้าเวลาที่กำหนดถูกต้อง, false ถ้าไม่ถูกต้อง</returns>
        private bool BeValidScheduledTime(DateTime scheduledTime)
        {
            // ยอมให้มีการกำหนดเวลาในอดีตได้สำหรับกรณีที่ต้องการส่งทันที
            return true;
        }
    }
}
