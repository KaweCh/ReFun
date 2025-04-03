using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaomiReFund.Domain.Entities
{
    public class rms_OrderRefundStatus
    {
        public byte TxnStatus { get; private set; }
        public string StatusDescription { get; private set; }

        // Navigation properties
        public virtual ICollection<rms_OrderRefund> OrderRefunds { get; private set; }

        private rms_OrderRefundStatus()
        {
            // Required by EF Core
            OrderRefunds = new List<rms_OrderRefund>();
        }

        // Factory method to create a new order refund status
        public static rms_OrderRefundStatus Create(byte txnStatus, string statusDescription)
        {
            return new rms_OrderRefundStatus
            {
                TxnStatus = txnStatus,
                StatusDescription = statusDescription
            };
        }
    }
}
