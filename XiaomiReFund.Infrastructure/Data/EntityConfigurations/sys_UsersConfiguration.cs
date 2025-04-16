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
    /// การกำหนดค่าเอนทิตี Users
    /// </summary>
    public class sys_UsersConfiguration : IEntityTypeConfiguration<sys_Users>
    {
        public void Configure(EntityTypeBuilder<sys_Users> builder)
        {
            // กำหนดชื่อตาราง
            builder.ToTable("sys_Users");

            // กำหนด Primary Key
            builder.HasKey(e => e.UserID);

            // กำหนดคุณสมบัติแต่ละฟิลด์
            builder.Property(e => e.UserID)
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(e => e.FullName)
                .HasMaxLength(100);

            builder.Property(e => e.Email)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(e => e.PasswordSalt)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(e => e.OneTimePassword)
                .HasMaxLength(255);

            builder.Property(e => e.RoleID)
                .IsRequired();

            builder.Property(e => e.UserStatus)
                .HasDefaultValue((byte)0);

            builder.Property(e => e.LoggedOn)
                .HasDefaultValue(false);

            builder.Property(e => e.ModifiedBy)
                .IsRequired();

            builder.Property(e => e.CreateDate)
                .HasDefaultValueSql("getdate()");

            builder.Property(e => e.UpdateDate)
                .HasDefaultValueSql("getdate()");

            // กำหนด Unique Constraints
            builder.HasIndex(e => e.Email)
                .IsUnique();

            builder.HasIndex(e => e.UserName)
                .IsUnique();

            // กำหนด Relationships
            builder.HasOne(e => e.UserStatusEntity)
                .WithMany(e => e.Users)
                .HasForeignKey(e => e.UserStatus)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.ModifiedByUser)
                .WithMany(e => e.ModifiedUsers)
                .HasForeignKey(e => e.ModifiedBy)
                .OnDelete(DeleteBehavior.Restrict);

            // กำหนดการส่งคำสั่ง SQL ที่เหมาะสมสำหรับแต่ละครั้งที่อัพเดท
            // เพื่อป้องกันการ lock ข้อมูลเป็นเวลานาน
            builder.UsePropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
