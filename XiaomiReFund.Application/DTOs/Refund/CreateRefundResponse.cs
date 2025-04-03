using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XiaomiReFund.Application.Common.Models;


namespace XiaomiReFund.Application.DTOs.Refund
{
    public class CreateRefundResponse
    {
        public ResultData Result { get; set; }
        public RefundDto Request { get; set; }

        public CreateRefundResponse()
        {
            Result = new ResultData();
        }
    }
}
