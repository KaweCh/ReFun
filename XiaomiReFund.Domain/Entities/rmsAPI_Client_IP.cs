using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaomiReFund.Domain.Entities
{
    public class rmsAPI_Client_IP
    {
        public int SeqID { get; private set; }
        public int ClientID { get; private set; }
        public byte IPType { get; private set; } // 4 = IPv4, 6 = IPv6
        public string IPAddress { get; private set; }
        public bool IsAllowed { get; private set; }
        public int ModifiedBy { get; private set; }
        public DateTime CreateDate { get; private set; }
        public DateTime? UpdateDate { get; private set; }

        // Navigation properties
        public virtual rmsAPI_ClientSignOn Client { get; private set; }
        public virtual sys_Users ModifiedByUser { get; private set; }

        private rmsAPI_Client_IP()
        {
            // Required by EF Core
        }

        // Factory method to create a new client IP
        public static rmsAPI_Client_IP Create(
            int clientId,
            byte ipType,
            string ipAddress,
            bool isAllowed,
            int modifiedBy)
        {
            if (ipType != 4 && ipType != 6)
                throw new ArgumentException("IPType must be either 4 (IPv4) or 6 (IPv6)");

            return new rmsAPI_Client_IP
            {
                ClientID = clientId,
                IPType = ipType,
                IPAddress = ipAddress,
                IsAllowed = isAllowed,
                ModifiedBy = modifiedBy,
                CreateDate = DateTime.Now
            };
        }

        // Method to update IP status
        public void UpdateStatus(bool isAllowed, int modifiedBy)
        {
            IsAllowed = isAllowed;
            ModifiedBy = modifiedBy;
            UpdateDate = DateTime.Now;
        }
    }
}
