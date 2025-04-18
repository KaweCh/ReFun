using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XiaomiReFund.Application.Interfaces.Services;
using XiaomiReFund.Infrastructure.Settings;

namespace XiaomiReFund.Infrastructure.BackgroundTasks
{
    /// <summary>
    /// บริการพื้นหลังสำหรับประมวลผล callback ที่อยู่ในคิว
    /// </summary>
    public class CallbackProcessor : BackgroundService
    {
        private readonly ILogger<CallbackProcessor> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly CallbackSettings _callbackSettings;
        private readonly TimeSpan _processingInterval;

        /// <summary>
        /// สร้าง CallbackProcessor ใหม่
        /// </summary>
        /// <param name="logger">บริการบันทึกข้อมูล</param>
        /// <param name="serviceProvider">ตัวให้บริการ dependency injection</param>
        /// <param name="callbackSettings">การตั้งค่า callback</param>
        public CallbackProcessor(
            ILogger<CallbackProcessor> logger,
            IServiceProvider serviceProvider,
            IOptions<CallbackSettings> callbackSettings)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _callbackSettings = callbackSettings.Value;

            // กำหนดช่วงเวลาการประมวลผลเป็นนาที (ค่าเริ่มต้น 1 นาที)
            _processingInterval = TimeSpan.FromMinutes(_callbackSettings.RetryDelayMinutes / 5);
            if (_processingInterval < TimeSpan.FromSeconds(30))
            {
                _processingInterval = TimeSpan.FromSeconds(30);
            }
        }

        /// <summary>
        /// ดำเนินการพื้นหลัง
        /// </summary>
        /// <param name="stoppingToken">โทเค็นการยกเลิก</param>
        /// <returns>Task</returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Callback Processor service is starting.");

            // วนลูปทำงานตราบใดที่บริการยังทำงานอยู่
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await ProcessPendingCallbacksAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while processing callbacks.");
                }

                // รอจนกว่าจะถึงรอบถัดไป
                await Task.Delay(_processingInterval, stoppingToken);
            }

            _logger.LogInformation("Callback Processor service is stopping.");
        }

        /// <summary>
        /// ประมวลผล callback ที่อยู่ในคิว
        /// </summary>
        /// <param name="cancellationToken">โทเค็นการยกเลิก</param>
        /// <returns>Task</returns>
        private async Task ProcessPendingCallbacksAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug("Processing pending callbacks...");

            // สร้าง scope เพื่อใช้งาน service
            using (var scope = _serviceProvider.CreateScope())
            {
                // ดึงบริการ callback
                var callbackService = scope.ServiceProvider.GetRequiredService<ICallbackService>();

                try
                {
                    // ประมวลผล callback ในคิว
                    int processedCount = await callbackService.ProcessCallbackQueueAsync();

                    if (processedCount > 0)
                    {
                        _logger.LogInformation("Successfully processed {Count} callbacks.", processedCount);
                    }
                    else
                    {
                        _logger.LogDebug("No pending callbacks to process.");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing callbacks from queue.");
                }
            }
        }
    }
}
