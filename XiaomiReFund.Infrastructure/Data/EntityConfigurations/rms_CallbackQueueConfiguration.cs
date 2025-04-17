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
    /// การกำหนดค่าเอนทิตี Callback Queue
    /// </summary>
    public class rms_CallbackQueueConfiguration : IEntityTypeConfiguration<rms_CallbackQueue>
    {
        public void Configure(EntityTypeBuilder<rms_CallbackQueue> builder)
        {
            // กำหนดชื่อตาราง
            builder.ToTable("rms_CallbackQueue");

            // กำหนด Primary Key
            builder.HasKey(e => e.CallbackID);

            // กำหนดคุณสมบัติแต่ละฟิลด์
            builder.Property(e => e.CallbackID)
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(e => e.RefundID)
                .IsRequired();

            builder.Property(e => e.Status)
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(e => e.StatusMessage)
                .HasMaxLength(255);

            builder.Property(e => e.RetryCount)
                .HasDefaultValue(0)
                .IsRequired();

            builder.Property(e => e.ScheduledTime)
                .HasColumnType("datetime")
                .IsRequired();

            builder.Property(e => e.IsProcessed)
                .HasDefaultValue(false)
                .IsRequired();

            builder.Property(e => e.ProcessedTime)
                .HasColumnType("datetime");

            builder.Property(e => e.CreateDate)
                .HasDefaultValueSql("getdate()")
                .IsRequired();

            builder.Property(e => e.UpdateDate)
                .HasColumnType("datetime");

            // กำหนด Indexes
            builder.HasIndex(e => e.RefundID);
            builder.HasIndex(e => e.IsProcessed);
            builder.HasIndex(e => e.ScheduledTime);
            builder.HasIndex(e => e.ProcessedTime);

            // กำหนด Relationships
            builder.HasOne(e => e.Refund)
                .WithMany()
                .HasForeignKey(e => e.RefundID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
