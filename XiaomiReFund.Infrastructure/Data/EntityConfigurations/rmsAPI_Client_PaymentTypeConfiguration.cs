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
    /// การกำหนดค่าเอนทิตี Client Payment Type
    /// </summary>
    public class rmsAPI_Client_PaymentTypeConfiguration : IEntityTypeConfiguration<rmsAPI_Client_PaymentType>
    {
        public void Configure(EntityTypeBuilder<rmsAPI_Client_PaymentType> builder)
        {
            // กำหนดชื่อตาราง
            builder.ToTable("rmsAPI_Client_PaymentType");

            // กำหนด Primary Key
            builder.HasKey(e => e.SeqID);

            // กำหนดคุณสมบัติแต่ละฟิลด์
            builder.Property(e => e.SeqID)
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(e => e.TerminalID)
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(e => e.PaymentType)
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(e => e.IsAllowed)
                .HasDefaultValue(false);

            builder.Property(e => e.ModifiedBy)
                .IsRequired();

            builder.Property(e => e.CreateDate)
                .HasDefaultValueSql("getdate()");

            // กำหนด Indexes
            builder.HasIndex(e => new { e.TerminalID, e.PaymentType })
                .IsUnique();

            // กำหนด Relationships
            builder.HasOne(e => e.Terminal)
                .WithMany(e => e.PaymentTypes)
                .HasForeignKey(e => e.TerminalID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.PaymentTypeEntity)
                .WithMany(e => e.ClientPaymentTypes)
                .HasForeignKey(e => e.PaymentType)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.ModifiedByUser)
                .WithMany(e => e.ModifiedClientPaymentTypes)
                .HasForeignKey(e => e.ModifiedBy)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
