using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaomiReFund.Application.DTOs.Callback
{
    // คลาสนี้เป็น Data Transfer Object (DTO) สำหรับการตอบกลับ (Callback Response)
    public class CallbackResponse
    {
        // คุณสมบัติ StatusCode เก็บรหัสสถานะ HTTP
        // ใช้สำหรับระบุผลลัพธ์ของการดำเนินการ
        public int StatusCode { get; set; }

        // คุณสมบัติ Status เก็บข้อความอธิบายสถานะ
        // ให้รายละเอียดเพิ่มเติมเกี่ยวกับผลลัพธ์ของการดำเนินการ
        public string Status { get; set; }

        // Constructor เริ่มต้นโดยไม่มีพารามิเตอร์
        // สร้างอ็อบเจ็กต์ CallbackResponse โดยไม่กำหนดค่าเริ่มต้น
        public CallbackResponse()
        {
        }

        // Constructor ที่รับพารามิเตอร์สองตัว
        // ใช้สำหรับสร้าง CallbackResponse พร้อมกำหนดรหัสสถานะและข้อความสถานะ
        public CallbackResponse(int statusCode, string status)
        {
            StatusCode = statusCode;
            Status = status;
        }

        // เมธอดสถิตย์ Accepted 
        // สร้างและคืนค่า CallbackResponse สำหรับกรณีที่การดำเนินการสำเร็จ
        // ใช้รหัสสถานะ HTTP 200 (OK) และข้อความ "Accepted"
        public static CallbackResponse Accepted()
        {
            return new CallbackResponse(200, "Accepted");
        }
    }
}

// ตัวอย่างการใช้งาน:
// 1. สร้าง CallbackResponse แบบเริ่มต้น
// var emptyResponse = new CallbackResponse();
//
// 2. สร้าง CallbackResponse ด้วยค่าที่กำหนดเอง
// var customResponse = new CallbackResponse(404, "Not Found");
//
// 3. ใช้เมธอด Accepted
// var acceptedResponse = CallbackResponse.Accepted();