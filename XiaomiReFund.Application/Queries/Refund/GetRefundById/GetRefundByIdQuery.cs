using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XiaomiReFund.Application.Common.Models;
using XiaomiReFund.Application.DTOs.Refund;

namespace XiaomiReFund.Application.Queries.Refund.GetRefundById
{
    /// <summary>
    /// คำสั่งสำหรับดึงข้อมูลการคืนเงินตาม ID
    /// </summary>
    public class GetRefundByIdQuery : IRequest<ApiResponse<RefundStatusDto>>
    {
        /// <summary>
        /// รหัสรายการคืนเงิน
        /// </summary>
        public int RefundID { get; set; }
    }
}
