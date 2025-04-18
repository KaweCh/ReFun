using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaomiReFund.Application.Common.Models
{
    /// <summary>
    /// การตั้งค่าสำหรับการส่ง callback
    /// </summary>
    public class CallbackSettings
    {
        /// <summary>
        /// จำนวนครั้งสูงสุดที่จะลองส่งใหม่
        /// </summary>
        public int MaxRetryCount { get; set; } = 5;

        /// <summary>
        /// ระยะเวลาที่รอก่อนลองส่งใหม่ (นาที)
        /// </summary>
        public int RetryDelayMinutes { get; set; } = 5;

        /// <summary>
        /// เวลาที่จะสิ้นสุดการลองส่งใหม่ (ชั่วโมง)
        /// </summary>
        public int GiveUpAfterHours { get; set; } = 24;

        /// <summary>
        /// จำนวนรายการสูงสุดที่จะประมวลผลในแต่ละรอบ
        /// </summary>
        public int BatchSize { get; set; } = 10;

        /// <summary>
        /// ระยะเวลา timeout สำหรับการส่ง callback (วินาที)
        /// </summary>
        public int TimeoutSeconds { get; set; } = 30;

        /// <summary>
        /// เปิดใช้งานการเพิ่มเวลารอแบบแบ่งช่วง (exponential backoff)
        /// </summary>
        public bool UseExponentialBackoff { get; set; } = true;

        /// <summary>
        /// ตัวคูณสำหรับการเพิ่มเวลารอแบบแบ่งช่วง
        /// </summary>
        public int BackoffMultiplier { get; set; } = 2;

        /// <summary>
        /// เวลารอสูงสุดระหว่างการลองใหม่ (นาที)
        /// </summary>
        public int MaxBackoffMinutes { get; set; } = 60;

        /// <summary>
        /// คำนวณเวลารอสำหรับการลองส่งใหม่ครั้งถัดไป
        /// </summary>
        /// <param name="retryCount">จำนวนครั้งที่ลองแล้ว</param>
        /// <returns>เวลารอเป็นนาที</returns>
        public int CalculateRetryDelay(int retryCount)
        {
            if (!UseExponentialBackoff)
                return RetryDelayMinutes;

            // เพิ่มเวลารอแบบแบ่งช่วง
            int delay = RetryDelayMinutes * (int)Math.Pow(BackoffMultiplier, retryCount);

            // จำกัดไม่ให้เกินค่าสูงสุด
            return Math.Min(delay, MaxBackoffMinutes);
        }
    }
}
