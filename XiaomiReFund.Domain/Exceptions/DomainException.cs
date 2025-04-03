using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaomiReFund.Domain.Exceptions
{
    /// <summary>
    /// ข้อผิดพลาดพื้นฐานของโดเมน
    /// </summary>
    public class DomainException : Exception
    {
        /// <summary>
        /// สร้างข้อผิดพลาดใหม่ด้วยข้อความ
        /// </summary>
        /// <param name="message">ข้อความแสดงข้อผิดพลาด</param>
        public DomainException(string message) : base(message)
        {
        }

        /// <summary>
        /// สร้างข้อผิดพลาดใหม่ด้วยข้อความและข้อผิดพลาดต้นเหตุ
        /// </summary>
        /// <param name="message">ข้อความแสดงข้อผิดพลาด</param>
        /// <param name="innerException">ข้อผิดพลาดต้นเหตุ</param>
        public DomainException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
