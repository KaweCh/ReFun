using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using XiaomiReFund.Domain.Interfaces.Repositories;
using XiaomiReFund.Infrastructure.Data.DbContext;

namespace XiaomiReFund.Infrastructure.Data.Repositories
{
    /// <summary>
    /// คลาสพื้นฐานสำหรับการเข้าถึงข้อมูล
    /// ใช้เป็นฐานสำหรับคลาสการเข้าถึงข้อมูลอื่นๆ
    /// </summary>
    /// <typeparam name="T">ประเภทของเอนทิตี</typeparam>
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly RefundDbContext _context;
        protected readonly DbSet<T> _entities;

        /// <summary>
        /// สร้าง BaseRepository ใหม่
        /// </summary>
        /// <param name="context">บริบทฐานข้อมูล</param>
        public BaseRepository(RefundDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            // T คือ entity
            _entities = _context.Set<T>();
        }

        /// <summary>
        /// เพิ่มข้อมูลใหม่
        /// </summary>
        /// <param name="entity">ข้อมูลที่ต้องการเพิ่ม</param>
        /// <returns>รหัสอ้างอิงที่สร้างขึ้น</returns>
        public async Task<int> AddAsync(T entity)
        {
            await _entities.AddAsync(entity);
            await _context.SaveChangesAsync();

            // คืนค่า ID ที่สร้างขึ้น (ถ้ามี)
            // หมายเหตุ: ต้องปรับใช้ให้เหมาะสมกับแต่ละเอนทิตี
            var idProperty = entity.GetType().GetProperty("ID") ??
                             entity.GetType().GetProperty("Id") ??
                             entity.GetType().GetProperty(entity.GetType().Name + "ID") ??
                             entity.GetType().GetProperty(entity.GetType().Name + "Id");

            return idProperty != null ? (int)idProperty.GetValue(entity) : 0;
        }

        /// <summary>
        /// นับจำนวนข้อมูลทั้งหมด
        /// </summary>
        /// <returns>จำนวนข้อมูลทั้งหมด</returns>
        public Task<int> CountAsync()
        {
            return _entities.CountAsync();
        }

        /// <summary>
        /// นับจำนวนข้อมูลตามเงื่อนไข
        /// </summary>
        /// <param name="predicate">เงื่อนไขในการค้นหา</param>
        /// <returns>จำนวนข้อมูลที่ตรงตามเงื่อนไข</returns>
        public Task<int> CountAsync(Expression<Func<T, bool>> predicate)
        {
            return _entities.CountAsync(predicate);
        }

        /// <summary>
        /// ลบข้อมูล
        /// </summary>
        /// <param name="entity">ข้อมูลที่ต้องการลบ</param>
        /// <returns>true ถ้าลบสำเร็จ, false ถ้าไม่สำเร็จ</returns>
        public async Task<bool> DeleteAsync(T entity)
        {
            _entities.Remove(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// ลบข้อมูลตาม ID
        /// </summary>
        /// <param name="id">รหัสอ้างอิง</param>
        /// <returns>true ถ้าลบสำเร็จ, false ถ้าไม่สำเร็จ</returns>
        public async Task<bool> DeleteByIdAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null)
                return false;

            _entities.Remove(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// ตรวจสอบว่ามีข้อมูลที่ตรงตามเงื่อนไขหรือไม่
        /// </summary>
        /// <param name="predicate">เงื่อนไขในการค้นหา</param>
        /// <returns>true ถ้ามีข้อมูล, false ถ้าไม่มี</returns>
        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            return await _entities.AnyAsync(predicate);
        }

        /// <summary>
        /// ดึงรายการทั้งหมด
        /// </summary>
        /// <returns>รายการทั้งหมด</returns>
        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _entities.ToListAsync();
        }

        /// <summary>
        /// ดึงข้อมูลตามเงื่อนไข
        /// </summary>
        /// <param name="predicate">เงื่อนไขในการค้นหา</param>
        /// <returns>รายการที่ตรงตามเงื่อนไข</returns>
        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await _entities.Where(predicate).ToListAsync();
        }

        /// <summary>
        /// ดึงข้อมูลตาม ID
        /// </summary>
        /// <param name="id">รหัสอ้างอิง</param>
        /// <returns>ข้อมูลที่พบ หรือ null ถ้าไม่พบ</returns>
        public async Task<T> GetByIdAsync(int id)
        {
            return await _entities.FindAsync(id);
        }

        /// <summary>
        /// อัพเดตข้อมูล
        /// </summary>
        /// <param name="entity">ข้อมูลที่ต้องการอัพเดต</param>
        /// <returns>true ถ้าอัพเดตสำเร็จ, false ถ้าไม่สำเร็จ</returns>
        public async Task<bool> UpdateAsync(T entity)
        {
            _entities.Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
