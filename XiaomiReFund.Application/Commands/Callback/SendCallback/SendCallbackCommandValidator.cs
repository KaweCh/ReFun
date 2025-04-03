using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaomiReFund.Application.Commands.Callback.SendCallback
{
    /// <summary>
    /// สร้าง SendCallbackCommandValidator ใหม่
    /// </summary>
    public SendCallbackCommandValidator()
    {
        RuleFor(v => v.Status)
            .NotEmpty().WithMessage("Status is required")
            .MaximumLength(20).WithMessage("Status must not exceed 20 characters")
            .Must(BeValidStatus).WithMessage("Status must be 'Approved' or 'Rejected'");

        RuleFor(v => v.Msg)
            .MaximumLength(255).WithMessage("Message must not exceed 255 characters");

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
    /// ตรวจสอบว่าสถานะถูกต้องหรือไม่
    /// </summary>
    /// <param name="status">สถานะที่ต้องการตรวจสอบ</param>
    /// <returns>true ถ้าสถานะถูกต้อง, false ถ้าไม่ถูกต้อง</returns>
    private bool BeValidStatus(string status)
    {
        return status == "Approved" || status == "Rejected";
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
