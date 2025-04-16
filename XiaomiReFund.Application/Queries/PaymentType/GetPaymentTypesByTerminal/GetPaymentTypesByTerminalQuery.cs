using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XiaomiReFund.Application.DTOs.PaymentType;

namespace XiaomiReFund.Application.Queries.PaymentType.GetPaymentTypesByTerminal
{
    /// <summary>
    /// คำสั่งสำหรับดึงประเภทการชำระเงินตาม Terminal
    /// </summary>
    public class GetPaymentTypesByTerminalQuery : IRequest<PaymentTypeListResponse>
    {
        /// <summary>
        /// รหัสเทอร์มินัล
        /// </summary>
        public string TerminalID { get; set; }
    }
}
