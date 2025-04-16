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
    /// การกำหนดค่าเอนทิตี Users Status
    /// </summary>
    public class sys_Users_StatusConfiguration : IEntityTypeConfiguration<sys_Users_Status>
    {
        public void Configure(EntityTypeBuilder<sys_Users_Status> builder)
        {
            // กำหนดชื่อตาราง
            builder.ToTable("sys_Users_Status");

            // กำหนด Primary Key
            builder.HasKey(e => e.UserStatus);

            // กำหนดคุณสมบัติแต่ละฟิลด์
            builder.Property(e => e.UserStatus)
                .ValueGeneratedNever()
                .IsRequired();

            builder.Property(e => e.StatusDescription)
                .HasMaxLength(100)
                .IsRequired();

            // กำหนด Relationships
            builder.HasMany(e => e.Users)
                .WithOne(e => e.UserStatusEntity)
                .HasForeignKey(e => e.UserStatus)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
