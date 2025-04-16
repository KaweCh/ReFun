using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XiaomiReFund.Application.Common.Models;
using XiaomiReFund.Application.DTOs.Refund;

namespace XiaomiReFund.Application.Queries.Refund.GetRefundByTerminalAndRequestId
{
    /// <summary>
    /// คำสั่งสำหรับดึงข้อมูลการคืนเงินตาม Terminal และ Request ID
    /// </summary>
    public class GetRefundByTerminalAndRequestIdQuery : IRequest<ApiResponse<RefundStatusDto>>
    {
        /// <summary>
        /// รหัสเทอร์มินัล
        /// </summary>
        public string TerminalID { get; set; }

        /// <summary>
        /// รหัสคำขอ
        /// </summary>
        public string RequestID { get; set; }
    }
}
