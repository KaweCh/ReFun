using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaomiReFund.Domain.Exceptions
{
    /// <summary>
    /// ข้อผิดพลาดเมื่อไม่พบข้อมูล
    /// </summary>
    public class NotFoundException : DomainException
    {
        /// <summary>
        /// ประเภทของเอนทิตี้ที่ไม่พบ
        /// </summary>
        public string EntityType { get; }

        /// <summary>
        /// คีย์ของเอนทิตี้ที่ใช้ในการค้นหา
        /// </summary>
        public object EntityKey { get; }

        /// <summary>
        /// สร้างข้อผิดพลาดใหม่ด้วยข้อความ
        /// </summary>
        /// <param name="message">ข้อความแสดงข้อผิดพลาด</param>
        public NotFoundException(string message) : base(message)
        {
        }

        /// <summary>
        /// สร้างข้อผิดพลาดใหม่ด้วยประเภทของเอนทิตี้และคีย์
        /// </summary>
        /// <param name="entityType">ประเภทของเอนทิตี้</param>
        /// <param name="entityKey">คีย์ของเอนทิตี้</param>
        public NotFoundException(string entityType, object entityKey)
            : base($"เอนทิตี้ '{entityType}' ที่มีคีย์ '{entityKey}' ไม่พบ")
        {
            EntityType = entityType;
            EntityKey = entityKey;
        }

        /// <summary>
        /// สร้างข้อผิดพลาดใหม่ด้วยข้อความและข้อผิดพลาดต้นเหตุ
        /// </summary>
        /// <param name="message">ข้อความแสดงข้อผิดพลาด</param>
        /// <param name="innerException">ข้อผิดพลาดต้นเหตุ</param>
        public NotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
