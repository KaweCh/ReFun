using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XiaomiReFund.Domain.Entities;

namespace XiaomiReFund.Domain.Interfaces.Repositories
{
    /// <summary>
    /// อินเทอร์เฟซสำหรับการเข้าถึงข้อมูลลูกค้า
    /// </summary>
    public interface IClientRepository : IBaseRepository<rmsAPI_ClientSignOn>
    {
        /// <summary>
        /// ค้นหาข้อมูลลูกค้าตามชื่อผู้ใช้
        /// </summary>
        /// <param name="username">ชื่อผู้ใช้</param>
        /// <returns>ข้อมูลลูกค้า หรือ null ถ้าไม่พบ</returns>
        Task<rmsAPI_ClientSignOn> GetByUsernameAsync(string username);

        /// <summary>
        /// ค้นหาข้อมูลลูกค้าตามโทเค็น
        /// </summary>
        /// <param name="token">โทเค็น</param>
        /// <returns>ข้อมูลลูกค้า หรือ null ถ้าไม่พบ</returns>
        Task<rmsAPI_ClientSignOn> GetByTokenAsync(string token);

        /// <summary>
        /// อัพเดตโทเค็นของลูกค้า
        /// </summary>
        /// <param name="clientId">รหัสลูกค้า</param>
        /// <param name="token">โทเค็นใหม่ (หรือ null เพื่อยกเลิกโทเค็น)</param>
        /// <returns>true ถ้าอัพเดตสำเร็จ, false ถ้าไม่สำเร็จ</returns>
        Task<bool> UpdateTokenAsync(int clientId, string token);

        /// <summary>
        /// ตรวจสอบความถูกต้องของโทเค็น
        /// </summary>
        /// <param name="clientId">รหัสลูกค้า</param>
        /// <param name="token">โทเค็นที่ต้องการตรวจสอบ</param>
        /// <returns>true ถ้าโทเค็นถูกต้อง, false ถ้าไม่ถูกต้อง</returns>
        Task<bool> ValidateTokenAsync(int clientId, string token);

        /// <summary>
        /// ตรวจสอบความถูกต้องของรหัสผ่าน
        /// </summary>
        /// <param name="clientId">รหัสลูกค้า</param>
        /// <param name="password">รหัสผ่านที่ต้องการตรวจสอบ</param>
        /// <returns>true ถ้ารหัสผ่านถูกต้อง, false ถ้าไม่ถูกต้อง</returns>
        Task<bool> ValidatePasswordAsync(int clientId, string password);

        /// <summary>
        /// ค้นหาข้อมูล Terminal IDs ของลูกค้า
        /// </summary>
        /// <param name="clientId">รหัสลูกค้า</param>
        /// <returns>รายการ Terminal IDs</returns>
        Task<IReadOnlyList<rmsAPI_Client_TerminalID>> GetTerminalIdsAsync(int clientId);

        /// <summary>
        /// ค้นหาข้อมูล IP ที่อนุญาตของลูกค้า
        /// </summary>
        /// <param name="clientId">รหัสลูกค้า</param>
        /// <returns>รายการ IP ที่อนุญาต</returns>
        Task<IReadOnlyList<rmsAPI_Client_IP>> GetAllowedIpsAsync(int clientId);

        /// <summary>
        /// ตรวจสอบว่า IP ได้รับอนุญาตหรือไม่
        /// </summary>
        /// <param name="clientId">รหัสลูกค้า</param>
        /// <param name="ipAddress">IP ที่ต้องการตรวจสอบ</param>
        /// <returns>true ถ้า IP ได้รับอนุญาต, false ถ้าไม่ได้รับอนุญาต</returns>
        Task<bool> IsIpAllowedAsync(int clientId, string ipAddress);
    }
}
