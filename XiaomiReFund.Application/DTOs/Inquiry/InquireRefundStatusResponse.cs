using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XiaomiReFund.Application.Common.Models;
using XiaomiReFund.Application.DTOs.Refund;

namespace XiaomiReFund.Application.DTOs.Inquiry
{
    /// <summary>
    /// ผลลัพธ์การสอบถามสถานะการคืนเงิน
    /// </summary>
    public class InquireRefundStatusResponse
    {
        /// <summary>
        /// ข้อมูลผลลัพธ์
        /// </summary>
        public ResultData Result { get; set; }

        /// <summary>
        /// ข้อมูลคำขอ
        /// </summary>
        public RefundDto Request { get; set; }

        /// <summary>
        /// สร้าง InquireRefundStatusResponse ใหม่
        /// </summary>
        public InquireRefundStatusResponse()
        {
            Result = new ResultData();
        }
    }
}
