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
    /// การกำหนดค่าเอนทิตี Payment Type
    /// </summary>
    public class rms_PaymentTypeConfiguration : IEntityTypeConfiguration<rms_PaymentType>
    {
        public void Configure(EntityTypeBuilder<rms_PaymentType> builder)
        {
            // กำหนดชื่อตาราง
            builder.ToTable("rms_PaymentType");

            // กำหนด Primary Key
            builder.HasKey(e => e.PaymentType);

            // กำหนดคุณสมบัติแต่ละฟิลด์
            builder.Property(e => e.PaymentType)
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(e => e.PaymentName)
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(e => e.IsActive)
                .HasDefaultValue(false);

            builder.Property(e => e.ModifiedBy)
                .IsRequired();

            builder.Property(e => e.CreateDate)
                .HasDefaultValueSql("getdate()");

            // กำหนด Relationships
            builder.HasOne(e => e.ModifiedByUser)
                .WithMany(e => e.ModifiedPaymentTypes)
                .HasForeignKey(e => e.ModifiedBy)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(e => e.ClientPaymentTypes)
                .WithOne(e => e.PaymentTypeEntity)
                .HasForeignKey(e => e.PaymentType)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(e => e.OrderRefunds)
                .WithOne(e => e.PaymentTypeEntity)
                .HasForeignKey(e => e.PaymentType)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
