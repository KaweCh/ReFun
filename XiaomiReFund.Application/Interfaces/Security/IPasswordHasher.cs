using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaomiReFund.Application.Interfaces.Security
{
    /// <summary>
    /// อินเทอร์เฟซสำหรับบริการเข้ารหัสรหัสผ่าน
    /// </summary>
    public interface IPasswordHasher
    {
        /// <summary>
        /// เข้ารหัสรหัสผ่าน
        /// </summary>
        /// <param name="password">รหัสผ่านที่ยังไม่ได้เข้ารหัส</param>
        /// <returns>รหัสผ่านที่เข้ารหัสแล้ว</returns>
        string HashPassword(string password);

        /// <summary>
        /// เข้ารหัสรหัสผ่านด้วยค่า salt ที่กำหนด
        /// </summary>
        /// <param name="password">รหัสผ่านที่ยังไม่ได้เข้ารหัส</param>
        /// <param name="salt">ค่า salt</param>
        /// <returns>รหัสผ่านที่เข้ารหัสแล้ว</returns>
        string HashPassword(string password, string salt);

        /// <summary>
        /// ตรวจสอบความถูกต้องของรหัสผ่าน
        /// </summary>
        /// <param name="hashedPassword">รหัสผ่านที่เข้ารหัสแล้ว</param>
        /// <param name="providedPassword">รหัสผ่านที่ต้องการตรวจสอบ</param>
        /// <returns>ผลการตรวจสอบความถูกต้อง</returns>
        bool VerifyPassword(string hashedPassword, string providedPassword);

        /// <summary>
        /// ตรวจสอบความถูกต้องของรหัสผ่านด้วยค่า salt ที่กำหนด
        /// </summary>
        /// <param name="hashedPassword">รหัสผ่านที่เข้ารหัสแล้ว</param>
        /// <param name="providedPassword">รหัสผ่านที่ต้องการตรวจสอบ</param>
        /// <param name="salt">ค่า salt</param>
        /// <returns>ผลการตรวจสอบความถูกต้อง</returns>
        bool VerifyPassword(string hashedPassword, string providedPassword, string salt);

        /// <summary>
        /// สร้างค่า salt ใหม่
        /// </summary>
        /// <returns>ค่า salt ที่สร้างขึ้น</returns>
        string GenerateSalt();
    }
}
