using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using XiaomiReFund.Application.Common.Interfaces;
using XiaomiReFund.Application.Interfaces.External;
using XiaomiReFund.Application.Interfaces.Security;
using XiaomiReFund.Application.Interfaces.Services;
using XiaomiReFund.Application.Services;
using XiaomiReFund.Domain.Interfaces.Repositories;
using XiaomiReFund.Infrastructure.BackgroundTasks;
using XiaomiReFund.Infrastructure.Data.DbContext;
using XiaomiReFund.Infrastructure.Data.Repositories;
using XiaomiReFund.Infrastructure.Http;
using XiaomiReFund.Infrastructure.Logging;
using XiaomiReFund.Infrastructure.Security;
using XiaomiReFund.Infrastructure.Settings;
using HttpClientService = XiaomiReFund.Infrastructure.Http.HttpClientService;
using TokenService = XiaomiReFund.Infrastructure.Security.TokenService;

namespace XiaomiReFund.Infrastructure
{
    /// <summary>
    /// คลาสกำหนดการฉีด dependency
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// กำหนดบริการสำหรับชั้น Infrastructure
        /// </summary>
        /// <param name="services">IServiceCollection</param>
        /// <param name="configuration">IConfiguration</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // ลงทะเบียน DbContext
            services.AddDbContext<RefundDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(RefundDbContext).Assembly.FullName)));

            // ลงทะเบียน Repositories
            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<IRefundRepository, RefundRepository>();
            services.AddScoped<IPaymentTypeRepository, PaymentTypeRepository>();

            // ลงทะเบียนบริการ Infrastructure
            RegisterInfrastructureServices(services, configuration);

            return services;
        }

        /// <summary>
        /// ลงทะเบียนบริการ Infrastructure
        /// </summary>
        /// <param name="services">IServiceCollection</param>
        /// <param name="configuration">IConfiguration</param>
        private static void RegisterInfrastructureServices(IServiceCollection services, IConfiguration configuration)
        {
            // ลงทะเบียนการตั้งค่า
            services.Configure<LogSettings>(options => configuration.GetSection("LogSettings").Bind(options));
            services.Configure<JwtSettings>(options => configuration.GetSection("JwtSettings").Bind(options));
            services.Configure<CallbackSettings>(options => configuration.GetSection("CallbackSettings").Bind(options));

            // ลงทะเบียนบริการบันทึกข้อมูล
            services.AddSingleton<FileLogger>();
            services.AddSingleton<ILoggerService, LoggerService>();

            // ลงทะเบียนบริการความปลอดภัย
            services.AddTransient<IPasswordHasher, PasswordHasher>();
            services.AddTransient<ITokenService, TokenService>();

            // ลงทะเบียนบริการ HTTP
            services.AddTransient<IHttpClientService, HttpClientService>();

            // ลงทะเบียนบริการอื่นๆ
            services.AddTransient<IDateTime, DateTimeService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();

            // ลงทะเบียนบริการพื้นหลัง
            services.AddHostedService<CallbackProcessor>();
        }
    }
}
