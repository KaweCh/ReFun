using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XiaomiReFund.Domain.Constants;
using XiaomiReFund.Infrastructure.Logging.Helpers;
using XiaomiReFund.Infrastructure.Settings;

namespace XiaomiReFund.Infrastructure.Logging
{
    public class FileLogger
    {
        private readonly LogSettings _logSettings;
        private readonly LogFileRotator _logFileRotator;
        private readonly object _lockObject = new object();

        /// <summary>
        /// สร้าง FileLogger ใหม่
        /// </summary>
        /// <param name="logSettings">การตั้งค่าการบันทึกข้อมูล</param>
        public FileLogger(IOptions<LogSettings> logSettings)
        {
            _logSettings = logSettings.Value;
            _logFileRotator = new LogFileRotator(_logSettings);

            // ตรวจสอบและสร้างไดเรกทอรีถ้ายังไม่มี
            EnsureLogDirectoryExists();
        }

        /// <summary>
        /// บันทึกข้อความลงไฟล์
        /// </summary>
        /// <param name="logType">ประเภทของ log</param>
        /// <param name="message">ข้อความที่ต้องการบันทึก</param>
        /// <param name="level">ระดับความสำคัญ</param>
        /// <returns>Task</returns>
        public async Task LogAsync(string logType, string message, string level = LogConstants.LogLevel.Info)
        {
            if (string.IsNullOrEmpty(message))
                return;

            try
            {
                // สร้างข้อความที่จะบันทึก
                var timestamp = DateTime.Now.ToString(LogConstants.LogFormat.DateTimeFormat);
                var logEntry = $"[{timestamp}] [{level}] {message}{Environment.NewLine}";

                // ใช้ LogFileRotator เพื่อกำหนดชื่อไฟล์
                string logFilePath = _logFileRotator.GetLogFilePath(logType);

                // บันทึกข้อมูลลงไฟล์แบบ thread-safe
                await WriteToFileAsync(logFilePath, logEntry);
            }
            catch (Exception ex)
            {
                // ถ้าเกิดข้อผิดพลาดในการบันทึก log ให้พยายามบันทึกไปยัง error log
                if (logType != LogConstants.LogType.Error)
                {
                    await LogErrorAsync($"Failed to write to {logType} log: {ex.Message}", ex);
                }
            }
        }

        /// <summary>
        /// บันทึกข้อผิดพลาดพร้อมรายละเอียด
        /// </summary>
        /// <param name="message">ข้อความข้อผิดพลาด</param>
        /// <param name="ex">ข้อยกเว้นที่เกิดขึ้น</param>
        /// <returns>Task</returns>
        public async Task LogErrorAsync(string message, Exception ex)
        {
            if (ex == null)
            {
                await LogAsync(LogConstants.LogType.Error, message, LogConstants.LogLevel.Error);
                return;
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(message);
            sb.AppendLine($"Exception: {ex.GetType().Name}");
            sb.AppendLine($"Message: {ex.Message}");
            sb.AppendLine($"StackTrace: {ex.StackTrace}");

            // บันทึก inner exception ถ้ามี
            var innerEx = ex.InnerException;
            if (innerEx != null)
            {
                sb.AppendLine("Inner Exception:");
                sb.AppendLine($"  Type: {innerEx.GetType().Name}");
                sb.AppendLine($"  Message: {innerEx.Message}");
                sb.AppendLine($"  StackTrace: {innerEx.StackTrace}");
            }

            await LogAsync(LogConstants.LogType.Error, sb.ToString(), LogConstants.LogLevel.Error);
        }

        /// <summary>
        /// เขียนข้อมูลลงไฟล์แบบ thread-safe
        /// </summary>
        /// <param name="filePath">พาธของไฟล์ที่ต้องการเขียน</param>
        /// <param name="content">เนื้อหาที่ต้องการเขียน</param>
        /// <returns>Task</returns>
        private async Task WriteToFileAsync(string filePath, string content)
        {
            // ใช้ lock เพื่อป้องกันการเขียนพร้อมกันจากหลาย thread
            lock (_lockObject)
            {
                // ตรวจสอบขนาดไฟล์และทำการหมุนเวียนถ้าจำเป็น
                _logFileRotator.RotateLogFileIfNeeded(filePath);

                // เขียนข้อมูลลงไฟล์
                using (StreamWriter writer = new StreamWriter(filePath, true, Encoding.UTF8))
                {
                    writer.Write(content);
                    writer.Flush();
                }
            }

            await Task.CompletedTask;
        }

        /// <summary>
        /// ตรวจสอบและสร้างไดเรกทอรีสำหรับเก็บไฟล์ log
        /// </summary>
        private void EnsureLogDirectoryExists()
        {
            if (!Directory.Exists(_logSettings.LogDirectory))
            {
                Directory.CreateDirectory(_logSettings.LogDirectory);
            }
        }
    }
}
