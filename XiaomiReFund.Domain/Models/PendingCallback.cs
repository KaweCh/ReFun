using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaomiReFund.Domain.Models
{
    /// <summary>
    /// โมเดลข้อมูล callback ที่รอการส่ง
    /// </summary>
    public class PendingCallback
    {
        /// <summary>
        /// รหัส callback (ถ้ามี)
        /// </summary>
        public int? CallbackID { get; set; }

        /// <summary>
        /// สถานะของรายการ
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// ข้อความเพิ่มเติม
        /// </summary>
        public string StatusMessage { get; set; }

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
        /// จำนวนเงินคืน
        /// </summary>
        public decimal RefundAmount { get; set; }

        /// <summary>
        /// รหัสคำร้องขอ
        /// </summary>
        public string RequestID { get; set; }

        /// <summary>
        /// ประเภทการชำระเงิน
        /// </summary>
        public string PaymentType { get; set; }

        /// <summary>
        /// จำนวนครั้งที่ลองใหม่
        /// </summary>
        public int RetryCount { get; set; }
    }
}
