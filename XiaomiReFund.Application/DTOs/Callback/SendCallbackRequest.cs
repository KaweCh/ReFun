using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaomiReFund.Application.DTOs.Callback
{
    // คลาสนี้เป็น Data Transfer Object (DTO) สำหรับคำร้องขอส่งการตอบกลับ
    public class SendCallbackRequest
    {
        // คุณสมบัติ Status - สถานะของรายการ
        [Required] // บังคับให้ต้องมีค่า
        [StringLength(20)] // จำกัดความยาวไม่เกิน 20 ตัวอักษร
        public string Status { get; set; }

        // คุณสมบัติ Msg - ข้อความเพิ่มเติม (ไม่บังคับ)
        [StringLength(255)] // จำกัดความยาวไม่เกิน 255 ตัวอักษร
        public string Msg { get; set; }

        // คุณสมบัติ TerminalID - รหัสเทอร์มินัล
        [Required] // บังคับให้ต้องมีค่า
        [StringLength(8)] // จำกัดความยาวไม่เกิน 8 ตัวอักษร
        public string TerminalID { get; set; }

        // คุณสมบัติ TransactionDate - วันที่ทำรายการ
        [Required] // บังคับให้ต้องมีค่า
        [StringLength(10)] // จำกัดความยาวไม่เกิน 10 ตัวอักษร
        public string TransactionDate { get; set; }

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

        // คุณสมบัติ PaymentType - ประเภทการชำระเงิน
        [Required] // บังคับให้ต้องมีค่า
        [StringLength(20)] // จำกัดความยาวไม่เกิน 20 ตัวอักษร
        public string PaymentType { get; set; }
    }
}

// ตัวอย่างการใช้งาน:
// var callbackRequest = new SendCallbackRequest
// {
//     Status = "Success",
//     Msg = "Refund processed successfully",
//     TerminalID = "TERM001",
//     TransactionDate = "2024-04-03",
//     TransactionID = "TRANS12345",
//     RefundAmount = 100.50m,
//     RequestID = "REQ67890",
//     PaymentType = "Credit Card"
// };