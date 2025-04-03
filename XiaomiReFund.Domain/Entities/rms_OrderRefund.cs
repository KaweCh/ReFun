using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaomiReFund.Domain.Entities
{
    public class rms_OrderRefund
    {
        public int RefundID { get; private set; }
        public string RequestID { get; private set; }
        public string TerminalID { get; private set; }
        public DateTime TransactionDate { get; private set; }
        public string TransactionID { get; private set; }
        public string PaymentType { get; private set; }
        public decimal RefundAmount { get; private set; }
        public byte TxnStatus { get; private set; }
        public int ClientID { get; private set; }
        public int ModifiedBy { get; private set; }
        public DateTime CreateDate { get; private set; }
        public DateTime? UpdateDate { get; private set; }

        // Navigation properties
        public virtual rmsAPI_ClientSignOn Client { get; private set; }
        public virtual rmsAPI_Client_TerminalID Terminal { get; private set; }
        public virtual rms_PaymentType PaymentTypeEntity { get; private set; }
        public virtual rms_OrderRefundStatus Status { get; private set; }
        public virtual sys_Users ModifiedByUser { get; private set; }

        private rms_OrderRefund()
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
