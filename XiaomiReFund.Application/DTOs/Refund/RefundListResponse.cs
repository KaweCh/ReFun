using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XiaomiReFund.Application.Common.Models;


namespace XiaomiReFund.Application.DTOs.Refund
{
    public class RefundListResponse
    {
        public ResultData Result { get; set; }
        public List<RefundStatusDto> Refunds { get; set; }

        public RefundListResponse()
        {
            Result = new ResultData();
            Refunds = new List<RefundStatusDto>();
        }
    }
}
