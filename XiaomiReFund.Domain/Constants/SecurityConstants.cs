using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaomiReFund.Domain.Constants
{
    /// <summary>
    /// ค่าคงที่สำหรับความปลอดภัย
    /// </summary>
    public static class SecurityConstants
    {
        // ค่าคงที่สำหรับการเข้ารหัส
        public static class PasswordHashing
        {
            public const int SaltSize = 16; // bytes
            public const int HashSize = 20; // bytes
            public const int Iterations = 10000; // จำนวนรอบในการแฮช
        }

        // ค่าคงที่สำหรับการยืนยันตัวตน
        public static class Authentication
        {
            public const int TokenLength = 64; // ความยาวของ token
            public const int TokenExpirationHours = 4; // ระยะเวลาหมดอายุของ token
        }

        // ค่าคงที่สำหรับการตรวจสอบ IP
        public static class IpValidation
        {
            public const byte IPv4 = 4;
            public const byte IPv6 = 6;
        }

        // ค่าคงที่สำหรับสถานะผู้ใช้
        public static class UserStatus
        {
            // ค่าเหล่านี้มาจากฐานข้อมูล sys_Users_Status
            public const byte Active = 1;
            public const byte Inactive = 2;
            public const byte Locked = 3;
            public const byte Suspended = 4;
        }
    }
}
