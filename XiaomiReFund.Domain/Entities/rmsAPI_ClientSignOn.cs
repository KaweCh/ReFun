using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaomiReFund.Domain.Entities
{
    public class rmsAPI_ClientSignOn
    {
        public int ClientID { get;  set; }
        public string ClientName { get;  set; }
        public string ClientEmail { get;  set; }
        public string ClientUserName { get;  set; }
        public string ClientPasswordHash { get;  set; }
        public string ClientToken { get;  set; }
        public bool VerifyIPAddress { get;  set; }
        public int ModifiedBy { get;  set; }
        public DateTime CreateDate { get;  set; }
        public DateTime? UpdateDate { get;  set; }

        // Navigation properties
        public virtual ICollection<rmsAPI_Client_IP> ClientIPs { get;  set; }
        public virtual ICollection<rmsAPI_Client_TerminalID> ClientTerminalIds { get;  set; }
        public virtual ICollection<rms_OrderRefund> OrderRefunds { get;  set; }
        public virtual sys_Users ModifiedByUser { get;  set; }

        // Constructor for creating a new client
         rmsAPI_ClientSignOn()
        {
            // Required by EF Core
            ClientIPs = new List<rmsAPI_Client_IP>();
            ClientTerminalIds = new List<rmsAPI_Client_TerminalID>();
            OrderRefunds = new List<rms_OrderRefund>();
        }

        // Factory method to create a new client
        public static rmsAPI_ClientSignOn Create(
            string clientName,
            string clientEmail,
            string clientUserName,
            string clientPasswordHash,
            string clientToken,
            bool verifyIPAddress,
            int modifiedBy)
        {
            return new rmsAPI_ClientSignOn
            {
                ClientName = clientName,
                ClientEmail = clientEmail,
                ClientUserName = clientUserName,
                ClientPasswordHash = clientPasswordHash,
                ClientToken = clientToken,
                VerifyIPAddress = verifyIPAddress,
                ModifiedBy = modifiedBy,
                CreateDate = DateTime.Now
            };
        }

        // Method to update client token
        public void UpdateToken(string newToken, int modifiedBy)
        {
            ClientToken = newToken;
            ModifiedBy = modifiedBy;
            UpdateDate = DateTime.Now;
        }

        // Method to update client information
        public void UpdateClientInfo(
            string clientName,
            string clientEmail,
            bool verifyIPAddress,
            int modifiedBy)
        {
            ClientName = clientName;
            ClientEmail = clientEmail;
            VerifyIPAddress = verifyIPAddress;
            ModifiedBy = modifiedBy;
            UpdateDate = DateTime.Now;
        }

        // Method to update password
        public void UpdatePassword(string passwordHash, int modifiedBy)
        {
            ClientPasswordHash = passwordHash;
            ModifiedBy = modifiedBy;
            UpdateDate = DateTime.Now;
        }
    }
}
