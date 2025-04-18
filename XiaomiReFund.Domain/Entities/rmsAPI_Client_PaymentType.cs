using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaomiReFund.Domain.Entities
{
    public class rmsAPI_Client_PaymentType
    {
        public int SeqID { get;  set; }
        public string TerminalID { get;  set; }
        public string PaymentType { get;  set; }
        public bool IsAllowed { get;  set; }
        public int ModifiedBy { get;  set; }
        public DateTime CreateDate { get;  set; }
        public DateTime? UpdateDate { get;  set; }

        // Navigation properties
        public virtual rmsAPI_Client_TerminalID Terminal { get;  set; }
        public virtual rms_PaymentType PaymentTypeEntity { get;  set; }
        public virtual sys_Users ModifiedByUser { get;  set; }

        public rmsAPI_Client_PaymentType()
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
