using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;

namespace XiaomiReFund.Application.Common.Exceptions
{
    // คลาสข้อยกเว้นสำหรับการตรวจสอบความถูกต้องของข้อมูล
    // จัดการข้อผิดพลาดจากการตรวจสอบข้อมูลด้วย FluentValidation
    public class ValidationException : Exception
    {
        // Constructor เริ่มต้น
        // สร้างข้อยกเว้นพร้อมข้อความเริ่มต้น
        public ValidationException()
            : base("One or more validation failures have occurred.")
        {
            // เริ่มต้นพจนานุกรมข้อผิดพลาดเป็นค่าว่าง
            Errors = new Dictionary<string, string[]>();
        }

        // Constructor รับรายการข้อผิดพลาดจาก FluentValidation
        public ValidationException(IEnumerable<ValidationFailure> failures)
            : this()
        {
            // จัดกลุ่มข้อผิดพลาดตามชื่อคุณสมบัติ
            Errors = failures
                .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
                .ToDictionary(
                    failureGroup => failureGroup.Key,      // คีย์คือชื่อคุณสมบัติ
                    failureGroup => failureGroup.ToArray() // ค่าคืออาร์เรย์ของข้อความผิดพลาด
                );
        }

        // พจนานุกรมเก็บข้อผิดพลาด
        // คีย์คือชื่อคุณสมบัติ ค่าคืออาร์เรย์ของข้อความผิดพลาด
        public IDictionary<string, string[]> Errors { get; }

        // เขียนทับเมธอด ToString เพื่อแสดงรายละเอียดข้อผิดพลาด
        public override string ToString()
        {
            return $"{base.ToString()}\nValidation errors: {string.Join(", ", Errors.SelectMany(e => e.Value).ToArray())}";
        }
    }
}

// ตัวอย่างการใช้งาน:
// public class CreateRefundCommandValidator : AbstractValidator<CreateRefundCommand>
// {
//     public CreateRefundCommandValidator()
//     {
//         RuleFor(x => x.Amount)
//             .GreaterThan(0).WithMessage("จำนวนเงินคืนต้องมากกว่า 0");
//         
//         RuleFor(x => x.UserId)
//             .NotEmpty().WithMessage("ต้องระบุรหัสผู้ใช้");
//     }
// }
//
// try 
// {
//     var validator = new CreateRefundCommandValidator();
//     var validationResult = validator.Validate(command);
//     
//     if (!validationResult.IsValid)
//     {
//         throw new ValidationException(validationResult.Errors);
//     }
// }
// catch (ValidationException ex)
// {
//     // จัดการข้อผิดพลาดการตรวจสอบ
//     foreach (var error in ex.Errors)
//     {
//         Console.WriteLine($"Property: {error.Key}, Errors: {string.Join(", ", error.Value)}");
//     }
// }