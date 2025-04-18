using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaomiReFund.Infrastructure.Logging.models
{
    /// <summary>
    /// โมเดลสำหรับบันทึกการทำงานของ callback
    /// </summary>
    public class CallbackLogEntry
    {
        /// <summary>
        /// เวลาที่เกิดเหตุการณ์
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// URL ปลายทางของ callback
        /// </summary>
        public string Endpoint { get; set; }

        /// <summary>
        /// ข้อมูล request ที่ส่งไป
        /// </summary>
        public object Request { get; set; }

        /// <summary>
        /// ข้อมูล response ที่ได้รับกลับมา
        /// </summary>
        public object Response { get; set; }

        /// <summary>
        /// สถานะความสำเร็จของการส่ง callback
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// จำนวนครั้งที่ลองส่ง
        /// </summary>
        public int RetryCount { get; set; }

        /// <summary>
        /// เวลาที่ใช้ในการส่ง (มิลลิวินาที)
        /// </summary>
        public long? ElapsedMilliseconds { get; set; }

        /// <summary>
        /// รหัสสถานะ HTTP (ถ้ามี)
        /// </summary>
        public int? StatusCode { get; set; }

        /// <summary>
        /// ข้อความข้อผิดพลาด (ถ้ามี)
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// แปลงเป็นข้อความสำหรับแสดงผล
        /// </summary>
        /// <returns>ข้อความที่แสดงรายละเอียดของ callback</returns>
        public override string ToString()
        {
            string status = IsSuccess ? "Success" : "Failed";
            return $"[{Timestamp:yyyy-MM-dd HH:mm:ss.fff}] Callback to {Endpoint} - Status: {status} - Retry: {RetryCount}";
        }
    }
}
