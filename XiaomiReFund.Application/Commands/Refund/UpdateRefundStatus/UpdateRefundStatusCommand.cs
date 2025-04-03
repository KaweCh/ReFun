using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XiaomiReFund.Application.Common.Models;

namespace XiaomiReFund.Application.Commands.Refund.UpdateRefundStatus
{
    /// <summary>
    /// คำสั่งสำหรับการอัพเดตสถานะการคืนเงิน
    /// </summary>
    public class UpdateRefundStatusCommand : IRequest<ApiResponse<bool>>
    {
        /// <summary>
        /// รหัสรายการคืนเงิน
        /// </summary>
        public int RefundID { get; set; }

        /// <summary>
        /// สถานะใหม่
        /// </summary>
        public byte Status { get; set; }

        /// <summary>
        /// ข้อความสถานะ
        /// </summary>
        public string StatusRemark { get; set; }
    }
}
