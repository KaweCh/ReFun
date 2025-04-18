using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaomiReFund.Infrastructure.Settings
{
    public class LogSettings
    {
        /// <summary>
        /// ไดเรกทอรีสำหรับเก็บไฟล์ log
        /// </summary>
        public string LogDirectory { get; set; } = "Logs";

        /// <summary>
        /// ขนาดไฟล์ log สูงสุดก่อนทำการหมุนเวียน (bytes)
        /// </summary>
        public long MaxLogFileSizeBytes { get; set; } = 10 * 1024 * 1024; // 10 MB

        /// <summary>
        /// จำนวนไฟล์ log สูงสุดที่เก็บไว้ต่อประเภท
        /// </summary>
        public int MaxLogFileCount { get; set; } = 30;

        /// <summary>
        /// เปิดใช้งานการบันทึก request
        /// </summary>
        public bool EnableRequestLogging { get; set; } = true;

        /// <summary>
        /// เปิดใช้งานการบันทึก response
        /// </summary>
        public bool EnableResponseLogging { get; set; } = true;

        /// <summary>
        /// เปิดใช้งานการบันทึกข้อผิดพลาด
        /// </summary>
        public bool EnableErrorLogging { get; set; } = true;

        /// <summary>
        /// เปิดใช้งานการบันทึกด้านประสิทธิภาพ
        /// </summary>
        public bool EnablePerformanceLogging { get; set; } = true;

        /// <summary>
        /// เปิดใช้งานการบันทึก callback
        /// </summary>
        public bool EnableCallbackLogging { get; set; } = true;

        /// <summary>
        /// เปิดใช้งานการบันทึกด้านความปลอดภัย
        /// </summary>
        public bool EnableSecurityLogging { get; set; } = true;

        /// <summary>
        /// ระดับความละเอียดของการบันทึก
        /// </summary>
        public string LogLevel { get; set; } = "Information";

        /// <summary>
        /// พาธเต็มของไฟล์ log สำหรับ callback
        /// </summary>
        public string CallbackLogPath => System.IO.Path.Combine(LogDirectory, "callback.log");

        /// <summary>
        /// พาธเต็มของไฟล์ log สำหรับข้อผิดพลาด
        /// </summary>
        public string ErrorLogPath => System.IO.Path.Combine(LogDirectory, "error.log");

        /// <summary>
        /// เวลาเก็บไฟล์ log (วัน)
        /// </summary>
        public int LogRetentionDays { get; set; } = 90;

        /// <summary>
        /// ตัดทอนข้อมูลที่ละเอียดอ่อนในการบันทึก
        /// </summary>
        public bool SanitizeSensitiveData { get; set; } = true;
    }
}
