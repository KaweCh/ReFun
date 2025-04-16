using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaomiReFund.Application.Queries.Refund.GetRefundByTerminalAndRequestId
{
    /// <summary>
    /// ตัวตรวจสอบความถูกต้องของคำสั่งดึงข้อมูลการคืนเงินตาม Terminal และ Request ID
    /// </summary>
    public class GetRefundByTerminalAndRequestIdQueryValidator : AbstractValidator<GetRefundByTerminalAndRequestIdQuery>
    {
        /// <summary>
        /// สร้าง GetRefundByTerminalAndRequestIdQueryValidator ใหม่
        /// </summary>
        public GetRefundByTerminalAndRequestIdQueryValidator()
        {
            RuleFor(x => x.TerminalID)
                .NotEmpty().WithMessage("Terminal ID is required")
                .MaximumLength(8).WithMessage("Terminal ID must not exceed 8 characters");

            RuleFor(x => x.RequestID)
                .NotEmpty().WithMessage("Request ID is required")
                .MaximumLength(30).WithMessage("Request ID must not exceed 30 characters");
        }
    }
}
