using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaomiReFund.Application.Common.Behaviors
{
    // คลาส ValidationBehavior ทำหน้าที่ตรวจสอบความถูกต้องของคำร้องขอ
    // ใช้ FluentValidation เพื่อตรวจสอบข้อมูลก่อนประมวลผล
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        // คอลเลกชันของตัวตรวจสอบความถูกต้อง
        // แต่ละตัวจะมีกฎเฉพาะสำหรับการตรวจสอบคำร้องขอ
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        // Constructor รับตัวตรวจสอบผ่าน Dependency Injection
        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            // เก็บรายการตัวตรวจสอบทั้งหมด
            _validators = validators;
        }

        // เมธอดหลักสำหรับจัดการคำร้องขอ
        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            // ถ้าไม่มีตัวตรวจสอบ ให้ดำเนินการต่อทันที
            if (!_validators.Any())
            {
                return await next();
            }

            // สร้างบริบทการตรวจสอบสำหรับคำร้องขอ
            var context = new ValidationContext<TRequest>(request);

            // รันการตรวจสอบทั้งหมดแบบขนานกัน
            var validationResults = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(context, cancellationToken))
            );

            // รวบรวมข้อผิดพลาดจากการตรวจสอบ
            var failures = validationResults
                .SelectMany(r => r.Errors)  // แยกข้อผิดพลาดออกมา
                .Where(f => f != null)      // กรองเอาเฉพาะข้อผิดพลาดที่ไม่เป็น null
                .ToList();                  // แปลงเป็นลิสต์

            // ถ้ามีข้อผิดพลาด ให้โยนข้อยกเว้น
            if (failures.Any())
            {
                throw new ValidationException(failures);
            }

            // ถ้าไม่มีข้อผิดพลาด ให้ดำเนินการต่อ
            return await next();
        }
    }
}

// ตัวอย่างการสร้างตัวตรวจสอบ:
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

// ตัวอย่างการลงทะเบียนการใช้งาน:
// services.AddTransient(
//     typeof(IPipelineBehavior<,>), 
//     typeof(ValidationBehavior<,>)
// );

// ตัวอย่างข้อผิดพลาดที่อาจเกิดขึ้น:
// ValidationException: 
//   - 'Amount' must be greater than 0
//   - 'UserId' must not be empty