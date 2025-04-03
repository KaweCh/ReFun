using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XiaomiReFund.Application.Common.Models;

namespace XiaomiReFund.Application.DTOs.Callback
{
    // คลาสนี้เป็น Data Transfer Object (DTO) สำหรับการตอบกลับการเข้าคิวการส่งกลับ
    public class EnqueueCallbackResponse
    {
        // คุณสมบัติ Result - เก็บข้อมูลผลลัพธ์การดำเนินการ
        // ใช้คลาส ResultData ซึ่งเป็นคลาสมาตรฐานสำหรับส่งผลลัพธ์ของการทำงาน
        public ResultData Result { get; set; }

        // คุณสมบัติ IsQueued - บ่งชี้ว่าการส่งกลับถูกเข้าคิวสำเร็จหรือไม่
        public bool IsQueued { get; set; }

        // คุณสมบัติ ScheduledTime - เวลาที่กำหนดสำหรับส่งกลับ (อาจเป็นค่า null)
        public DateTime? ScheduledTime { get; set; }

        // Constructor เริ่มต้นของคลาส
        // สร้าง ResultData ใหม่ทุกครั้งที่มีการสร้าง EnqueueCallbackResponse
        public EnqueueCallbackResponse()
        {
            Result = new ResultData();
        }
    }
}

// ตัวอย่างการใช้งาน:
// var enqueueResponse = new EnqueueCallbackResponse
// {
//     Result = new ResultData 
//     {
//         Success = true,
//         Message = "Callback successfully queued"
//     },
//     IsQueued = true,
//     ScheduledTime = DateTime.Now.AddMinutes(5)
// };