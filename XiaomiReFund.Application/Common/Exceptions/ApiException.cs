using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaomiReFund.Application.Common.Exceptions
{
    // คลาสข้อยกเว้นที่ใช้สำหรับการจัดการข้อผิดพลาดใน API
    // ช่วยในการส่งรหัสสถานะ HTTP และข้อความแบบมีโครงสร้าง
    public class ApiException : Exception
    {
        // รหัสสถานะ HTTP ที่เกิดขึ้น
        public int StatusCode { get; }

        // ข้อความสถานะ
        public string Status { get; }

        // Constructor พื้นฐาน
        // message: ข้อความอธิบายข้อผิดพลาด
        // statusCode: รหัสสถานะ HTTP (ค่าเริ่มต้น 500 - Internal Server Error)
        // status: ข้อความสถานะ (ค่าเริ่มต้น "Error")
        public ApiException(string message, int statusCode = 500, string status = "Error")
            : base(message)
        {
            StatusCode = statusCode;
            Status = status;
        }

        // Constructor สำหรับกรณีมี inner exception
        // innerException: ข้อยกเว้นดั้งเดิมที่เกิดขึ้น
        public ApiException(string message, Exception innerException, int statusCode = 500, string status = "Error")
            : base(message, innerException)
        {
            StatusCode = statusCode;
            Status = status;
        }

        // เมธอดสถิตย์สำหรับสร้าง ApiException แบบต่างๆ

        // คำร้องขอไม่ถูกต้อง (400 Bad Request)
        public static ApiException BadRequest(string message = "Bad Request")
        {
            return new ApiException(message, 400, "Bad Request");
        }

        // ไม่ได้รับอนุญาต (401 Unauthorized)
        // มักใช้กับปัญหาการรับรองความถูกต้อง เช่น โทเค็นไม่ถูกต้อง
        public static ApiException Unauthorized(string message = "Invalid Token or Token Expired")
        {
            return new ApiException(message, 401, "Unauthorized");
        }

        // ต้องการสิทธิ์เพิ่มเติม (403 Forbidden)
        // ผู้ใช้ไม่มีสิทธิ์เข้าถึงทรัพยากร
        public static ApiException Forbidden(string message = "Forbidden")
        {
            return new ApiException(message, 403, "Forbidden");
        }

        // ไม่พบทรัพยากร (404 Not Found)
        public static ApiException NotFound(string message = "Not Found")
        {
            return new ApiException(message, 404, "Not Found");
        }

        // มีความขัดแย้งกับสถานะปัจจุบัน (409 Conflict)
        // เช่น พยายามสร้างรายการที่มีอยู่แล้ว
        public static ApiException Conflict(string message = "Already Requested")
        {
            return new ApiException(message, 409, "Existed");
        }
    }
}

// ตัวอย่างการใช้งาน:
// try 
// {
//     // โค้ดการทำงาน
//     if (userNotFound)
//         throw ApiException.NotFound("User not found");
//     
//     if (invalidRefund)
//         throw ApiException.BadRequest("Invalid refund request");
// }
// catch (ApiException ex)
// {
//     // จัดการข้อผิดพลาดพร้อมรหัสสถานะที่เหมาะสม
//     return StatusCode(ex.StatusCode, new {
//         message = ex.Message,
//         status = ex.Status
//     });
// }