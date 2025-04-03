using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XiaomiReFund.Domain.Entities;

namespace XiaomiReFund.Domain.Interfaces.Repositories
{
    /// <summary>
    /// อินเทอร์เฟซสำหรับการเข้าถึงข้อมูลประเภทการชำระเงิน
    /// </summary>
    public interface IPaymentTypeRepository : IBaseRepository<rms_PaymentType>
    {
        /// <summary>
        /// ดึงประเภทการชำระเงินที่เปิดใช้งาน
        /// </summary>
        /// <returns>รายการประเภทการชำระเงินที่เปิดใช้งาน</returns>
        Task<IReadOnlyList<rms_PaymentType>> GetActivePaymentTypesAsync();

        /// <summary>
        /// ค้นหาประเภทการชำระเงินตามรหัส
        /// </summary>
        /// <param name="paymentType">รหัสประเภทการชำระเงิน</param>
        /// <returns>ข้อมูลประเภทการชำระเงิน หรือ null ถ้าไม่พบ</returns>
        Task<rms_PaymentType> GetPaymentTypeByCodeAsync(string paymentType);

        /// <summary>
        /// ดึงประเภทการชำระเงินที่เปิดใช้งานสำหรับเทอร์มินัล
        /// </summary>
        /// <param name="terminalId">รหัสเทอร์มินัล</param>
        /// <returns>รายการประเภทการชำระเงินที่เปิดใช้งานสำหรับเทอร์มินัล</returns>
        Task<IReadOnlyList<rms_PaymentType>> GetPaymentTypesByTerminalAsync(string terminalId);

        /// <summary>
        /// ตรวจสอบว่าประเภทการชำระเงินเปิดใช้งานสำหรับเทอร์มินัลหรือไม่
        /// </summary>
        /// <param name="terminalId">รหัสเทอร์มินัล</param>
        /// <param name="paymentType">รหัสประเภทการชำระเงิน</param>
        /// <returns>true ถ้าเปิดใช้งาน, false ถ้าไม่เปิดใช้งาน</returns>
        Task<bool> IsPaymentTypeAllowedForTerminalAsync(string terminalId, string paymentType);

        /// <summary>
        /// เปิดใช้งานประเภทการชำระเงิน
        /// </summary>
        /// <param name="paymentType">รหัสประเภทการชำระเงิน</param>
        /// <param name="modifiedBy">ID ของผู้ทำรายการ</param>
        /// <returns>true ถ้าอัพเดตสำเร็จ, false ถ้าไม่สำเร็จ</returns>
        Task<bool> EnablePaymentTypeAsync(string paymentType, int modifiedBy);

        /// <summary>
        /// ปิดใช้งานประเภทการชำระเงิน
        /// </summary>
        /// <param name="paymentType">รหัสประเภทการชำระเงิน</param>
        /// <param name="modifiedBy">ID ของผู้ทำรายการ</param>
        /// <returns>true ถ้าอัพเดตสำเร็จ, false ถ้าไม่สำเร็จ</returns>
        Task<bool> DisablePaymentTypeAsync(string paymentType, int modifiedBy);

        /// <summary>
        /// เพิ่มประเภทการชำระเงินสำหรับเทอร์มินัล
        /// </summary>
        /// <param name="terminalId">รหัสเทอร์มินัล</param>
        /// <param name="paymentType">รหัสประเภทการชำระเงิน</param>
        /// <param name="modifiedBy">ID ของผู้ทำรายการ</param>
        /// <returns>true ถ้าเพิ่มสำเร็จ, false ถ้าไม่สำเร็จ</returns>
        Task<bool> AddPaymentTypeForTerminalAsync(string terminalId, string paymentType, int modifiedBy);

        /// <summary>
        /// ลบประเภทการชำระเงินสำหรับเทอร์มินัล
        /// </summary>
        /// <param name="terminalId">รหัสเทอร์มินัล</param>
        /// <param name="paymentType">รหัสประเภทการชำระเงิน</param>
        /// <returns>true ถ้าลบสำเร็จ, false ถ้าไม่สำเร็จ</returns>
        Task<bool> RemovePaymentTypeForTerminalAsync(string terminalId, string paymentType);
    }
}
