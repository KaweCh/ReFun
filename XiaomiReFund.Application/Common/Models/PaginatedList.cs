using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaomiReFund.Application.Common.Models
{
    /// <summary>
    /// แบบจำลองสำหรับรายการข้อมูลแบบแบ่งหน้า
    /// </summary>
    /// <typeparam name="T">ประเภทข้อมูลในรายการ</typeparam>
    public class PaginatedList<T>
    {
        /// <summary>
        /// รายการข้อมูล
        /// </summary>
        public List<T> Items { get; }

        /// <summary>
        /// หน้าปัจจุบัน
        /// </summary>
        public int PageIndex { get; }

        /// <summary>
        /// จำนวนหน้าทั้งหมด
        /// </summary>
        public int TotalPages { get; }

        /// <summary>
        /// จำนวนรายการทั้งหมด
        /// </summary>
        public int TotalCount { get; }

        /// <summary>
        /// สร้าง PaginatedList ใหม่
        /// </summary>
        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            TotalCount = count;
            Items = items;
        }

        /// <summary>
        /// เช็คว่ามีหน้าก่อนหน้านี้หรือไม่
        /// </summary>
        public bool HasPreviousPage => PageIndex > 1;

        /// <summary>
        /// เช็คว่ามีหน้าถัดไปหรือไม่
        /// </summary>
        public bool HasNextPage => PageIndex < TotalPages;

        /// <summary>
        /// สร้าง PaginatedList จาก IQueryable
        /// </summary>
        public static PaginatedList<T> Create(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }

        /// <summary>
        /// สร้าง PaginatedList จาก IEnumerable
        /// </summary>
        public static PaginatedList<T> Create(IEnumerable<T> source, int pageIndex, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }
    }
}