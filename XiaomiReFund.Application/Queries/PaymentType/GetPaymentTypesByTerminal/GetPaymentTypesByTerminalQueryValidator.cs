using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaomiReFund.Application.Queries.PaymentType.GetPaymentTypesByTerminal
{
    /// <summary>
    /// ตัวตรวจสอบความถูกต้องของคำสั่งดึงประเภทการชำระเงินตาม Terminal
    /// </summary>
    public class GetPaymentTypesByTerminalQueryValidator : AbstractValidator<GetPaymentTypesByTerminalQuery>
    {
        /// <summary>
        /// สร้าง GetPaymentTypesByTerminalQueryValidator ใหม่
        /// </summary>
        public GetPaymentTypesByTerminalQueryValidator()
        {
            RuleFor(x => x.TerminalID)
                .NotEmpty().WithMessage("Terminal ID is required")
                .MaximumLength(8).WithMessage("Terminal ID must not exceed 8 characters");
        }
    }
}
