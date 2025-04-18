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
    /// การเข้าถึงข้อมูลประเภทการชำระเงิน
    /// </summary>
    public class PaymentTypeRepository : BaseRepository<rms_PaymentType>, IPaymentTypeRepository
    {
        private readonly IDateTime _dateTime;

        /// <summary>
        /// สร้าง PaymentTypeRepository ใหม่
        /// </summary>
        /// <param name="context">บริบทฐานข้อมูล</param>
        /// <param name="dateTime">บริการวันเวลา</param>
        public PaymentTypeRepository(IDateTime dateTime, RefundDbContext context) : base(context)
        {
            _dateTime = dateTime;
        }

        // <summary>
        /// เพิ่มประเภทการชำระเงินสำหรับเทอร์มินัล
        /// </summary>
        /// <param name="terminalId">รหัสเทอร์มินัล</param>
        /// <param name="paymentType">รหัสประเภทการชำระเงิน</param>
        /// <param name="modifiedBy">ID ของผู้ทำรายการ</param>
        /// <returns>true ถ้าเพิ่มสำเร็จ, false ถ้าไม่สำเร็จ</returns>
        public async Task<bool> AddPaymentTypeForTerminalAsync(string terminalId, string paymentType, int modifiedBy)
        {
            // ตรวจสอบว่าเทอร์มินัลมีอยู่จริงหรือไม่
            var terminal = await _context.ClientTerminalIDs.FirstOrDefaultAsync(t => t.TerminalID == terminalId && t.IsAllowed);
            if (terminal == null)
            {
                return false;
            }

            // ตรวจสอบว่าประเภทการชำระเงินมีอยู่จริงหรือไม่
            var paymentTypeEntity = await _context.PaymentTypes
                .FirstOrDefaultAsync(pt => pt.PaymentType == paymentType && pt.IsActive);
            if (paymentTypeEntity == null) 
            { 
                return false;
            }

            // ตรวจสอบว่าประเภทการชำระเงินนี้มีอยู่ในเทอร์มินัลแล้วหรือไม่
            var existingClientPaymentType = await _context.ClientPaymentTypes
                .FirstOrDefaultAsync(cpt => cpt.PaymentType == paymentType && cpt.TerminalID == terminalId);
            if (existingClientPaymentType != null)
            {
                // ถ้ามีอยู่แล้ว ให้อัพเดตเป็นเปิดใช้งาน
                existingClientPaymentType.IsAllowed = true;
                existingClientPaymentType.ModifiedBy = modifiedBy;
                existingClientPaymentType.UpdateDate = _dateTime.Now;

                _context.ClientPaymentTypes.Update(existingClientPaymentType);
            }
            else
            {
                // ถ้ายังไม่มี ให้สร้างใหม่
                var newClientPaymentType = new rmsAPI_Client_PaymentType
                {
                    TerminalID = terminalId,
                    PaymentType = paymentType,
                    IsAllowed = true,
                    ModifiedBy = modifiedBy,
                    CreateDate = _dateTime.Now,
                };

                await _context.ClientPaymentTypes.AddAsync(newClientPaymentType);
            }

            return await _context.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// ปิดใช้งานประเภทการชำระเงิน
        /// </summary>
        /// <param name="paymentType">รหัสประเภทการชำระเงิน</param>
        /// <param name="modifiedBy">ID ของผู้ทำรายการ</param>
        /// <returns>true ถ้าอัพเดตสำเร็จ, false ถ้าไม่สำเร็จ</returns>
        public async Task<bool> DisablePaymentTypeAsync(string paymentType, int modifiedBy)
        {
            var paymentTypeEntity = await _context.PaymentTypes
                .FirstOrDefaultAsync(pt => pt.PaymentName == paymentType);
            if (paymentTypeEntity == null)
            {
                return false;
            }

            paymentTypeEntity.IsActive = false;
            paymentTypeEntity.ModifiedBy = modifiedBy;
            paymentTypeEntity.UpdateDate = _dateTime.Now;

            _context.PaymentTypes.Update(paymentTypeEntity);
            return await _context.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// เปิดใช้งานประเภทการชำระเงิน
        /// </summary>
        /// <param name="paymentType">รหัสประเภทการชำระเงิน</param>
        /// <param name="modifiedBy">ID ของผู้ทำรายการ</param>
        /// <returns>true ถ้าอัพเดตสำเร็จ, false ถ้าไม่สำเร็จ</returns>
        public async Task<bool> EnablePaymentTypeAsync(string paymentType, int modifiedBy)
        {
            var paymentTypeEntity = await _context.PaymentTypes
                .FirstOrDefaultAsync(pt => pt.PaymentName == paymentType);
            if (paymentTypeEntity == null)
            {
                return false;
            }

            paymentTypeEntity.IsActive = true;
            paymentTypeEntity.ModifiedBy = modifiedBy;
            paymentTypeEntity.UpdateDate = _dateTime.Now;

            _context.PaymentTypes.Update(paymentTypeEntity);
            return await _context.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// ดึงประเภทการชำระเงินที่เปิดใช้งาน
        /// </summary>
        /// <returns>รายการประเภทการชำระเงินที่เปิดใช้งาน</returns>
        public async Task<IReadOnlyList<rms_PaymentType>> GetActivePaymentTypesAsync()
        {
           return await _context.PaymentTypes
                .Where(pt => pt.IsActive)
                .OrderBy(pt => pt.PaymentName)
                .ToListAsync();
        }

        /// <summary>
        /// ค้นหาประเภทการชำระเงินตามรหัส
        /// </summary>
        /// <param name="paymentType">รหัสประเภทการชำระเงิน</param>
        /// <returns>ข้อมูลประเภทการชำระเงิน หรือ null ถ้าไม่พบ</returns>
        public async Task<rms_PaymentType> GetPaymentTypeByCodeAsync(string paymentType)
        {
            return await _context.PaymentTypes
                .FirstOrDefaultAsync(pt => pt.PaymentType == paymentType && pt.IsActive);
        }

        /// <summary>
        /// ดึงประเภทการชำระเงินที่เปิดใช้งานสำหรับเทอร์มินัล
        /// </summary>
        /// <param name="terminalId">รหัสเทอร์มินัล</param>
        /// <returns>รายการประเภทการชำระเงินที่เปิดใช้งานสำหรับเทอร์มินัล</returns>
        public async Task<IReadOnlyList<rms_PaymentType>> GetPaymentTypesByTerminalAsync(string terminalId)
        {
            // ดึงข้อมูลเทอร์มินัล
            var terminal = await _context.ClientTerminalIDs.FirstOrDefaultAsync(t => t.TerminalID == terminalId && t.IsAllowed);
            if (terminal == null)
            {
                return new List<rms_PaymentType>();
            }

            // ถ้าไม่ต้องการตรวจสอบประเภทการชำระเงิน
            if (!terminal.IsCheckPaymentType)
            {
                return await GetActivePaymentTypesAsync();
            }

            // ดึงรายการประเภทการชำระเงินที่เปิดใช้งานสำหรับเทอร์มินัล
            return await _context.ClientPaymentTypes
                .Where(cpt => cpt.TerminalID == terminalId && cpt.IsAllowed)
                .Join(_context.PaymentTypes, 
                cpt => cpt.PaymentType,
                pt => pt.PaymentType,
                (cpt, pt) => pt)
                .OrderBy (pt => pt.PaymentName)
                .ToListAsync();
        }

        /// <summary>
        /// ตรวจสอบว่าประเภทการชำระเงินเปิดใช้งานสำหรับเทอร์มินัลหรือไม่
        /// </summary>
        /// <param name="terminalId">รหัสเทอร์มินัล</param>
        /// <param name="paymentType">รหัสประเภทการชำระเงิน</param>
        /// <returns>true ถ้าเปิดใช้งาน, false ถ้าไม่เปิดใช้งาน</returns>
        public async Task<bool> IsPaymentTypeAllowedForTerminalAsync(string terminalId, string paymentType)
        {
            // ดึงข้อมูลเทอร์มินัล
            var terminal = await _context.ClientTerminalIDs
                .FirstOrDefaultAsync(t => t.TerminalID == terminalId && t.IsAllowed);
            if(terminal == null)
            {
                return false;
            }

            // ถ้าไม่ต้องการตรวจสอบประเภทการชำระเงิน
            if (!terminal.IsCheckPaymentType)
            {
                // ตรวจสอบเฉพาะว่าประเภทการชำระเงินเปิดใช้งานหรือไม่
                return await _context.PaymentTypes
                    .AnyAsync(pt => pt.PaymentType == paymentType && pt.IsActive);
            }

            // ตรวจสอบว่าประเภทการชำระเงินเปิดใช้งานสำหรับเทอร์มินัลหรือไม่
            return await _context.ClientPaymentTypes
                .AnyAsync(cpt => cpt.TerminalID == terminalId &&
                cpt.PaymentType == paymentType &&
                cpt.IsAllowed);
        }

        public async Task<bool> RemovePaymentTypeForTerminalAsync(string terminalId, string paymentType)
        {
            var clientPaymentType = await _context.ClientPaymentTypes.FirstOrDefaultAsync(cpt => cpt.PaymentType == paymentType);
            if (clientPaymentType == null)
            {
                return false;
            }

            _context.ClientPaymentTypes.Remove(clientPaymentType);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
