using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaomiReFund.Application.Commands.Refund.UpdateRefundStatus
{
    /// <summary>
    /// ตัวตรวจสอบความถูกต้องของคำสั่งอัพเดตสถานะการคืนเงิน
    /// </summary>
    public class UpdateRefundStatusCommandValidator : AbstractValidator<UpdateRefundStatusCommand>
    {
        /// <summary>
        /// สร้าง UpdateRefundStatusCommandValidator ใหม่
        /// </summary>
        public UpdateRefundStatusCommandValidator()
        {
            RuleFor(v => v.RefundID)
                .GreaterThan(0).WithMessage("Refund ID is required and must be greater than 0");

            RuleFor(v => v.Status)
                .Must(BeValidStatus).WithMessage("Status must be 0 (Processing), 1 (Approved), or 2 (Rejected)");

            RuleFor(v => v.StatusRemark)
                .MaximumLength(255).WithMessage("Status remark must not exceed 255 characters");
        }

        /// <summary>
        /// ตรวจสอบว่าสถานะถูกต้องหรือไม่
        /// </summary>
        /// <param name="status">สถานะที่ต้องการตรวจสอบ</param>
        /// <returns>true ถ้าสถานะถูกต้อง, false ถ้าไม่ถูกต้อง</returns>
        private bool BeValidStatus(byte status)
        {
            return status == 0 || status == 1 || status == 2;
        }
    }
}
