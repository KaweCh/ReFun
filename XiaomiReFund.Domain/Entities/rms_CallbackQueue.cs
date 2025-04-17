using System;

namespace XiaomiReFund.Domain.Entities
{
    /// <summary>
    /// เอนทิตี้สำหรับเก็บข้อมูลคิวการ callback
    /// </summary>
    public class rms_CallbackQueue
    {
        /// <summary>
        /// รหัสรายการในคิว
        /// </summary>
        public int CallbackID { get; set; }

        /// <summary>
        /// รหัสการคืนเงิน
        /// </summary>
        public int RefundID { get; set; }

        /// <summary>
        /// สถานะ (Approved, Rejected)
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// ข้อความสถานะ
        /// </summary>
        public string StatusMessage { get; set; }

        /// <summary>
        /// จำนวนครั้งที่ลองส่งแล้ว
        /// </summary>
        public int RetryCount { get; set; }

        /// <summary>
        /// เวลาที่กำหนดให้ส่ง
        /// </summary>
        public DateTime ScheduledTime { get; set; }

        /// <summary>
        /// สถานะการส่ง
        /// </summary>
        public bool IsProcessed { get; set; }

        /// <summary>
        /// เวลาที่ส่งสำเร็จ
        /// </summary>
        public DateTime? ProcessedTime { get; set; }

        /// <summary>
        /// วันที่สร้าง
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// วันที่อัปเดต
        /// </summary>
        public DateTime? UpdateDate { get; set; }

        // Navigation property
        public virtual rms_OrderRefund Refund { get; set; }

        /// <summary>
        /// สร้างรายการคิวใหม่
        /// </summary>
        public static rms_CallbackQueue Create(
            int refundId,
            string status,
            string statusMessage,
            int retryCount,
            DateTime scheduledTime)
        {
            return new rms_CallbackQueue
            {
                RefundID = refundId,
                Status = status,
                StatusMessage = statusMessage,
                RetryCount = retryCount,
                ScheduledTime = scheduledTime,
                IsProcessed = false,
                ProcessedTime = null,
                CreateDate = DateTime.Now
            };
        }

        /// <summary>
        /// อัปเดตสถานะการประมวลผล
        /// </summary>
        public void UpdateProcessingStatus(bool isProcessed)
        {
            IsProcessed = isProcessed;
            if (isProcessed)
            {
                ProcessedTime = DateTime.Now;
            }
            UpdateDate = DateTime.Now;
        }

        /// <summary>
        /// เพิ่มจำนวนครั้งที่ลองส่ง
        /// </summary>
        public void IncrementRetryCount(DateTime nextScheduledTime)
        {
            RetryCount++;
            ScheduledTime = nextScheduledTime;
            UpdateDate = DateTime.Now;
        }
    }
}