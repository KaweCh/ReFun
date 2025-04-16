using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaomiReFund.Application.Queries.Refund.GetRefundById
{
    /// <summary>
    /// ตัวตรวจสอบความถูกต้องของคำสั่งดึงข้อมูลการคืนเงินตาม ID
    /// </summary>
    public class GetRefundByIdQueryValidator : AbstractValidator<GetRefundByIdQuery>
    {
        /// <summary>
        /// สร้าง GetRefundByIdQueryValidator ใหม่
        /// </summary>
        public GetRefundByIdQueryValidator()
        {
            RuleFor(x => x.RefundID)
                .GreaterThan(0).WithMessage("Refund ID must be greater than 0");
        }
    }
}
