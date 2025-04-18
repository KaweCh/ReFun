using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Diagnostics; // เพิ่ม namespace Stopwatch
using XiaomiReFund.Application.Common.Interfaces;

namespace XiaomiReFund.Application.Common.Behaviors
{
    // คลาส LoggingBehavior เป็นส่วนของ MediatR Pipeline 
    // ทำหน้าที่บันทึกการทำงานของคำร้องขอทั้งก่อนและหลังการประมวลผล
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        // ประกาศตัวแปรสำหรับบันทึกแบบต่างๆ
        // _logger: บันทึกมาตรฐานของ .NET
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

        // _currentUserService: บริการดึงข้อมูลผู้ใช้ปัจจุบัน
        private readonly ICurrentUserService _currentUserService;

        // _loggerService: บริการบันทึกแบบกำหนดเอง
        private readonly ILoggerService _loggerService;

        // Constructor รับบริการต่างๆ ผ่าน Dependency Injection
        public LoggingBehavior(
            ILogger<LoggingBehavior<TRequest, TResponse>> logger,
            ICurrentUserService currentUserService,
            ILoggerService loggerService)
        {
            // กำหนดค่าให้กับตัวแปรจาก dependencies ที่ส่งเข้ามา
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
            // ดึงชื่อประเภทของคำร้องขอ
            var requestName = typeof(TRequest).Name;

            // ดึง UserID จาก CurrentUserService (ถ้าไม่มีให้ใช้ 0)
            var userId = _currentUserService.UserId ?? 0;

            // บันทึกการเริ่มต้นประมวลผลคำร้องขอ
            _logger.LogInformation("Handling {RequestName} for user {UserId}", requestName, userId);

            try
            {
                // ล้างข้อมูลที่ละเอียดอ่อนก่อนบันทึก
                var sanitizedRequest = SanitizeSensitiveData(request);

                // บันทึกรายละเอียดคำร้องขอ
                _loggerService.LogRequest(requestName, sanitizedRequest, userId);

                // เริ่มจับเวลาการประมวลผล
                var stopwatch = Stopwatch.StartNew();

                // ประมวลผลคำร้องขอ
                var response = await next();

                // หยุดจับเวลา
                stopwatch.Stop();

                // บันทึกข้อมูลการประมวลผลสำเร็จ
                _logger.LogInformation(
                    "Handled {RequestName} in {ElapsedMilliseconds}ms",
                    requestName,
                    stopwatch.ElapsedMilliseconds
                );

                // บันทึกการตอบกลับ
                _loggerService.LogResponse(requestName, response, stopwatch.ElapsedMilliseconds);

                // ส่งคืนผลลัพธ์
                return response;
            }
            catch (Exception ex)
            {
                // บันทึกข้อผิดพลาดหากการประมวลผลล้มเหลว
                _logger.LogError(
                    ex,
                    "Error handling {RequestName} for user {UserId}",
                    requestName,
                    userId
                );

                // บันทึกข้อผิดพลาดผ่าน Logger Service
                _loggerService.LogError(requestName, ex, userId);

                // ส่งต่อข้อผิดพลาด
                throw;
            }
        }

        // เมธอดสำหรับล้างข้อมูลที่ละเอียดอ่อน
        private object SanitizeSensitiveData(TRequest request)
        {
            // แปลงออบเจ็กต์เป็น JSON เพื่อสร้างสำเนา
            var json = JsonSerializer.Serialize(request);

            // แปลง JSON กลับเป็นออบเจ็กต์
            var sanitizedRequest = JsonSerializer.Deserialize<object>(json);

            // TODO: เพิ่มตรรกะการล้างข้อมูลที่ละเอียดอ่อน
            // เช่น ลบฟิลด์รหัสผ่าน, หมายเลขบัตรเครดิต ฯลฯ

            return sanitizedRequest;
        }
    }
}

// ตัวอย่างการใช้งาน
// public class CreateRefundCommand : IRequest<RefundResponse>
// {
//     public string UserId { get; set; }
//     public decimal Amount { get; set; }
//     // การใช้ LoggingBehavior จะบันทึกการร้องขอนี้โดยอัตโนมัติ
// }