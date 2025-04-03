using System; 
using System.Collections.Generic; 
using System.ComponentModel.DataAnnotations; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace XiaomiReFund.Application.DTOs.Auth 
{
    // คลาสนี้เป็น Data Transfer Object (DTO) สำหรับการร้องขอการรับรองความถูกต้อง (Authentication Request)
    public class AuthenticateRequest
    {
        // คุณสมบัติ Username 
        [Required] // Attribute ที่ระบุว่าฟิลด์นี้จำเป็นต้องมีค่า (ห้ามเป็นค่าว่าง)
        [StringLength(50)] // Attribute ที่กำหนดความยาวสูงสุดของสตริง (ไม่เกิน 50 ตัวอักษร)
        public string Username { get; set; } // คุณสมบัติสำหรับชื่อผู้ใช้

        // คุณสมบัติ Password
        [Required] // Attribute ที่ระบุว่าฟิลด์นี้จำเป็นต้องมีค่า (ห้ามเป็นค่าว่าง)
        [StringLength(255)] // Attribute ที่กำหนดความยาวสูงสุดของสตริง (ไม่เกิน 255 ตัวอักษร)
        public string Password { get; set; } // คุณสมบัติสำหรับรหัสผ่าน
    }
}

// คำอธิบายเพิ่มเติม:
// - คลาสนี้ใช้เพื่อรับข้อมูลการเข้าสู่ระบบ (Login)
// - Data Annotations ช่วยในการตรวจสอบความถูกต้องของข้อมูล
//   - [Required] ทำให้ต้องระบุค่า Username และ Password
//   - [StringLength] จำกัดความยาวของข้อมูล เพื่อป้องกันการป้อนข้อมูลที่lålåยาวเกินไป

// ตัวอย่างการใช้งาน:
// var loginRequest = new AuthenticateRequest 
// {
//     Username = "xiaomi_user", 
//     Password = "SecurePassword123" 
// };