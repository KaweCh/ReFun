using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public int QueueID { get; private set; }

        /// <summary>
        /// รหัสการคืนเงิน
        /// </summary>
        public int RefundID { get; private set; }

        /// <summary>
        /// รหัสเทอร์มินัล
        /// </summary>
        public string TerminalID { get; private set; }

        /// <summary>
        /// วันที่ทำรายการ
        /// </summary>
        public DateTime TransactionDate { get; private set; }

        /// <summary>
        /// รหัสธุรกรรม
        /// </summary>
        public string TransactionID { get; private set; }

        /// <summary>
        /// รหัสคำขอ
        /// </summary>
        public string RequestID { get; private set; }

        /// <summary>
        /// จำนวนเงินที่คืน
        /// </summary>
        public decimal RefundAmount { get; private set; }

        /// <summary>
        /// สถานะ (Approved, Rejected)
        /// </summary>
        public string Status { get; private set; }

        /// <summary>
        /// ข้อความสถานะ
        /// </summary>
        public string StatusMessage { get; private set; }

        /// <summary>
        /// ประเภทการชำระเงิน
        /// </summary>
        public string PaymentType { get; private set; }

        /// <summary>
        /// จำนวนครั้งที่ลองส่งแล้ว
        /// </summary>
        public int RetryCount { get; private set; }

        /// <summary>
        /// เวลาที่กำหนดให้ส่ง
        /// </summary>
        public DateTime ScheduledTime { get; private set; }

        /// <summary>
        /// สถานะการส่ง
        /// </summary>
        public bool IsProcessed { get; private set; }

        /// <summary>
        /// เวลาที่ส่งสำเร็จ
        /// </summary>
        public DateTime? ProcessedTime { get; private set; }

        /// <summary>
        /// วันที่สร้าง
        /// </summary>
        public DateTime CreateDate { get; private set; }

        /// <summary>
        /// วันที่อัปเดต
        /// </summary>
        public DateTime? UpdateDate { get; private set; }

        /// <summary>
        /// รหัสผู้ใช้ที่แก้ไข
        /// </summary>
        public int ModifiedBy { get; private set; }

        // Navigation properties
        /// <summary>
        /// รายการคืนเงินที่เกี่ยวข้อง
        /// </summary>
        public virtual rms_OrderRefund Refund { get; private set; }

        /// <summary>
        /// ผู้ใช้ที่แก้ไข
        /// </summary>
        public virtual sys_Users ModifiedByUser { get; private set; }

        private rms_CallbackQueue()
        {
            // Required by EF Core
        }

        /// <summary>
        /// สร้างรายการคิวใหม่
        /// </summary>
        public static rms_CallbackQueue Create(
            int refundId,
            string terminalId,
            DateTime transactionDate,
            string transactionId,
            string requestId,
            decimal refundAmount,
            string status,
            string statusMessage,
            string paymentType,
            int retryCount,
            DateTime scheduledTime,
            int modifiedBy)
        {
            return new rms_CallbackQueue
            {
                RefundID = refundId,
                TerminalID = terminalId,
                TransactionDate = transactionDate,
                TransactionID = transactionId,
                RequestID = requestId,
                RefundAmount = refundAmount,
                Status = status,
                StatusMessage = statusMessage,
                PaymentType = paymentType,
                RetryCount = retryCount,
                ScheduledTime = scheduledTime,
                IsProcessed = false,
                ProcessedTime = null,
                CreateDate = DateTime.Now,
                ModifiedBy = modifiedBy
            };
        }

        /// <summary>
        /// อัปเดตสถานะการประมวลผล
        /// </summary>
        public void UpdateProcessingStatus(bool isProcessed, int modifiedBy)
        {
            IsProcessed = isProcessed;
            if (isProcessed)
            {
                ProcessedTime = DateTime.Now;
            }
            ModifiedBy = modifiedBy;
            UpdateDate = DateTime.Now;
        }

        /// <summary>
        /// เพิ่มจำนวนครั้งที่ลองส่ง
        /// </summary>
        public void IncrementRetryCount(DateTime nextScheduledTime, int modifiedBy)
        {
            RetryCount++;
            ScheduledTime = nextScheduledTime;
            ModifiedBy = modifiedBy;
            UpdateDate = DateTime.Now;
        }
    }
}
