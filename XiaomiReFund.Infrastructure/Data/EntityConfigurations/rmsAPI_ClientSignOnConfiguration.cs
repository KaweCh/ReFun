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
    /// การกำหนดค่าเอนทิตี Client Sign On
    /// </summary>
    public class rmsAPI_ClientSignOnConfiguration : IEntityTypeConfiguration<rmsAPI_ClientSignOn>
    {
        public void Configure(EntityTypeBuilder<rmsAPI_ClientSignOn> builder)
        {
            // กำหนดชื่อตาราง
            builder.ToTable("rmsAPI_ClientSignOn");

            // กำหนด Primary Key
            builder.HasKey(e => e.ClientID);

            // กำหนดคุณสมบัติแต่ละฟิลด์
            builder.Property(e => e.ClientID)
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(e => e.ClientName)
                .HasMaxLength(100);

            builder.Property(e => e.ClientEmail)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(e => e.ClientUserName)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(e => e.ClientPasswordHash)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(e => e.ClientToken)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(e => e.VerifyIPAddress)
                .HasDefaultValue(false);

            builder.Property(e => e.ModifiedBy)
                .IsRequired();

            builder.Property(e => e.CreateDate)
                .HasDefaultValueSql("getdate()");

            // กำหนด Index
            builder.HasIndex(e => e.ClientEmail)
                .IsUnique();

            builder.HasIndex(e => e.ClientUserName)
                .IsUnique();

            // กำหนด Relationships
            builder.HasMany(e => e.ClientIPs)
                .WithOne(e => e.Client)
                .HasForeignKey(e => e.ClientID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(e => e.ClientTerminalIds)
                .WithOne(e => e.Client)
                .HasForeignKey(e => e.ClientID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(e => e.OrderRefunds)
                .WithOne(e => e.Client)
                .HasForeignKey(e => e.ClientID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.ModifiedByUser)
                .WithMany(e => e.ModifiedClients)
                .HasForeignKey(e => e.ModifiedBy)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
