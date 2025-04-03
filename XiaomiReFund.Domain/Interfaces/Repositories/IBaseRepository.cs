using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace XiaomiReFund.Domain.Interfaces.Repositories
{
    /// <summary>
    /// อินเทอร์เฟซพื้นฐานสำหรับการเข้าถึงข้อมูล
    /// </summary>
    /// <typeparam name="T">ประเภทของเอนทิตี</typeparam>
    public interface IBaseRepository<T> where T : class
    {
        /// <summary>
        /// ดึงรายการทั้งหมด
        /// </summary>
        /// <returns>รายการทั้งหมด</returns>
        Task<IReadOnlyList<T>> GetAllAsync();

        /// <summary>
        /// ดึงข้อมูลตาม ID
        /// </summary>
        /// <param name="id">รหัสอ้างอิง</param>
        /// <returns>ข้อมูลที่พบ หรือ null ถ้าไม่พบ</returns>
        Task<T> GetByIdAsync(int id);

        /// <summary>
        /// ดึงข้อมูลตามเงื่อนไข
        /// </summary>
        /// <param name="predicate">เงื่อนไขในการค้นหา</param>
        /// <returns>รายการที่ตรงตามเงื่อนไข</returns>
        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// เพิ่มข้อมูลใหม่
        /// </summary>
        /// <param name="entity">ข้อมูลที่ต้องการเพิ่ม</param>
        /// <returns>รหัสอ้างอิงที่สร้างขึ้น</returns>
        Task<int> AddAsync(T entity);

        /// <summary>
        /// อัพเดตข้อมูล
        /// </summary>
        /// <param name="entity">ข้อมูลที่ต้องการอัพเดต</param>
        /// <returns>true ถ้าอัพเดตสำเร็จ, false ถ้าไม่สำเร็จ</returns>
        Task<bool> UpdateAsync(T entity);

        /// <summary>
        /// ลบข้อมูล
        /// </summary>
        /// <param name="entity">ข้อมูลที่ต้องการลบ</param>
        /// <returns>true ถ้าลบสำเร็จ, false ถ้าไม่สำเร็จ</returns>
        Task<bool> DeleteAsync(T entity);

        /// <summary>
        /// ลบข้อมูลตาม ID
        /// </summary>
        /// <param name="id">รหัสอ้างอิง</param>
        /// <returns>true ถ้าลบสำเร็จ, false ถ้าไม่สำเร็จ</returns>
        Task<bool> DeleteByIdAsync(int id);

        /// <summary>
        /// ตรวจสอบว่ามีข้อมูลที่ตรงตามเงื่อนไขหรือไม่
        /// </summary>
        /// <param name="predicate">เงื่อนไขในการค้นหา</param>
        /// <returns>true ถ้ามีข้อมูล, false ถ้าไม่มี</returns>
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// นับจำนวนข้อมูลทั้งหมด
        /// </summary>
        /// <returns>จำนวนข้อมูลทั้งหมด</returns>
        Task<int> CountAsync();

        /// <summary>
        /// นับจำนวนข้อมูลตามเงื่อนไข
        /// </summary>
        /// <param name="predicate">เงื่อนไขในการค้นหา</param>
        /// <returns>จำนวนข้อมูลที่ตรงตามเงื่อนไข</returns>
        Task<int> CountAsync(Expression<Func<T, bool>> predicate);
    }
}
