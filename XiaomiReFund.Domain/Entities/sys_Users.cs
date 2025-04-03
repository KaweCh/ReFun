using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaomiReFund.Domain.Entities
{
    public class sys_Users
    {
        public int UserID { get; private set; }
        public string FullName { get; private set; }
        public string Email { get; private set; }
        public string UserName { get; private set; }
        public string PasswordHash { get; private set; }
        public string PasswordSalt { get; private set; }
        public string OneTimePassword { get; private set; }
        public byte RoleID { get; private set; }
        public byte UserStatus { get; private set; }
        public bool LoggedOn { get; private set; }
        public int ModifiedBy { get; private set; }
        public DateTime CreateDate { get; private set; }
        public DateTime? UpdateDate { get; private set; }

        // Navigation properties
        public virtual sys_Users_Status UserStatusEntity { get; private set; }
        public virtual sys_Users ModifiedByUser { get; private set; }
        public virtual ICollection<sys_Users> ModifiedUsers { get; private set; }
        public virtual ICollection<rmsAPI_ClientSignOn> ModifiedClients { get; private set; }
        public virtual ICollection<rmsAPI_Client_IP> ModifiedClientIps { get; private set; }
        public virtual ICollection<rmsAPI_Client_PaymentType> ModifiedClientPaymentTypes { get; private set; }
        public virtual ICollection<rmsAPI_Client_TerminalID> ModifiedClientTerminalIds { get; private set; }
        public virtual ICollection<rms_OrderRefund> ModifiedOrderRefunds { get; private set; }
        public virtual ICollection<rms_PaymentType> ModifiedPaymentTypes { get; private set; }

        private sys_Users()
        {
            // Required by EF Core
            ModifiedUsers = new List<sys_Users>();
            ModifiedClients = new List<rmsAPI_ClientSignOn>();
            ModifiedClientIps = new List<rmsAPI_Client_IP>();
            ModifiedClientPaymentTypes = new List<rmsAPI_Client_PaymentType>();
            ModifiedClientTerminalIds = new List<rmsAPI_Client_TerminalID>();
            ModifiedOrderRefunds = new List<rms_OrderRefund>();
            ModifiedPaymentTypes = new List<rms_PaymentType>();
        }

        // Factory method to create a new user
        public static sys_Users Create(
            string fullName,
            string email,
            string userName,
            string passwordHash,
            string passwordSalt,
            byte roleId,
            byte userStatus,
            int modifiedBy)
        {
            return new sys_Users
            {
                FullName = fullName,
                Email = email,
                UserName = userName,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                RoleID = roleId,
                UserStatus = userStatus,
                LoggedOn = false,
                ModifiedBy = modifiedBy,
                CreateDate = DateTime.Now
            };
        }

        // Method to update user status
        public void UpdateStatus(byte userStatus, int modifiedBy)
        {
            UserStatus = userStatus;
            ModifiedBy = modifiedBy;
            UpdateDate = DateTime.Now;
        }

        // Method to update login status
        public void UpdateLoginStatus(bool loggedOn)
        {
            LoggedOn = loggedOn;
            UpdateDate = DateTime.Now;
        }

        // Method to update user information
        public void UpdateUserInfo(
            string fullName,
            string email,
            byte roleId,
            int modifiedBy)
        {
            FullName = fullName;
            Email = email;
            RoleID = roleId;
            ModifiedBy = modifiedBy;
            UpdateDate = DateTime.Now;
        }

        // Method to update password
        public void UpdatePassword(
            string passwordHash,
            string passwordSalt,
            int modifiedBy)
        {
            PasswordHash = passwordHash;
            PasswordSalt = passwordSalt;
            ModifiedBy = modifiedBy;
            UpdateDate = DateTime.Now;
        }

        // Method to set one-time password
        public void SetOneTimePassword(string oneTimePassword)
        {
            OneTimePassword = oneTimePassword;
            UpdateDate = DateTime.Now;
        }
    }
}
