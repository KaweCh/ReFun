using System; 
using System.Collections.Generic; 
using System.ComponentModel.DataAnnotations; 
using System.Linq; 
using System.Text;
using System.Threading.Tasks; 

namespace XiaomiReFund.Application.DTOs.Auth 
{
    // คลาสนี้เป็น Data Transfer Object (DTO) สำหรับคำร้องขอรีเฟรชโทเค็น
    public class RefreshTokenRequest
    {
        // คุณสมบัติ UserID 
        [Required] // Attribute ที่ระบุว่าฟิลด์นี้จำเป็นต้องมีค่า (ห้ามเป็นค่าว่าง)
        public int UserID { get; set; } // รหัสประจำตัวผู้ใช้

        // คุณสมบัติ Token
        [Required] // Attribute ที่ระบุว่าฟิลด์นี้จำเป็นต้องมีค่า (ห้ามเป็นค่าว่าง)
        [StringLength(255)] // Attribute ที่กำหนดความยาวสูงสุดของสตริง (ไม่เกิน 255 ตัวอักษร)
        public string Token { get; set; } // โทเค็นเดิมที่ต้องการรีเฟรช
    }
}

// คำอธิบายเพิ่มเติม:
// - คลาสนี้ใช้เพื่อส่งคำร้องขอรีเฟรชโทเค็นใหม่
// - ใช้ Data Annotations เพื่อตรวจสอบความถูกต้องของข้อมูล
//   - [Required] บังคับให้ต้องระบุค่า UserID และ Token
//   - [StringLength] จำกัดความยาวของ Token

// ตัวอย่างการใช้งาน:
// var refreshRequest = new RefreshTokenRequest 
// {
//     UserID = 12345, 
//     Token = "oldTokenStringHere" 
// };