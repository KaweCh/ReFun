using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using XiaomiReFund.Application.Common.Interfaces;
using XiaomiReFund.Domain.Constants;
using XiaomiReFund.Infrastructure.Logging.models;
using XiaomiReFund.Infrastructure.Settings;

namespace XiaomiReFund.Infrastructure.Logging
{
    /// <summary>
    /// บริการบันทึกข้อมูลระบบ
    /// </summary>
    public class LoggerService : ILoggerService
    {
        private readonly FileLogger _fileLogger;
        private readonly LogSettings _logSettings;
        private readonly JsonSerializerOptions _jsonOptions;

        /// <summary>
        /// สร้าง LoggerService ใหม่
        /// </summary>
        /// <param name="fileLogger">ตัวบันทึกข้อมูลลงไฟล์</param>
        /// <param name="logSettings">การตั้งค่าการบันทึกข้อมูล</param>
        public LoggerService(FileLogger fileLogger, IOptions<LogSettings> logSettings)
        {
            _fileLogger = fileLogger;
            _logSettings = logSettings.Value;

            // ตั้งค่า JSON Serializer
            _jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        /// <inheritdoc/>
        public void LogRequest(string requestName, object request, int userId)
        {
            if (!_logSettings.EnableRequestLogging)
                return;

            try
            {
                // สร้าง log entry
                var logEntry = new ApiLogEntry
                {
                    Timestamp = DateTime.Now,
                    RequestName = requestName,
                    UserID = userId,
                    Data = request,
                    Direction = "Request"
                };

                // แปลงเป็น JSON
                string json = SerializeToJson(logEntry);

                // บันทึกลงไฟล์
                _ = _fileLogger.LogAsync(LogConstants.LogType.Request, json);
            }
            catch (Exception ex)
            {
                // ถ้าเกิดข้อผิดพลาดในการบันทึก request log ให้บันทึกไปยัง error log
                _ = _fileLogger.LogErrorAsync($"Error logging request {requestName}", ex);
            }
        }

        /// <inheritdoc/>
        public void LogResponse(string requestName, object response, long elapsedMilliseconds)
        {
            if (!_logSettings.EnableResponseLogging)
                return;

            try
            {
                // สร้าง log entry
                var logEntry = new ApiLogEntry
                {
                    Timestamp = DateTime.Now,
                    RequestName = requestName,
                    ElapsedMilliseconds = elapsedMilliseconds,
                    Data = response,
                    Direction = "Response"
                };

                // แปลงเป็น JSON
                string json = SerializeToJson(logEntry);

                // บันทึกลงไฟล์
                _ = _fileLogger.LogAsync(LogConstants.LogType.Response, json);
            }
            catch (Exception ex)
            {
                // ถ้าเกิดข้อผิดพลาดในการบันทึก response log ให้บันทึกไปยัง error log
                _ = _fileLogger.LogErrorAsync($"Error logging response for {requestName}", ex);
            }
        }

        /// <inheritdoc/>
        public void LogError(string requestName, Exception exception, int userId)
        {
            if (!_logSettings.EnableErrorLogging)
                return;

            try
            {
                // สร้าง log entry
                var logEntry = new ErrorLogEntry
                {
                    Timestamp = DateTime.Now,
                    RequestName = requestName,
                    UserID = userId,
                    ExceptionType = exception.GetType().Name,
                    ExceptionMessage = exception.Message,
                    StackTrace = exception.StackTrace
                };

                // เพิ่ม inner exception ถ้ามี
                if (exception.InnerException != null)
                {
                    logEntry.InnerExceptionType = exception.InnerException.GetType().Name;
                    logEntry.InnerExceptionMessage = exception.InnerException.Message;
                }

                // แปลงเป็น JSON
                string json = SerializeToJson(logEntry);

                // บันทึกลงไฟล์
                _ = _fileLogger.LogErrorAsync(json, exception);
            }
            catch (Exception ex)
            {
                // พยายามบันทึกข้อผิดพลาดแบบพื้นฐาน
                _ = _fileLogger.LogErrorAsync($"Error in error logging for {requestName}: {ex.Message}", ex);
            }
        }

        /// <inheritdoc/>
        public void LogPerformanceWarning(string requestName, long elapsedMilliseconds, int userId)
        {
            if (!_logSettings.EnablePerformanceLogging)
                return;

            try
            {
                var logEntry = new
                {
                    Timestamp = DateTime.Now,
                    RequestName = requestName,
                    UserID = userId,
                    ElapsedMilliseconds = elapsedMilliseconds,
                    Message = $"Long running request: {requestName} took {elapsedMilliseconds}ms for user {userId}"
                };

                string json = SerializeToJson(logEntry);

                _ = _fileLogger.LogAsync(
                    LogConstants.LogType.System,
                    json,
                    LogConstants.LogLevel.Warning);
            }
            catch (Exception ex)
            {
                _ = _fileLogger.LogErrorAsync($"Error logging performance warning for {requestName}", ex);
            }
        }

        /// <inheritdoc/>
        public void LogCallback(string endpoint, object request, object response, bool isSuccess, int retryCount)
        {
            if (!_logSettings.EnableCallbackLogging)
                return;

            try
            {
                // สร้าง log entry
                var logEntry = new CallbackLogEntry
                {
                    Timestamp = DateTime.Now,
                    Endpoint = endpoint,
                    Request = request,
                    Response = response,
                    IsSuccess = isSuccess,
                    RetryCount = retryCount
                };

                // แปลงเป็น JSON
                string json = SerializeToJson(logEntry);

                // บันทึกลงไฟล์
                _ = _fileLogger.LogAsync(LogConstants.LogType.Callback, json);

                // ถ้าไม่สำเร็จ ให้บันทึกเพิ่มเติมเป็น warning
                if (!isSuccess)
                {
                    _ = _fileLogger.LogAsync(
                        LogConstants.LogType.Callback,
                        $"Callback to {endpoint} failed. Retry count: {retryCount}",
                        LogConstants.LogLevel.Warning);
                }
            }
            catch (Exception ex)
            {
                _ = _fileLogger.LogErrorAsync($"Error logging callback to {endpoint}", ex);
            }
        }

        /// <inheritdoc/>
        public void LogAuthenticationSuccess(string username, int userId)
        {
            if (!_logSettings.EnableSecurityLogging)
                return;

            try
            {
                var logEntry = new
                {
                    Timestamp = DateTime.Now,
                    Event = "Authentication Success",
                    Username = username,
                    UserID = userId
                };

                string json = SerializeToJson(logEntry);

                _ = _fileLogger.LogAsync(LogConstants.LogType.System, json);
            }
            catch (Exception ex)
            {
                _ = _fileLogger.LogErrorAsync($"Error logging authentication success for {username}", ex);
            }
        }

        /// <inheritdoc/>
        public void LogAuthenticationFailure(string username, string reason)
        {
            if (!_logSettings.EnableSecurityLogging)
                return;

            try
            {
                var logEntry = new
                {
                    Timestamp = DateTime.Now,
                    Event = "Authentication Failure",
                    Username = username,
                    Reason = reason
                };

                string json = SerializeToJson(logEntry);

                _ = _fileLogger.LogAsync(
                    LogConstants.LogType.System,
                    json,
                    LogConstants.LogLevel.Warning);
            }
            catch (Exception ex)
            {
                _ = _fileLogger.LogErrorAsync($"Error logging authentication failure for {username}", ex);
            }
        }

        /// <summary>
        /// แปลงออบเจกต์เป็น JSON
        /// </summary>
        /// <param name="obj">ออบเจกต์ที่ต้องการแปลง</param>
        /// <returns>JSON string</returns>
        private string SerializeToJson(object obj)
        {
            try
            {
                return JsonSerializer.Serialize(obj, _jsonOptions);
            }
            catch
            {
                // ถ้า serialize ไม่ได้ ให้ใช้วิธีพื้นฐาน
                return $"{{\"error\":\"Cannot serialize object of type {obj?.GetType().Name}\",\"toString\":\"{obj}\"}}";
            }
        }
    }
}
