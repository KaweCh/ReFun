using esgCryption;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XiaomiReFund.Application.Common.Interfaces;
using XiaomiReFund.Domain.Entities;
using XiaomiReFund.Domain.Interfaces.Repositories;
using XiaomiReFund.Infrastructure.Data.DbContext;

namespace XiaomiReFund.Infrastructure.Data.Repositories
{
    /// <summary>
    /// การเข้าถึงข้อมูลลูกค้า
    /// </summary>
    public class ClientRepository : BaseRepository<rmsAPI_ClientSignOn>, IClientRepository
    {
        private readonly IDateTime _dateTime;

        /// <summary>
        /// สร้าง ClientRepository ใหม่
        /// </summary>
        /// <param name="context">บริบทฐานข้อมูล</param>
        /// <param name="dateTime">บริการวันเวลา</param>
        public ClientRepository(IDateTime dateTime, RefundDbContext context) : base(context)
        {
            _dateTime = dateTime;
        }

        /// <summary>
        /// ค้นหาข้อมูล IP ที่อนุญาตของลูกค้า
        /// </summary>
        /// <param name="clientId">รหัสลูกค้า</param>
        /// <returns>รายการ IP ที่อนุญาต</returns>
        public async Task<IReadOnlyList<rmsAPI_Client_IP>> GetAllowedIpsAsync(int clientId)
        {
            return await _context.ClientIPs.Where(ip => ip.ClientID == clientId && ip.IsAllowed).ToListAsync();
        }

        /// <summary>
        /// ตรวจสอบว่า IP ได้รับอนุญาตหรือไม่
        /// </summary>
        /// <param name="clientId">รหัสลูกค้า</param>
        /// <param name="ipAddress">IP ที่ต้องการตรวจสอบ</param>
        /// <returns>true ถ้า IP ได้รับอนุญาต, false ถ้าไม่ได้รับอนุญาต</returns>
        public async Task<rmsAPI_ClientSignOn> GetByTokenAsync(string token)
        {
            return await _context.ClientSignOns.FirstOrDefaultAsync(c => c.ClientToken == token);
        }

        /// <summary>
        /// ค้นหาข้อมูลลูกค้าตามชื่อผู้ใช้
        /// </summary>
        /// <param name="username">ชื่อผู้ใช้</param>
        /// <returns>ข้อมูลลูกค้า หรือ null ถ้าไม่พบ</returns>
        public async Task<rmsAPI_ClientSignOn> GetByUsernameAsync(string username)
        {
            return await _context.ClientSignOns.FirstOrDefaultAsync(c => c.ClientUserName == username);
        }

        /// <summary>
        /// ค้นหาข้อมูล Terminal IDs ของลูกค้า
        /// </summary>
        /// <param name="clientId">รหัสลูกค้า</param>
        /// <returns>รายการ Terminal IDs</returns>
        public async Task<IReadOnlyList<rmsAPI_Client_TerminalID>> GetTerminalIdsAsync(int clientId)
        {
            return await _context.ClientTerminalIDs.Where(t => t.ClientID == clientId).ToListAsync();
        }

        /// <summary>
        /// ตรวจสอบว่า IP ได้รับอนุญาตหรือไม่
        /// </summary>
        /// <param name="clientId">รหัสลูกค้า</param>
        /// <param name="ipAddress">IP ที่ต้องการตรวจสอบ</param>
        /// <returns>true ถ้า IP ได้รับอนุญาต, false ถ้าไม่ได้รับอนุญาต</returns>
        public async Task<bool> IsIpAllowedAsync(int clientId, string ipAddress)
        {
            // ตรวจสอบว่าลูกค้าต้องการตรวจสอบ IP หรือไม่
            var client = await _context.ClientSignOns.FindAsync(clientId);
            if(client == null || !client.VerifyIPAddress)
            {
                return true; // ถ้าไม่ต้องการตรวจสอบ IP ให้ผ่านไปเลย
            }

            // ตรวจสอบว่า IP อยู่ในรายการที่อนุญาตหรือไม่
            return await _context.ClientIPs.AnyAsync(ip => ip.IPAddress == ipAddress && ip.ClientID == clientId  && ip.IsAllowed);
        }

        /// <summary>
        /// อัพเดตโทเค็นของลูกค้า
        /// </summary>
        /// <param name="clientId">รหัสลูกค้า</param>
        /// <param name="token">โทเค็นใหม่ (หรือ null เพื่อยกเลิกโทเค็น)</param>
        /// <returns>true ถ้าอัพเดตสำเร็จ, false ถ้าไม่สำเร็จ</returns>
        public async Task<bool> UpdateTokenAsync(int clientId, string token)
        {
            var client = await _context.ClientSignOns.FindAsync(clientId);
            if(client == null)
            {
                return false;
            }

            client.ClientToken = token;
            client.UpdateDate = DateTime.UtcNow;

            _context.ClientSignOns.Update(client);
            return await _context.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// ตรวจสอบความถูกต้องของรหัสผ่าน
        /// </summary>
        /// <param name="clientId">รหัสลูกค้า</param>
        /// <param name="password">รหัสผ่านที่ต้องการตรวจสอบ</param>
        /// <returns>true ถ้ารหัสผ่านถูกต้อง, false ถ้าไม่ถูกต้อง</returns>
        public async Task<bool> ValidatePasswordAsync(int clientId, string password)
        {
            var client = await _context.ClientSignOns.FindAsync(clientId);
            if(client == null)
            {
                return false;
            }

            return rmsSecurity.Decryption(client.ClientPasswordHash, client.SaltKey) == password;
        }

        /// <summary>
        /// ตรวจสอบความถูกต้องของโทเค็น
        /// </summary>
        /// <param name="clientId">รหัสลูกค้า</param>
        /// <param name="token">โทเค็นที่ต้องการตรวจสอบ</param>
        /// <returns>true ถ้าโทเค็นถูกต้อง, false ถ้าไม่ถูกต้อง</returns>
        public async Task<bool> ValidateTokenAsync(int clientId, string token)
        {
            var client = await _context.ClientSignOns.FirstOrDefaultAsync(c => c.ClientID == clientId && c.ClientToken == token);

            return client != null;
        }
    }
}
