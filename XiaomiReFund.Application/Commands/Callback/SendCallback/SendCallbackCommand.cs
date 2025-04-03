using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XiaomiReFund.Application.DTOs.Callback;

namespace XiaomiReFund.Application.Commands.Callback.SendCallback
{
    /// <summary>
    /// คำสั่งสำหรับการส่ง callback
    /// </summary>
    public class SendCallbackCommand : IRequest<CallbackResponse>
    {
        /// <summary>
        /// สถานะของ callback (Approved, Rejected)
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// ข้อความเพิ่มเติม
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// รหัสเทอร์มินัล
        /// </summary>
        public string TerminalID { get; set; }

        /// <summary>
        /// วันที่ทำรายการ
        /// </summary>
        public string TransactionDate { get; set; }

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
        /// ประเภทการชำระเงิน
        /// </summary>
        public string PaymentType { get; set; }
    }
}
