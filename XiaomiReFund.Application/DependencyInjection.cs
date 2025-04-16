using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using XiaomiReFund.Application.Common.Behaviors;
using XiaomiReFund.Application.Common.Interfaces;
using XiaomiReFund.Application.Interfaces.External;
using XiaomiReFund.Application.Interfaces.Services;
using XiaomiReFund.Application.Services;

namespace XiaomiReFund.Application
{
    /// <summary>
    /// คลาสที่ทำหน้าที่ลงทะเบียนบริการสำหรับชั้น Application
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// เพิ่มบริการของชั้น Application เข้าสู่ DI Container
        /// </summary>
        /// <param name="services">IServiceCollection สำหรับลงทะเบียนบริการ</param>
        /// <returns>IServiceCollection ที่ได้ลงทะเบียนบริการแล้ว</returns>
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // ลงทะเบียน MediatR
            services.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });

            // ลงทะเบียน AutoMapper
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            // ลงทะเบียน FluentValidation - การลงทะเบียนตัวตรวจสอบด้วยตนเอง
            // หมายเหตุ: ถ้ามี FluentValidation.DependencyInjection คุณสามารถใช้:
            // services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            // กำหนดประเภทของตัวตรวจสอบโดยใช้อินเทอร์เฟซทั่วไป IValidator<>
            var validatorType = typeof(IValidator<>);

            // ค้นหาและเลือกประเภทต่าง ๆ จากแอสเซมบลีปัจจุบัน
            // - กรองเฉพาะคลาสที่ไม่เป็นแบบนามธรรม (abstract) และไม่ใช่ประเภทเทมเพลตทั่วไป
            // - เลือกประเภทที่มีอินเทอร์เฟซที่เป็นเทมเพลตตรงกับ IValidator<>
            var validatorTypes = Assembly.GetExecutingAssembly()
                .GetExportedTypes() // ดึงประเภทที่ส่งออกทั้งหมด
                .Where(t => !t.IsAbstract && !t.IsGenericTypeDefinition) // กรองประเภทที่ไม่เป็นแบบนามธรรมและไม่ใช่เทมเพลต
                .Select(t => (Type: t, Interfaces: t.GetInterfaces())) // เลือกประเภทและอินเทอร์เฟซของแต่ละประเภท
                .Where(x => x.Interfaces.Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == validatorType)) // กรองเฉพาะประเภทที่มีอินเทอร์เฟซ IValidator<>
                .ToList();

            // วนลูปผ่านประเภทตัวตรวจสอบที่พบ
            foreach (var (type, interfaces) in validatorTypes)
            {
                // ค้นหาอินเทอร์เฟซ IValidator<> ที่ถูกใช้งานโดยประเภทปัจจุบัน
                var implementedValidatorType = interfaces.First(i => i.IsGenericType && i.GetGenericTypeDefinition() == validatorType);

                // กำหนดอินเทอร์เฟซตัวตรวจสอบ
                var validatorInterface = implementedValidatorType;

                // ลงทะเบียนการบริการสำหรับตัวตรวจสอบแต่ละประเภทด้วยการใช้ Transient Lifetime
                services.AddTransient(validatorInterface, type);
            }

            // ลงทะเบียน Behaviors
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehavior<,>));

            // ลงทะเบียนบริการ
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<ICallbackService, CallbackService>();
            services.AddTransient<IDateTime, DateTimeService>();
            services.AddTransient<IHttpClientService, HttpClientService>();

            return services;
        }
    }
}
