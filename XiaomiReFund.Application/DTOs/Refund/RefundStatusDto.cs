using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaomiReFund.Application.DTOs.Refund
{
    public class RefundStatusDto
    {
        public int RefundID { get; set; }
        public string RequestID { get; set; }
        public string TerminalID { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionID { get; set; }
        public string PaymentType { get; set; }
        public string PaymentName { get; set; }
        public decimal RefundAmount { get; set; }
        public byte TxnStatus { get; set; }
        public string StatusDescription { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
