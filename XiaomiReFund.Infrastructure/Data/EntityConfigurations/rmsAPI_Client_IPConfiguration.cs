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
    /// การกำหนดค่าเอนทิตี Client IP
    /// </summary>
    public class rmsAPI_Client_IPConfiguration : IEntityTypeConfiguration<rmsAPI_Client_IP>
    {
        public void Configure(EntityTypeBuilder<rmsAPI_Client_IP> builder)
        {
            // กำหนดชื่อตาราง
            builder.ToTable("rmsAPI_Client_IP");

            // กำหนด Primary Key
            builder.HasKey(e => e.SeqID);

            // กำหนดคุณสมบัติแต่ละฟิลด์
            builder.Property(e => e.SeqID)
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(e => e.ClientID)
                .IsRequired();

            builder.Property(e => e.IPType)
                .IsRequired();

            builder.Property(e => e.IPAddress)
                .HasMaxLength(45)
                .IsRequired();

            builder.Property(e => e.IsAllowed)
                .HasDefaultValue(false);

            builder.Property(e => e.ModifiedBy)
                .IsRequired();

            builder.Property(e => e.CreateDate)
                .HasDefaultValueSql("getdate()");

            // กำหนด Check Constraint
            builder.HasCheckConstraint("CK_IPType", "([IPType]=(6) OR [IPType]=(4))");

            // กำหนด Indexes
            builder.HasIndex(e => new { e.ClientID, e.IPAddress })
                .IsUnique();

            // กำหนด Relationships
            builder.HasOne(e => e.Client)
                .WithMany(e => e.ClientIPs)
                .HasForeignKey(e => e.ClientID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.ModifiedByUser)
                .WithMany(e => e.ModifiedClientIps)
                .HasForeignKey(e => e.ModifiedBy)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
