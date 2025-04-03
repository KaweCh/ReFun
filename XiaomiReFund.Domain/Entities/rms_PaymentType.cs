using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaomiReFund.Domain.Entities
{
    public class rms_PaymentType
    {
        public string PaymentType { get; private set; }
        public string PaymentName { get; private set; }
        public bool IsActive { get; private set; }
        public int ModifiedBy { get; private set; }
        public DateTime CreateDate { get; private set; }
        public DateTime? UpdateDate { get; private set; }

        // Navigation properties
        public virtual sys_Users ModifiedByUser { get; private set; }
        public virtual ICollection<rmsAPI_Client_PaymentType> ClientPaymentTypes { get; private set; }
        public virtual ICollection<rms_OrderRefund> OrderRefunds { get; private set; }

        private rms_PaymentType()
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
