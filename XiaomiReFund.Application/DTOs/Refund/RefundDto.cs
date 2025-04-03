using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaomiReFund.Application.DTOs.Refund
{
    public class RefundDto
    {
        public string TerminalID { get; set; }
        public string TransactionDate { get; set; }
        public string TransactionID { get; set; }
        public decimal RefundAmount { get; set; }
        public string RequestID { get; set; }
        public string PaymentType { get; set; }
    }
}
