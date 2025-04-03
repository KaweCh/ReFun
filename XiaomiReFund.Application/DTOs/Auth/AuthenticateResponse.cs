using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XiaomiReFund.Application.Common.Models;


namespace XiaomiReFund.Application.DTOs.Auth
{
    // คลาสนี้เป็น Data Transfer Object (DTO) สำหรับการตอบกลับการรับรองความถูกต้อง (Authentication Response)
    public class AuthenticateResponse
    {
        // คุณสมบัติ Result เก็บข้อมูลผลลัพธ์การดำเนินการ
        // ใช้คลาส ResultData ซึ่งน่าจะเป็นคลาสมาตรฐานสำหรับส่งผลลัพธ์ของการทำงาน
        public ResultData Result { get; set; }

        // คุณสมบัติ UserID เก็บรหัสประจำตัวผู้ใช้
        public int UserID { get; set; }

        // คุณสมบัติ Token เก็บโทเค็นการรับรองความถูกต้อง
        // โทเค็นนี้มักใช้สำหรับการตรวจสอบสิทธิ์ในการเข้าถึงทรัพยากรอื่นๆ
        public string Token { get; set; }

        // คุณสมบัติ Expire เก็บระยะเวลาหมดอายุของโทเค็น
        // อาจเก็บเป็นจำนวนวินาทีหรือ timestamp ที่โทเค็นจะหมดอายุ
        public int Expire { get; set; }

        // Constructor เริ่มต้นของคลาส
        // สร้าง ResultData ใหม่ทุกครั้งที่มีการสร้าง AuthenticateResponse
        public AuthenticateResponse()
        {
            Result = new ResultData();
        }
    }
}

// ตัวอย่างการใช้งาน:
// var authResponse = new AuthenticateResponse 
// {
//     UserID = 12345,
//     Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
//     Expire = 3600, // หมดอายุใน 1 ชั่วโมง
//     Result = new ResultData 
//     {
//         Success = true,
//         Message = "Login successful"
//     }
// };
