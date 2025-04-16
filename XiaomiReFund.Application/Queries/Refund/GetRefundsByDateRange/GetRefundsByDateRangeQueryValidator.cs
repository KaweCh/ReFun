using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaomiReFund.Application.Queries.Refund.GetRefundsByDateRange
{
    /// <summary>
    /// ตัวตรวจสอบความถูกต้องของคำสั่งดึงข้อมูลการคืนเงินตามช่วงวันที่
    /// </summary>
    public class GetRefundsByDateRangeQueryValidator : AbstractValidator<GetRefundsByDateRangeQuery>
    {
        /// <summary>
        /// สร้าง GetRefundsByDateRangeQueryValidator ใหม่
        /// </summary>
        public GetRefundsByDateRangeQueryValidator()
        {
            RuleFor(x => x.StartDate)
                .LessThanOrEqualTo(x => x.EndDate)
                .WithMessage("Start date must be less than or equal to end date");

            RuleFor(x => x.EndDate)
                .GreaterThanOrEqualTo(x => x.StartDate)
                .WithMessage("End date must be greater than or equal to start date")
                .LessThanOrEqualTo(DateTime.Now.AddDays(1))
                .WithMessage("End date cannot be in the future");

            RuleFor(x => x.PageNumber)
                .GreaterThan(0)
                .WithMessage("Page number must be greater than 0");

            RuleFor(x => x.PageSize)
                .GreaterThan(0)
                .WithMessage("Page size must be greater than 0")
                .LessThanOrEqualTo(100)
                .WithMessage("Page size must not exceed 100");

            // TxnStatus is optional, but if provided, it must be a valid value
            When(x => x.TxnStatus.HasValue, () =>
            {
                RuleFor(x => x.TxnStatus.Value)
                    .InclusiveBetween((byte)0, (byte)2)
                    .WithMessage("Transaction status must be between 0 and 2");
            });
        }
    }
}
