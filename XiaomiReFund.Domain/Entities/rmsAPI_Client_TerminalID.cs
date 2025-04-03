using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaomiReFund.Domain.Entities
{
    public class rmsAPI_Client_TerminalID
    {
        public string TerminalID { get; private set; }
        public int ClientID { get; private set; }
        public bool IsAllowed { get; private set; }
        public bool IsCheckPaymentType { get; private set; }
        public int ModifiedBy { get; private set; }
        public DateTime CreateDate { get; private set; }
        public DateTime? UpdateDate { get; private set; }

        // Navigation properties
        public virtual rmsAPI_ClientSignOn Client { get; private set; }
        public virtual sys_Users ModifiedByUser { get; private set; }
        public virtual ICollection<rmsAPI_Client_PaymentType> PaymentTypes { get; private set; }
        public virtual ICollection<rms_OrderRefund> OrderRefunds { get; private set; }

        private rmsAPI_Client_TerminalID()
        {
            // Required by EF Core
            PaymentTypes = new List<rmsAPI_Client_PaymentType>();
            OrderRefunds = new List<rms_OrderRefund>();
        }

        // Factory method to create a new client terminal ID
        public static rmsAPI_Client_TerminalID Create(
            string terminalId,
            int clientId,
            bool isAllowed,
            bool isCheckPaymentType,
            int modifiedBy)
        {
            return new rmsAPI_Client_TerminalID
            {
                TerminalID = terminalId,
                ClientID = clientId,
                IsAllowed = isAllowed,
                IsCheckPaymentType = isCheckPaymentType,
                ModifiedBy = modifiedBy,
                CreateDate = DateTime.Now
            };
        }

        // Method to update terminal status
        public void UpdateStatus(bool isAllowed, bool isCheckPaymentType, int modifiedBy)
        {
            IsAllowed = isAllowed;
            IsCheckPaymentType = isCheckPaymentType;
            ModifiedBy = modifiedBy;
            UpdateDate = DateTime.Now;
        }
    }
}
