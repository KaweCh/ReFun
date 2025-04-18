using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaomiReFund.Infrastructure.Logging.models
{
    /// <summary>
    /// โมเดลสำหรับบันทึก API request และ response
    /// </summary>
    public class ApiLogEntry
    {
        /// <summary>
        /// เวลาที่เกิดเหตุการณ์
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// ชื่อของ request
        /// </summary>
        public string RequestName { get; set; }

        /// <summary>
        /// รหัสผู้ใช้ (ถ้ามี)
        /// </summary>
        public int? UserID { get; set; }

        /// <summary>
        /// ทิศทางของข้อมูล (Request หรือ Response)
        /// </summary>
        public string Direction { get; set; }

        /// <summary>
        /// เวลาที่ใช้ในการประมวลผล (มิลลิวินาที)
        /// </summary>
        public long? ElapsedMilliseconds { get; set; }

        /// <summary>
        /// ข้อมูล request หรือ response
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// แปลงเป็นข้อความสำหรับแสดงผล
        /// </summary>
        /// <returns>ข้อความที่แสดงรายละเอียดของ log entry</returns>
        public override string ToString()
        {
            return $"[{Timestamp:yyyy-MM-dd HH:mm:ss.fff}] {Direction} - {RequestName} - UserID: {UserID} - Elapsed: {ElapsedMilliseconds}ms";
        }
    }
}
