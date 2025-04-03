using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XiaomiReFund.Application.DTOs.Callback;

namespace XiaomiReFund.Application.Commands.Callback.EnqueueCallback
{
    /// <summary>
    /// คำสั่งสำหรับการเพิ่ม callback เข้าคิว
    /// </summary>
    public class EnqueueCallbackCommand : IRequest<EnqueueCallbackResponse>
    {
        /// <summary>
        /// รหัสรายการคืนเงิน
        /// </summary>
        public int RefundID { get; set; }

        /// <summary>
        /// รหัสเทอร์มินัล
        /// </summary>
        public string TerminalID { get; set; }

        /// <summary>
        /// วันที่ทำรายการ
        /// </summary>
        public DateTime TransactionDate { get; set; }

        /// <summary>
        /// รหัสธุรกรรม
        /// </summary>
        public string TransactionID { get; set; }

        /// <summary>
        /// จำนวนเงินที่คืน
        /// </summary>
        public decimal RefundAmount { get; set; }

        /// <summary>
        /// รหัสคำขอ
        /// </summary>
        public string RequestID { get; set; }

        /// <summary>
        /// สถานะของ callback (Approved, Rejected)
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// ข้อความสถานะ
        /// </summary>
        public string StatusMessage { get; set; }

        /// <summary>
        /// ประเภทการชำระเงิน
        /// </summary>
        public string PaymentType { get; set; }

        /// <summary>
        /// จำนวนครั้งที่ลองใหม่
        /// </summary>
        public int RetryCount { get; set; }

        /// <summary>
        /// เวลาที่กำหนดให้ส่ง
        /// </summary>
        public DateTime ScheduledTime { get; set; }
    }
}
