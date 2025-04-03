using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaomiReFund.Application.DTOs.Callback
{
    // คลาสนี้เป็น Data Transfer Object (DTO) สำหรับคำร้องขอเข้าคิวการส่งกลับ
    public class EnqueueCallbackRequest
    {
        // คุณสมบัติ RefundID - รหัสอ้างอิงการคืนเงิน
        [Required] // บังคับให้ต้องมีค่า
        public int RefundID { get; set; }

        // คุณสมบัติ TerminalID - รหัสเทอร์มินัล
        [Required] // บังคับให้ต้องมีค่า
        [StringLength(8)] // จำกัดความยาวไม่เกิน 8 ตัวอักษร
        public string TerminalID { get; set; }

        // คุณสมบัติ TransactionDate - วันที่และเวลาของธุรกรรม
        [Required] // บังคับให้ต้องมีค่า
        public DateTime TransactionDate { get; set; }

        // คุณสมบัติ TransactionID - รหัสธุรกรรม
        [Required] // บังคับให้ต้องมีค่า
        [StringLength(50)] // จำกัดความยาวไม่เกิน 50 ตัวอักษร
        public string TransactionID { get; set; }

        // คุณสมบัติ RefundAmount - จำนวนเงินคืน
        [Required] // บังคับให้ต้องมีค่า
        public decimal RefundAmount { get; set; }

        // คุณสมบัติ RequestID - รหัสคำร้องขอ
        [Required] // บังคับให้ต้องมีค่า
        [StringLength(30)] // จำกัดความยาวไม่เกิน 30 ตัวอักษร
        public string RequestID { get; set; }

        // คุณสมบัติ Status - สถานะของรายการ
        [Required] // บังคับให้ต้องมีค่า
        [StringLength(20)] // จำกัดความยาวไม่เกิน 20 ตัวอักษร
        public string Status { get; set; }

        // คุณสมบัติ StatusMessage - ข้อความอธิบายสถานะ (ไม่บังคับ)
        [StringLength(255)] // จำกัดความยาวไม่เกิน 255 ตัวอักษร
        public string StatusMessage { get; set; }

        // คุณสมบัติ PaymentType - ประเภทการชำระเงิน
        [Required] // บังคับให้ต้องมีค่า
        [StringLength(20)] // จำกัดความยาวไม่เกิน 20 ตัวอักษร
        public string PaymentType { get; set; }

        // คุณสมบัติ RetryCount - จำนวนครั้งที่พยายามส่งกลับ
        [Required] // บังคับให้ต้องมีค่า
        public int RetryCount { get; set; } = 0; // ค่าเริ่มต้นเป็น 0

        // คุณสมบัติ ScheduledTime - เวลาที่กำหนดสำหรับส่งกลับ
        [Required] // บังคับให้ต้องมีค่า
        public DateTime ScheduledTime { get; set; }
    }
}

// ตัวอย่างการใช้งาน:
// var enqueueRequest = new EnqueueCallbackRequest
// {
//     RefundID = 1234,
//     TerminalID = "TERM001",
//     TransactionDate = DateTime.Now,
//     TransactionID = "TRANS12345",
//     RefundAmount = 100.50m,
//     RequestID = "REQ67890",
//     Status = "Pending",
//     StatusMessage = "Refund in process",
//     PaymentType = "Credit Card",
//     RetryCount = 0,
//     ScheduledTime = DateTime.Now.AddMinutes(5)
// };