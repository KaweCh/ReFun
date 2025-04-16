using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XiaomiReFund.Application.DTOs.Refund;

namespace XiaomiReFund.Application.Queries.Refund.GetRefundsByDateRange
{
    /// <summary>
    /// คำสั่งสำหรับดึงข้อมูลการคืนเงินตามช่วงวันที่
    /// </summary>
    public class GetRefundsByDateRangeQuery : IRequest<RefundListResponse>
    {
        /// <summary>
        /// วันที่เริ่มต้น
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// วันที่สิ้นสุด
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// สถานะการคืนเงิน (ถ้าระบุ)
        /// </summary>
        public byte? TxnStatus { get; set; }

        /// <summary>
        /// หน้าที่ต้องการ
        /// </summary>
        public int PageNumber { get; set; } = 1;

        /// <summary>
        /// จำนวนรายการต่อหน้า
        /// </summary>
        public int PageSize { get; set; } = 10;
    }
}
