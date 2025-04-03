using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XiaomiReFund.Application.Common.Models;


namespace XiaomiReFund.Application.DTOs.PaymentType
{
    public class PaymentTypeListResponse
    {
        public ResultData Result { get; set; }
        public List<PaymentTypeResponse> PaymentType { get; set; }

        public PaymentTypeListResponse()
        {
            Result = new ResultData();
            PaymentType = new List<PaymentTypeResponse>();
        }
    }
}
