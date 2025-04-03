using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace XiaomiReFund.Application.Common.Models
{
    /// <summary>
    /// แบบจำลองสำหรับข้อมูลผลลัพธ์
    /// </summary>
    public class ResultData
    {
        /// <summary>
        /// รหัสสถานะ HTTP
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// สถานะการตอบกลับ
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// ข้อความอธิบาย
        /// </summary>
        public string Msg { get; set; }
    }
}