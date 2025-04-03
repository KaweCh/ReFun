using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaomiReFund.Domain.Exceptions
{
    /// <summary>
    /// ข้อผิดพลาดเกี่ยวกับการตรวจสอบข้อมูล
    /// </summary>
    public class ValidationException : DomainException
    {
        /// <summary>
        /// รายการข้อผิดพลาดการตรวจสอบ
        /// </summary>
        public IReadOnlyDictionary<string, string[]> Errors { get; }

        /// <summary>
        /// สร้างข้อผิดพลาดใหม่ด้วยข้อความ
        /// </summary>
        /// <param name="message">ข้อความแสดงข้อผิดพลาด</param>
        public ValidationException(string message) : base(message)
        {
            Errors = new Dictionary<string, string[]>();
        }

        /// <summary>
        /// สร้างข้อผิดพลาดใหม่ด้วยข้อความและรายการข้อผิดพลาด
        /// </summary>
        /// <param name="message">ข้อความแสดงข้อผิดพลาด</param>
        /// <param name="errors">รายการข้อผิดพลาด</param>
        public ValidationException(string message, IDictionary<string, string[]> errors) : base(message)
        {
            Errors = new Dictionary<string, string[]>(errors);
        }

        /// <summary>
        /// สร้างข้อผิดพลาดใหม่ด้วยข้อความและข้อผิดพลาดต้นเหตุ
        /// </summary>
        /// <param name="message">ข้อความแสดงข้อผิดพลาด</param>
        /// <param name="innerException">ข้อผิดพลาดต้นเหตุ</param>
        public ValidationException(string message, Exception innerException) : base(message, innerException)
        {
            Errors = new Dictionary<string, string[]>();
        }
    }
}
