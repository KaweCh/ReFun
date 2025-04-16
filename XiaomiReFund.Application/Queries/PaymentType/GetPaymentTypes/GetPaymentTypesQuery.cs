using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XiaomiReFund.Application.DTOs.PaymentType;

namespace XiaomiReFund.Application.Queries.PaymentType.GetPaymentTypes
{
    /// <summary>
    /// คำสั่งสำหรับดึงประเภทการชำระเงินทั้งหมด
    /// </summary>
    public class GetPaymentTypesQuery : IRequest<PaymentTypeListResponse>
    {
        // ไม่มีพารามิเตอร์เนื่องจากเป็นการดึงข้อมูลทั้งหมด
    }
}
