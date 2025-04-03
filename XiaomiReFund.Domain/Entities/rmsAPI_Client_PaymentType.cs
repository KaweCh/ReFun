using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaomiReFund.Domain.Entities
{
    public class rmsAPI_Client_PaymentType
    {
        public int SeqID { get; private set; }
        public string TerminalID { get; private set; }
        public string PaymentType { get; private set; }
        public bool IsAllowed { get; private set; }
        public int ModifiedBy { get; private set; }
        public DateTime CreateDate { get; private set; }
        public DateTime? UpdateDate { get; private set; }

        // Navigation properties
        public virtual rmsAPI_Client_TerminalID Terminal { get; private set; }
        public virtual rms_PaymentType PaymentTypeEntity { get; private set; }
        public virtual sys_Users ModifiedByUser { get; private set; }

        private rmsAPI_Client_PaymentType()
        {
            // Required by EF Core
        }

        // Factory method to create a new client payment type
        public static rmsAPI_Client_PaymentType Create(
            string terminalId,
            string paymentType,
            bool isAllowed,
            int modifiedBy)
        {
            return new rmsAPI_Client_PaymentType
            {
                TerminalID = terminalId,
                PaymentType = paymentType,
                IsAllowed = isAllowed,
                ModifiedBy = modifiedBy,
                CreateDate = DateTime.Now
            };
        }

        // Method to update payment type status
        public void UpdateStatus(bool isAllowed, int modifiedBy)
        {
            IsAllowed = isAllowed;
            ModifiedBy = modifiedBy;
            UpdateDate = DateTime.Now;
        }
    }
}
