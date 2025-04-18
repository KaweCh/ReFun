using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using XiaomiReFund.Application.Common.Interfaces;
using XiaomiReFund.Domain.Entities;

namespace XiaomiReFund.Infrastructure.Data.DbContext
{
    /// <summary>
    /// บริบทฐานข้อมูลสำหรับระบบคืนเงิน
    /// </summary>
    public class RefundDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTime _dateTime;

        /// <summary>
        /// สร้าง RefundDbContext ใหม่
        /// </summary>
        /// <param name="options">ตัวเลือกสำหรับ DbContext</param>
        /// <param name="currentUserService">บริการข้อมูลผู้ใช้ปัจจุบัน</param>
        /// <param name="dateTime">บริการวันเวลา</param>
        public RefundDbContext(
            DbContextOptions<RefundDbContext> options,
            ICurrentUserService currentUserService,
            IDateTime dateTime) : base(options)
        {
            _currentUserService = currentUserService;
            _dateTime = dateTime;
        }

        #region DbSets

        /// <summary>
        /// ข้อมูลลูกค้า
        /// </summary>
        public DbSet<rmsAPI_ClientSignOn> ClientSignOns { get; set; }

        /// <summary>
        /// ข้อมูล IP ของลูกค้า
        /// </summary>
        public DbSet<rmsAPI_Client_IP> ClientIPs { get; set; }

        /// <summary>
        /// ข้อมูลประเภทการชำระเงินของลูกค้า
        /// </summary>
        public DbSet<rmsAPI_Client_PaymentType> ClientPaymentTypes { get; set; }

        /// <summary>
        /// ข้อมูล Terminal ID ของลูกค้า
        /// </summary>
        public DbSet<rmsAPI_Client_TerminalID> ClientTerminalIDs { get; set; }

        /// <summary>
        /// ข้อมูลการคืนเงิน
        /// </summary>
        public DbSet<rms_OrderRefund> OrderRefunds { get; set; }

        /// <summary>
        /// ข้อมูลสถานะการคืนเงิน
        /// </summary>
        public DbSet<rms_OrderRefundStatus> OrderRefundStatuses { get; set; }

        /// <summary>
        /// ข้อมูลประเภทการชำระเงิน
        /// </summary>
        public DbSet<rms_PaymentType> PaymentTypes { get; set; }

        /// <summary>
        /// ข้อมูลผู้ใช้
        /// </summary>
        public DbSet<sys_Users> Users { get; set; }

        /// <summary>
        /// ข้อมูลสถานะผู้ใช้
        /// </summary>
        public DbSet<sys_Users_Status> UserStatuses { get; set; }

        /// <summary>
        /// ข้อมูลสถานะผู้ใช้
        /// </summary>
        public DbSet<rms_CallbackQueue> CallbackQueue { get; set; }

        #endregion

        /// <summary>
        /// กำหนดค่าโมเดล (Entity) ในขณะสร้าง DbContext
        /// </summary>
        /// <param name="modelBuilder">ModelBuilder สำหรับกำหนดค่าโมเดล</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // นำเข้าการกำหนดค่าทั้งหมดจาก EntityConfigurations
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// บันทึกการเปลี่ยนแปลงลงฐานข้อมูล
        /// </summary>
        /// <param name="cancellationToken">โทเค็นการยกเลิก</param>
        /// <returns>จำนวนรายการที่ได้รับผลกระทบ</returns>
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // ดักจับเอนทิตีที่มีการเพิ่มหรือแก้ไข เพื่ออัพเดต timestamp ให้อัตโนมัติ
            foreach (var entry in ChangeTracker.Entries<rms_OrderRefund>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        // ใส่ข้อมูลวันที่สร้างอัตโนมัติ
                        entry.Entity.CreateDate = _dateTime.Now;
                        break;
                    case EntityState.Modified:
                        // อัพเดตข้อมูลวันที่แก้ไขอัตโนมัติ
                        entry.Entity.UpdateDate = _dateTime.Now;
                        break;
                }
            }

            // ดักจับเอนทิตีที่มีการเพิ่มหรือแก้ไข เพื่ออัพเดต timestamp ให้อัตโนมัติ
            foreach (var entry in ChangeTracker.Entries<rmsAPI_ClientSignOn>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        // ใส่ข้อมูลวันที่สร้างอัตโนมัติ
                        entry.Entity.CreateDate = _dateTime.Now;
                        break;
                    case EntityState.Modified:
                        // อัพเดตข้อมูลวันที่แก้ไขอัตโนมัติ
                        entry.Entity.UpdateDate = _dateTime.Now;
                        break;
                }
            }

            // ดักจับเอนทิตีที่มีการเพิ่มหรือแก้ไข เพื่ออัพเดต timestamp ให้อัตโนมัติ
            foreach (var entry in ChangeTracker.Entries<rmsAPI_Client_IP>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        // ใส่ข้อมูลวันที่สร้างอัตโนมัติ
                        entry.Entity.CreateDate = _dateTime.Now;
                        break;
                    case EntityState.Modified:
                        // อัพเดตข้อมูลวันที่แก้ไขอัตโนมัติ
                        entry.Entity.UpdateDate = _dateTime.Now;
                        break;
                }
            }

            // ดักจับเอนทิตีที่มีการเพิ่มหรือแก้ไข เพื่ออัพเดต timestamp ให้อัตโนมัติ
            foreach (var entry in ChangeTracker.Entries<rms_CallbackQueue>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        // ใส่ข้อมูลวันที่สร้างอัตโนมัติ
                        entry.Entity.CreateDate = _dateTime.Now;
                        break;
                    case EntityState.Modified:
                        // อัพเดตข้อมูลวันที่แก้ไขอัตโนมัติ
                        entry.Entity.UpdateDate = _dateTime.Now;
                        break;
                }
            }

            // บันทึกการเปลี่ยนแปลงทั้งหมด
            return await base.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// กำหนดค่าการเชื่อมต่อฐานข้อมูล
        /// </summary>
        /// <param name="optionsBuilder">ตัวเลือกสำหรับ DbContext</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // เพิ่มการตั้งค่าเพื่อรองรับการทำงานที่มีประสิทธิภาพ
            if (!optionsBuilder.IsConfigured)
            {
                // เพิ่มการตั้งค่าสำหรับการจัดการกับข้อมูลจำนวนมาก
                optionsBuilder.EnableSensitiveDataLogging(false);
                optionsBuilder.EnableDetailedErrors(false);

                // ตั้งค่าสำหรับสภาพแวดล้อมที่มีการเรียก request พร้อมกันจำนวนมาก
                optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

                // สำหรับ SQL Server
                // optionsBuilder.UseSqlServer(
                //     "YourConnectionString", 
                //     sqlServerOptionsAction: sqlOptions =>
                //     {
                //         sqlOptions.EnableRetryOnFailure(
                //             maxRetryCount: 5,
                //             maxRetryDelay: TimeSpan.FromSeconds(30),
                //             errorNumbersToAdd: null);
                //         sqlOptions.CommandTimeout(60); // เพิ่ม timeout เป็น 60 วินาที
                //     });
            }

            base.OnConfiguring(optionsBuilder);
        }
    }
}
