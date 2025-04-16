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
    /// การกำหนดค่าเอนทิตี Order Refund
    /// </summary>
    public class rms_OrderRefundConfiguration : IEntityTypeConfiguration<rms_OrderRefund>
    {
        public void Configure(EntityTypeBuilder<rms_OrderRefund> builder)
        {
            // กำหนดชื่อตาราง
            builder.ToTable("rms_OrderRefund");

            // กำหนด Primary Key
            builder.HasKey(e => e.RefundID);

            // กำหนดคุณสมบัติแต่ละฟิลด์
            builder.Property(e => e.RefundID)
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(e => e.RequestID)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(e => e.TerminalID)
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(e => e.TransactionDate)
                .HasColumnType("datetime")
                .HasDefaultValueSql("getdate()");

            builder.Property(e => e.TransactionID)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(e => e.PaymentType)
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(e => e.RefundAmount)
                .HasColumnType("decimal(18, 2)")
                .IsRequired();

            builder.Property(e => e.TxnStatus)
                .HasDefaultValue((byte)0)
                .IsRequired();

            builder.Property(e => e.ClientID)
                .IsRequired();

            builder.Property(e => e.ModifiedBy)
                .IsRequired();

            builder.Property(e => e.CreateDate)
                .HasDefaultValueSql("getdate()");

            // กำหนด Unique Constraints
            builder.HasAlternateKey(e => new { e.TerminalID, e.RequestID })
                .HasName("UQ_Terminal_Request");

            // กำหนด Indexes
            builder.HasIndex(e => new { e.TerminalID, e.TransactionID });
            builder.HasIndex(e => e.RequestID);
            builder.HasIndex(e => e.TransactionDate);
            builder.HasIndex(e => e.TxnStatus);

            // กำหนด Relationships
            builder.HasOne(e => e.Client)
                .WithMany(e => e.OrderRefunds)
                .HasForeignKey(e => e.ClientID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Terminal)
                .WithMany(e => e.OrderRefunds)
                .HasForeignKey(e => e.TerminalID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.PaymentTypeEntity)
                .WithMany(e => e.OrderRefunds)
                .HasForeignKey(e => e.PaymentType)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Status)
                .WithMany(e => e.OrderRefunds)
                .HasForeignKey(e => e.TxnStatus)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.ModifiedByUser)
                .WithMany(e => e.ModifiedOrderRefunds)
                .HasForeignKey(e => e.ModifiedBy)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
