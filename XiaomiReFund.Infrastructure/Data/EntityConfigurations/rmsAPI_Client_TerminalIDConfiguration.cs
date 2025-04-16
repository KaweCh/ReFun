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
    /// การกำหนดค่าเอนทิตี Client Terminal ID
    /// </summary>
    public class rmsAPI_Client_TerminalIDConfiguration : IEntityTypeConfiguration<rmsAPI_Client_TerminalID>
    {
        public void Configure(EntityTypeBuilder<rmsAPI_Client_TerminalID> builder)
        {
            // กำหนดชื่อตาราง
            builder.ToTable("rmsAPI_Client_TerminalID");

            // กำหนด Primary Key
            builder.HasKey(e => e.TerminalID);

            // กำหนดคุณสมบัติแต่ละฟิลด์
            builder.Property(e => e.TerminalID)
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(e => e.ClientID)
                .IsRequired();

            builder.Property(e => e.IsAllowed)
                .HasDefaultValue(false);

            builder.Property(e => e.IsCheckPaymentType)
                .HasDefaultValue(false);

            builder.Property(e => e.ModifiedBy)
                .IsRequired();

            builder.Property(e => e.CreateDate)
                .HasDefaultValueSql("getdate()");

            // กำหนด Indexes
            builder.HasIndex(e => e.ClientID);

            // กำหนด Relationships
            builder.HasOne(e => e.Client)
                .WithMany(e => e.ClientTerminalIds)
                .HasForeignKey(e => e.ClientID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.ModifiedByUser)
                .WithMany(e => e.ModifiedClientTerminalIds)
                .HasForeignKey(e => e.ModifiedBy)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(e => e.PaymentTypes)
                .WithOne(e => e.Terminal)
                .HasForeignKey(e => e.TerminalID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(e => e.OrderRefunds)
                .WithOne(e => e.Terminal)
                .HasForeignKey(e => e.TerminalID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
