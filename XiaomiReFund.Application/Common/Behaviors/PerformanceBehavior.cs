using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XiaomiReFund.Application.Common.Interfaces;

namespace XiaomiReFund.Application.Common.Behaviors
{
    // คลาส PerformanceBehavior ทำหน้าที่ตรวจสอบประสิทธิภาพของคำร้องขอ
    // สามารถระบุและบันทึกคำร้องขอที่ใช้เวลานานเกินกว่ากำหนด
    public class PerformanceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        // Stopwatch ใช้จับเวลาการประมวลผลคำร้องขอ
        private readonly Stopwatch _timer;

        // Logger มาตรฐานของ .NET สำหรับบันทึกข้อมูล
        private readonly ILogger<PerformanceBehavior<TRequest, TResponse>> _logger;

        // บริการดึงข้อมูลผู้ใช้ปัจจุบัน
        private readonly ICurrentUserService _currentUserService;

        // บริการบันทึกログแบบกำหนดเอง
        private readonly ILoggerService _loggerService;

        // Constructor รับ dependencies ผ่าน Dependency Injection
        public PerformanceBehavior(
            ILogger<PerformanceBehavior<TRequest, TResponse>> logger,
            ICurrentUserService currentUserService,
            ILoggerService loggerService)
        {
            // สร้าง Stopwatch ใหม่
            _timer = new Stopwatch();

            // กำหนดค่า dependencies
            _logger = logger;
            _currentUserService = currentUserService;
            _loggerService = loggerService;
        }

        // เมธอดหลักสำหรับจัดการคำร้องขอ
        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            // เริ่มจับเวลา
            _timer.Start();

            // ประมวลผลคำร้องขอ
            var response = await next();

            // หยุดจับเวลา
            _timer.Stop();

            // คำนวณเวลาที่ใช้ไป (มิลลิวินาที)
            var elapsedMilliseconds = _timer.ElapsedMilliseconds;

            // ตรวจสอบว่าคำร้องขอใช้เวลานานเกิน 500 มิลลิวินาทีหรือไม่
            if (elapsedMilliseconds > 500)
            {
                // ดึงชื่อประเภทของคำร้องขอ
                var requestName = typeof(TRequest).Name;

                // ดึง UserID (ถ้าไม่มีให้ใช้ 0)
                var userId = _currentUserService.UserId ?? 0;

                // บันทึกคำเตือนการทำงานที่ช้า ด้วย .NET Logger
                _logger.LogWarning(
                    "Long running request: {RequestName} ({ElapsedMilliseconds} milliseconds) for user {UserId}",
                    requestName, elapsedMilliseconds, userId
                );

                // บันทึกคำเตือนการทำงานที่ช้า ด้วย Custom Logger Service
                _loggerService.LogPerformanceWarning(
                    requestName,
                    elapsedMilliseconds,
                    userId
                );
            }

            // ส่งคืนผลลัพธ์
            return response;
        }
    }
}

// ตัวอย่างการลงทะเบียนการใช้งาน:
// services.AddTransient(
//     typeof(IPipelineBehavior<,>), 
//     typeof(PerformanceBehavior<,>)
// );

// ตัวอย่างผลลัพธ์การบันทึกการทำงานที่ช้า:
// [Warning] Long running request: CreateRefundCommand (750 milliseconds) for user 12345