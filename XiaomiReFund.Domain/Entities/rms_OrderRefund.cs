using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaomiReFund.Domain.Entities
{
    public class rms_OrderRefund
    {
        public int RefundID { get;  set; }
        public string RequestID { get;  set; }
        public string TerminalID { get;  set; }
        public DateTime TransactionDate { get;  set; }
        public string TransactionID { get;  set; }
        public string PaymentType { get;  set; }
        public decimal RefundAmount { get;  set; }
        public byte TxnStatus { get;  set; }
        public int ClientID { get;  set; }
        public int ModifiedBy { get;  set; }
        public DateTime CreateDate { get;  set; }
        public DateTime? UpdateDate { get;  set; }

        // Navigation properties
        public virtual rmsAPI_ClientSignOn Client { get;  set; }
        public virtual rmsAPI_Client_TerminalID Terminal { get;  set; }
        public virtual rms_PaymentType PaymentTypeEntity { get;  set; }
        public virtual rms_OrderRefundStatus Status { get;  set; }
        public virtual sys_Users ModifiedByUser { get;  set; }

         rms_OrderRefund()
        {
            // Required by EF Core
        }

        // Factory method to create a new order refund
        public static rms_OrderRefund Create(
            string requestId,
            string terminalId,
            DateTime transactionDate,
            string transactionId,
            string paymentType,
            decimal refundAmount,
            byte txnStatus,
            int clientId,
            int modifiedBy)
        {
            return new rms_OrderRefund
            {
                RequestID = requestId,
                TerminalID = terminalId,
                TransactionDate = transactionDate,
                TransactionID = transactionId,
                PaymentType = paymentType,
                RefundAmount = refundAmount,
                TxnStatus = txnStatus,
                ClientID = clientId,
                ModifiedBy = modifiedBy,
                CreateDate = DateTime.Now
            };
        }

        // Method to update transaction status
        public void UpdateStatus(byte txnStatus, int modifiedBy)
        {
            TxnStatus = txnStatus;
            ModifiedBy = modifiedBy;
            UpdateDate = DateTime.Now;
        }
    }
}
