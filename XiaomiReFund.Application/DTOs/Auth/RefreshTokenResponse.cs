﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XiaomiReFund.Application.Common.Models;


namespace XiaomiReFund.Application.DTOs.Auth
{
    // คลาสนี้เป็น Data Transfer Object (DTO) สำหรับการตอบกลับการรีเฟรชโทเค็น
    public class RefreshTokenResponse
    {
        // คุณสมบัติ Result เก็บข้อมูลผลลัพธ์การดำเนินการ
        // ใช้คลาส ResultData ซึ่งเป็นคลาสมาตรฐานสำหรับส่งผลลัพธ์ของการทำงาน
        public ResultData Result { get; set; }

        // คุณสมบัติ UserID เก็บรหัสประจำตัวผู้ใช้
        public int UserID { get; set; }

        // คุณสมบัติ Token เก็บโทเค็นใหม่หลังจากรีเฟรช
        public string Token { get; set; }

        // คุณสมบัติ Expire เก็บระยะเวลาหมดอายุของโทเค็นใหม่
        // อาจเก็บเป็นจำนวนวินาทีหรือ timestamp ที่โทเค็นจะหมดอายุ
        public int Expire { get; set; }

        // Constructor เริ่มต้นของคลาส
        // สร้าง ResultData ใหม่ทุกครั้งที่มีการสร้าง RefreshTokenResponse
        public RefreshTokenResponse()
        {
            Result = new ResultData();
        }
    }
}

// ตัวอย่างการใช้งาน:
// var refreshResponse = new RefreshTokenResponse 
// {
//     UserID = 12345,
//     Token = "newRefreshedTokenString...",
//     Expire = 3600, // หมดอายุใน 1 ชั่วโมง
//     Result = new ResultData 
//     {
//         Success = true,
//         Message = "Token refreshed successfully"
//     }
// };