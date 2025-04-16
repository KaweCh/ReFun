using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaomiReFund.Domain.Entities
{
    public class rms_PaymentType
    {
        public string PaymentType { get;  set; }
        public string PaymentName { get;  set; }
        public bool IsActive { get;  set; }
        public int ModifiedBy { get;  set; }
        public DateTime CreateDate { get;  set; }
        public DateTime? UpdateDate { get;  set; }

        // Navigation properties
        public virtual sys_Users ModifiedByUser { get;  set; }
        public virtual ICollection<rmsAPI_Client_PaymentType> ClientPaymentTypes { get;  set; }
        public virtual ICollection<rms_OrderRefund> OrderRefunds { get;  set; }

         rms_PaymentType()
        {
            // Required by EF Core
            ClientPaymentTypes = new List<rmsAPI_Client_PaymentType>();
            OrderRefunds = new List<rms_OrderRefund>();
        }

        // Factory method to create a new payment type
        public static rms_PaymentType Create(
            string paymentType,
            string paymentName,
            bool isActive,
            int modifiedBy)
        {
            return new rms_PaymentType
            {
                PaymentType = paymentType,
                PaymentName = paymentName,
                IsActive = isActive,
                ModifiedBy = modifiedBy,
                CreateDate = DateTime.Now
            };
        }

        // Method to update payment type information
        public void UpdateInfo(string paymentName, bool isActive, int modifiedBy)
        {
            PaymentName = paymentName;
            IsActive = isActive;
            ModifiedBy = modifiedBy;
            UpdateDate = DateTime.Now;
        }
    }
}
