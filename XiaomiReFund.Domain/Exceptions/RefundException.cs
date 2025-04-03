using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaomiReFund.Domain.Exceptions
{
    /// <summary>
    /// ข้อผิดพลาดเฉพาะสำหรับการคืนเงิน
    /// </summary>
    public class RefundException : DomainException
    {
        /// <summary>
        /// รหัสข้อผิดพลาด
        /// </summary>
        public string ErrorCode { get; }

        /// <summary>
        /// ข้อมูลเพิ่มเติม
        /// </summary>
        public object AdditionalData { get; }

        /// <summary>
        /// สร้างข้อผิดพลาดใหม่ด้วยข้อความ
        /// </summary>
        /// <param name="message">ข้อความแสดงข้อผิดพลาด</param>
        public RefundException(string message) : base(message)
        {
        }

        /// <summary>
        /// สร้างข้อผิดพลาดใหม่ด้วยข้อความและรหัสข้อผิดพลาด
        /// </summary>
        /// <param name="message">ข้อความแสดงข้อผิดพลาด</param>
        /// <param name="errorCode">รหัสข้อผิดพลาด</param>
        public RefundException(string message, string errorCode) : base(message)
        {
            ErrorCode = errorCode;
        }

        /// <summary>
        /// สร้างข้อผิดพลาดใหม่ด้วยข้อความ รหัสข้อผิดพลาด และข้อมูลเพิ่มเติม
        /// </summary>
        /// <param name="message">ข้อความแสดงข้อผิดพลาด</param>
        /// <param name="errorCode">รหัสข้อผิดพลาด</param>
        /// <param name="additionalData">ข้อมูลเพิ่มเติม</param>
        public RefundException(string message, string errorCode, object additionalData) : base(message)
        {
            ErrorCode = errorCode;
            AdditionalData = additionalData;
        }

        /// <summary>
        /// สร้างข้อผิดพลาดใหม่ด้วยข้อความและข้อผิดพลาดต้นเหตุ
        /// </summary>
        /// <param name="message">ข้อความแสดงข้อผิดพลาด</param>
        /// <param name="innerException">ข้อผิดพลาดต้นเหตุ</param>
        public RefundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// สร้างข้อผิดพลาดใหม่ด้วยข้อความ รหัสข้อผิดพลาด และข้อผิดพลาดต้นเหตุ
        /// </summary>
        /// <param name="message">ข้อความแสดงข้อผิดพลาด</param>
        /// <param name="errorCode">รหัสข้อผิดพลาด</param>
        /// <param name="innerException">ข้อผิดพลาดต้นเหตุ</param>
        public RefundException(string message, string errorCode, Exception innerException) : base(message, innerException)
        {
            ErrorCode = errorCode;
        }
    }
}
