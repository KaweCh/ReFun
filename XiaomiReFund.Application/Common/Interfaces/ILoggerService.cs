using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaomiReFund.Application.Common.Interfaces
{
    // อินเทอร์เฟซสำหรับบริการบันทึก
    // ออกแบบให้ครอบคลุมการบันทึกเหตุการณ์ต่างๆ ในระบบ
    public interface ILoggerService
    {
        // บันทึกการร้องขอ (Request)
        // requestName: ชื่อของคำร้องขอ
        // request: อ็อบเจ็กต์ของคำร้องขอ
        // userId: รหัสผู้ใช้ที่ทำการร้องขอ
        void LogRequest(string requestName, object request, int userId);

        // บันทึกการตอบกลับ (Response)
        // requestName: ชื่อของคำร้องขอ
        // response: อ็อบเจ็กต์ของการตอบกลับ
        // elapsedMilliseconds: เวลาที่ใช้ในการประมวลผล (หน่วยมิลลิวินาที)
        void LogResponse(string requestName, object response, long elapsedMilliseconds);

        // บันทึกข้อผิดพลาด
        // requestName: ชื่อของคำร้องขอ
        // exception: อ็อบเจ็กต์ข้อยกเว้นที่เกิดขึ้น
        // userId: รหัสผู้ใช้ที่เกิดข้อผิดพลาด
        void LogError(string requestName, Exception exception, int userId);

        // บันทึกคำเตือนเกี่ยวกับประสิทธิภาพ
        // requestName: ชื่อของคำร้องขอ
        // elapsedMilliseconds: เวลาที่ใช้ในการประมวลผล (หน่วยมิลลิวินาที)
        // userId: รหัสผู้ใช้ที่ทำการร้องขอ
        void LogPerformanceWarning(string requestName, long elapsedMilliseconds, int userId);

        // บันทึกการเรียกกลับ (Callback)
        // endpoint: จุดปลายทางของการเรียกกลับ
        // request: อ็อบเจ็กต์ของคำร้องขอ
        // response: อ็อบเจ็กต์ของการตอบกลับ
        // isSuccess: สถานะความสำเร็จของการเรียกกลับ
        // retryCount: จำนวนครั้งที่พยายาม
        void LogCallback(string endpoint, object request, object response, bool isSuccess, int retryCount);

        // บันทึกความสำเร็จในการรับรองความถูกต้อง
        // username: ชื่อผู้ใช้
        // userId: รหัสผู้ใช้
        void LogAuthenticationSuccess(string username, int userId);

        // บันทึกความล้มเหลวในการรับรองความถูกต้อง
        // username: ชื่อผู้ใช้
        // reason: เหตุผลของความล้มเหลว
        void LogAuthenticationFailure(string username, string reason);
    }
}

// ตัวอย่างการใช้งาน (Pseudocode):
// public class LoggerServiceImplementation : ILoggerService 
// {
//     public void LogRequest(string requestName, object request, int userId)
//     {
//         // บันทึกรายละเอียดคำร้องขอ เช่น ลงในไฟล์ หรือส่งไปยังระบบบันทึก
//     }
//
//     // การประกาศเมธอดอื่นๆ ตามอินเทอร์เฟซ
// }