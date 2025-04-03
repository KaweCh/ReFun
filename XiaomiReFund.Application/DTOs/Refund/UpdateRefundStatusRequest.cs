using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaomiReFund.Application.DTOs.Refund
{
    public class UpdateRefundStatusRequest
    {
        [Required]
        public int RefundID { get; set; }

        [Required]
        public byte TxnStatus { get; set; }

        [StringLength(255)]
        public string StatusRemark { get; set; }

        [Required]
        public int ModifiedBy { get; set; }
    }
}
