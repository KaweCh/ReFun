using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaomiReFund.Application.DTOs.Refund
{
    public class CreateRefundRequest
    {
        [Required]
        [StringLength(8)]
        public string TerminalID { get; set; }

        [Required]
        [StringLength(10)]
        public string TransactionDate { get; set; }

        [Required]
        [StringLength(50)]
        public string TransactionID { get; set; }

        [Required]
        public decimal RefundAmount { get; set; }

        [Required]
        [StringLength(30)]
        public string RequestID { get; set; }

        [Required]
        [StringLength(20)]
        public string PaymentType { get; set; }
    }
}
