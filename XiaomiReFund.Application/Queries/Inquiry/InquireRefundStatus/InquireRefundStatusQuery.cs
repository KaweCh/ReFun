using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XiaomiReFund.Application.DTOs.Inquiry;

namespace XiaomiReFund.Application.Queries.Inquiry.InquireRefundStatus
{
    /// <summary>
    /// คำสั่งสำหรับสอบถามสถานะการคืนเงิน
    /// </summary>
    public class InquireRefundStatusQuery : IRequest<InquireRefundStatusResponse>
    {
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
        /// จำนวนเงินที่ต้องการคืน
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
