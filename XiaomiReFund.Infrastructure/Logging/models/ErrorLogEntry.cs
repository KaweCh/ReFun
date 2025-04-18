using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaomiReFund.Infrastructure.Logging.models
{
    /// <summary>
    /// โมเดลสำหรับบันทึกข้อผิดพลาด
    /// </summary>
    public class ErrorLogEntry
    {
        /// <summary>
        /// เวลาที่เกิดข้อผิดพลาด
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// ชื่อของ request ที่เกิดข้อผิดพลาด
        /// </summary>
        public string RequestName { get; set; }

        /// <summary>
        /// รหัสผู้ใช้ (ถ้ามี)
        /// </summary>
        public int? UserID { get; set; }

        /// <summary>
        /// ประเภทของข้อยกเว้น
        /// </summary>
        public string ExceptionType { get; set; }

        /// <summary>
        /// ข้อความข้อผิดพลาด
        /// </summary>
        public string ExceptionMessage { get; set; }

        /// <summary>
        /// รายละเอียด stack trace
        /// </summary>
        public string StackTrace { get; set; }

        /// <summary>
        /// ประเภทของ inner exception (ถ้ามี)
        /// </summary>
        public string InnerExceptionType { get; set; }

        /// <summary>
        /// ข้อความของ inner exception (ถ้ามี)
        /// </summary>
        public string InnerExceptionMessage { get; set; }

        /// <summary>
        /// ข้อมูลเพิ่มเติม (ถ้ามี)
        /// </summary>
        public object AdditionalData { get; set; }

        /// <summary>
        /// แปลงเป็นข้อความสำหรับแสดงผล
        /// </summary>
        /// <returns>ข้อความที่แสดงรายละเอียดของข้อผิดพลาด</returns>
        public override string ToString()
        {
            return $"[{Timestamp:yyyy-MM-dd HH:mm:ss.fff}] Error in {RequestName} - {ExceptionType}: {ExceptionMessage}";
        }
    }
}
