using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XiaomiReFund.Domain.Entities;

namespace XiaomiReFund.Infrastructure.Data.EntityConfigurations
{
    /// <summary>
    /// การกำหนดค่าเอนทิตี Order Refund Status
    /// </summary>
    public class rms_OrderRefundStatusConfiguration : IEntityTypeConfiguration<rms_OrderRefundStatus>
    {
        public void Configure(EntityTypeBuilder<rms_OrderRefundStatus> builder)
        {
            // กำหนดชื่อตาราง
            builder.ToTable("rms_OrderRefundStatus");

            // กำหนด Primary Key
            builder.HasKey(e => e.TxnStatus);

            // กำหนดคุณสมบัติแต่ละฟิลด์
            builder.Property(e => e.TxnStatus)
                .ValueGeneratedNever()
                .IsRequired();

            builder.Property(e => e.StatusDescription)
                .HasMaxLength(100)
                .IsRequired();

            // กำหนด Relationships
            builder.HasMany(e => e.OrderRefunds)
                .WithOne(e => e.Status)
                .HasForeignKey(e => e.TxnStatus)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
