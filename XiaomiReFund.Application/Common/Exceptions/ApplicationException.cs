using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaomiReFund.Application.Common.Exceptions
{
    // คลาสข้อยกเว้นทั่วไปสำหรับแอปพลิเคชัน
    // ใช้สำหรับการจัดการข้อผิดพลาดที่เกิดขึ้นในชั้นแอปพลิเคชัน
    public class ApplicationException : Exception
    {
        // Constructor พื้นฐาน
        // รับข้อความอธิบายข้อผิดพลาด
        public ApplicationException(string message)
            : base(message)
        {
        }

        // Constructor สำหรับกรณีมี inner exception
        // รับข้อความอธิบายและข้อยกเว้นดั้งเดิม
        public ApplicationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}

// ตัวอย่างการใช้งาน:
// try 
// {
//     // โค้ดการทำงานที่อาจเกิดข้อผิดพลาด
//     if (someErrorCondition)
//     {
//         throw new ApplicationException("เกิดข้อผิดพลาดในการประมวลผล");
//     }
// }
// catch (ApplicationException ex)
// {
//     // จัดการข้อผิดพลาดเฉพาะของแอปพลิเคชัน
//     _logger.LogError(ex, "เกิดข้อผิดพลาดในแอปพลิเคชัน");
// }

// ตัวอย่างการใช้งานกับ inner exception:
// try 
// {
//     // บางโค้ดที่อาจโยงข้อยกเว้นอื่นๆ
//     SomeMethod();
// }
// catch (Exception ex)
// {
//     // ครอบคลุมข้อยกเว้นดั้งเดิมเพื่อให้ข้อมูลเพิ่มเติม
//     throw new ApplicationException("เกิดข้อผิดพลาดในการดำเนินการ", ex);
// }