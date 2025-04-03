using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaomiReFund.Domain.Entities
{
    public class sys_Users_Status
    {
        public byte UserStatus { get; private set; }
        public string StatusDescription { get; private set; }

        // Navigation properties
        public virtual ICollection<sys_Users> Users { get; private set; }

        private sys_Users_Status()
        {
            // Required by EF Core
            Users = new List<sys_Users>();
        }

        // Factory method to create a new user status
        public static sys_Users_Status Create(byte userStatus, string statusDescription)
        {
            return new sys_Users_Status
            {
                UserStatus = userStatus,
                StatusDescription = statusDescription
            };
        }
    }
}
