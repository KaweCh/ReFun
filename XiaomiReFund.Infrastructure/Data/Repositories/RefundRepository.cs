using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XiaomiReFund.Application.Common.Interfaces;
using XiaomiReFund.Application.DTOs.Callback;
using XiaomiReFund.Domain.Constants;
using XiaomiReFund.Domain.Entities;
using XiaomiReFund.Domain.Interfaces.Repositories;
using XiaomiReFund.Domain.Models;
using XiaomiReFund.Infrastructure.Data.DbContext;

namespace XiaomiReFund.Infrastructure.Data.Repositories
{
    /// <summary>
    /// การใช้งานคลังข้อมูลการคืนเงิน
    /// </summary>
    public class RefundRepository : BaseRepository<rms_OrderRefund>, IRefundRepository
    {
        private readonly IDateTime _dateTime;
        public RefundRepository(RefundDbContext context, IDateTime dateTime) : base(context)
        {
            _dateTime = dateTime;
        }

        /// <summary>
        /// สร้างข้อมูลการคืนเงินใหม่
        /// </summary>
        /// <param name="terminalId">รหัสเทอร์มินัล</param>
        /// <param name="transactionDate">วันที่ทำรายการ</param>
        /// <param name="transactionId">รหัสธุรกรรม</param>
        /// <param name="paymentType">ประเภทการชำระเงิน</param>
        /// <param name="refundAmount">จำนวนเงินที่คืน</param>
        /// <param name="requestId">รหัสคำขอ</param>
        /// <param name="clientId">รหัสลูกค้า</param>
        /// <param name="modifiedBy">รหัสผู้สร้างรายการ</param>
        /// <returns>รหัสของรายการคืนเงินที่สร้าง</returns>
        public async Task<int> CreateRefundAsync(string terminalId, DateTime transactionDate, string transactionId, string paymentType, decimal refundAmount, string requestId, int clientId, int modifiedBy)
        {
            var refund = rms_OrderRefund.Create(
                    requestId,
                    terminalId,
                    transactionDate,
                    transactionId,
                    paymentType,
                    refundAmount,
                    RefundConstants.TransactionStatus.Pending,
                    clientId,
                    modifiedBy
                );

            // เพิ่มลงในคอนเท็กซ์
            _context.OrderRefunds.Add( refund );
           await _context.SaveChangesAsync();

            return refund.RefundID;
        }

        /// <summary>
        /// ดึงข้อมูลสถานะการคืนเงินทั้งหมด
        /// </summary>
        /// <returns>รายการสถานะการคืนเงินทั้งหมด</returns>
        public async Task<IReadOnlyList<rms_OrderRefundStatus>> GetAllRefundStatusesAsync()
        {
            return await _context.OrderRefundStatuses.ToListAsync();
        }

        /// <summary>
        /// ดึงข้อมูลการคืนเงินตาม Terminal และ Request ID
        /// </summary>
        /// <param name="terminalId">รหัสเทอร์มินัล</param>
        /// <param name="requestId">รหัสคำขอ</param>
        /// <returns>ข้อมูลการคืนเงิน หรือ null ถ้าไม่พบ</returns>
        public async Task<rms_OrderRefund> GetByTerminalAndRequestIdAsync(string terminalId, string requestId)
        {
            return await _context.OrderRefunds.FirstOrDefaultAsync(r => r.TerminalID == terminalId && r.RequestID == requestId);
        }

        /// <summary>
        /// ดึง URL ปลายทางของ callback จากเทอร์มินัล
        /// </summary>
        /// <param name="terminalId">Terminal ID</param>
        /// <returns>URL ปลายทางของ callback</returns>
        public async Task<string> GetCallbackUrlAsync(string terminalId)
        {
            var clientId = await _context.ClientTerminalIDs
                .Where(c => c.TerminalID == terminalId)
                .Select(c => c.ClientID)
                .FirstOrDefaultAsync();

            if (clientId == null)
            {
                return string.Empty;
            }

            var callbackUrl = await _context.ClientSignOns
                .Where(c => c.ClientID == clientId)
                .Select(c => c.ClientWebHook)
                .FirstOrDefaultAsync();

            if (callbackUrl == null)
            {
                return string.Empty;
            }

            return callbackUrl.ToString();
        }

        /// <summary>
        /// ดึงข้อมูล callback ที่รอการส่ง
        /// </summary>
        /// <param name="count">จำนวนที่ต้องการดึง</param>
        /// <returns>รายการข้อมูล callback ที่รอการส่ง</returns>
        public async Task<PendingCallback[]> GetPendingCallbacksForProcessingAsync(int count)
        {
            var pendingCallbacks = await _context.CallbackQueue
                .Join(_context.OrderRefunds,
                c => c.RefundID,
                r => r.RefundID,
                (c, r) => new { Callback = c, Refund = r })
                .Where(x => !x.Callback.IsProcessed && x.Callback.ScheduledTime <= _dateTime.Now)
                .OrderBy(x => x.Callback.ScheduledTime)
                .Take(count)
                .Select(x => new PendingCallback
                {
                    CallbackID = x.Callback.CallbackID,
                    Status = x.Callback.Status,
                    StatusMessage = x.Callback.StatusMessage,
                    TerminalID = x.Refund.TransactionID,
                    TransactionID = x.Refund.TransactionID,
                    TransactionDate = x.Refund.TransactionDate,
                    PaymentType = x.Refund.PaymentType,
                    RefundAmount = x.Refund.RefundAmount,
                    RequestID = x.Refund.RequestID,
                    RetryCount = x.Callback.RetryCount
                })
                .ToArrayAsync();

            return pendingCallbacks;
        }

        /// <summary>
        /// ดึงข้อมูลการคืนเงินที่อยู่ระหว่างดำเนินการ
        /// </summary>
        /// <returns>รายการการคืนเงินที่อยู่ระหว่างดำเนินการ</returns>
        public async Task<IReadOnlyList<rms_OrderRefund>> GetPendingRefundsAsync()
        {
            return await _context.OrderRefunds
                                .Where(r => r.TxnStatus == RefundConstants.TransactionStatus.Pending)
                                .ToListAsync();
        }

        /// <summary>
        /// ดึงข้อมูลการคืนเงินตาม Terminal และ Transaction ID
        /// </summary>
        /// <param name="terminalId">รหัสเทอร์มินัล</param>
        /// <param name="transactionId">รหัสธุรกรรม</param>
        /// <returns>ข้อมูลการคืนเงิน หรือ null ถ้าไม่พบ</returns>
        public async Task<rms_OrderRefund> GetRefundByTerminalAndTransactionIdAsync(string terminalId, string transactionId)
        {
            return await _context.OrderRefunds
                                .Where(r => r.TerminalID == terminalId && r.TransactionID == transactionId)
                                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// ดึงข้อมูลการคืนเงินตามช่วงวันที่และสถานะ
        /// </summary>
        /// <param name="startDate">วันที่เริ่มต้น</param>
        /// <param name="endDate">วันที่สิ้นสุด</param>
        /// <param name="status">สถานะที่ต้องการค้นหา</param>
        /// <returns>รายการการคืนเงินในช่วงวันที่และสถานะที่กำหนด</returns>
        public async Task<IReadOnlyList<rms_OrderRefund>> GetRefundsByDateRangeAndStatusAsync(DateTime startDate, DateTime endDate, byte status)
        {
            // ปรับเวลาสิ้นสุดให้รวมถึงสิ้นวัน 2025-04-18 23:59:59.9999999
            var adjustedEndDate = endDate.Date.AddDays(1).AddTicks(-1);

            return await _context.OrderRefunds
                .Where(r => r.TxnStatus == status && r.TransactionDate >= startDate && r.TransactionDate <= adjustedEndDate)
                .ToListAsync();
        }

        /// <summary>
        /// ดึงข้อมูลการคืนเงินตามช่วงวันที่
        /// </summary>
        /// <param name="startDate">วันที่เริ่มต้น</param>
        /// <param name="endDate">วันที่สิ้นสุด</param>
        /// <returns>รายการการคืนเงินในช่วงวันที่ที่กำหนด</returns>
        public async Task<IReadOnlyList<rms_OrderRefund>> GetRefundsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            // ปรับเวลาสิ้นสุดให้รวมถึงสิ้นวัน 2025-04-18 23:59:59.9999999
            var adjustedEndDate = endDate.Date.AddDays(1).AddTicks(-1);

            return await _context.OrderRefunds
                .Where(r => r.TransactionDate >= startDate && r.TransactionDate <= adjustedEndDate)
                .ToListAsync();
        }

        /// <summary>
        /// ดึงข้อมูลสถานะการคืนเงิน
        /// </summary>
        /// <param name="status">รหัสสถานะ</param>
        /// <returns>ข้อมูลสถานะการคืนเงิน หรือ null ถ้าไม่พบ</returns>
        public async Task<rms_OrderRefundStatus> GetRefundStatusAsync(byte status)
        {
            return await _context.OrderRefundStatuses
                .FirstOrDefaultAsync(s => s.TxnStatus == status);
        }

        /// <summary>
        /// บันทึกข้อมูล callback เข้าคิว
        /// </summary>
        /// <param name="refundId">ID ของรายการคืนเงิน</param>
        /// <param name="terminalId">Terminal ID</param>
        /// <param name="transactionDate">วันที่ทำรายการ</param>
        /// <param name="transactionId">Transaction ID</param>
        /// <param name="refundAmount">จำนวนเงินที่คืน</param>
        /// <param name="requestId">Request ID</param>
        /// <param name="status">สถานะของ callback</param>
        /// <param name="statusMessage">ข้อความสถานะ</param>
        /// <param name="paymentType">ประเภทการชำระเงิน</param>
        /// <param name="retryCount">จำนวนครั้งที่ลองใหม่</param>
        /// <param name="scheduledTime">เวลาที่กำหนดให้ส่ง</param>
        /// <returns>ID ของ callback ที่บันทึก</returns>
        public async Task<int> SaveCallbackToQueueAsync(int refundId, string terminalId, DateTime transactionDate, string transactionId, decimal refundAmount, string requestId, string status, string statusMessage, string paymentType, int retryCount, DateTime scheduledTime)
        {
            var callbackQueue = rms_CallbackQueue.Create(
                refundId,
                status,
                statusMessage,
                retryCount,
                scheduledTime
                );

           _context.CallbackQueue.Add( callbackQueue );
            await _context.SaveChangesAsync();

            return callbackQueue.CallbackID;
        }

        /// <summary>
        /// อัพเดตสถานะการประมวลผล callback
        /// </summary>
        /// <param name="callbackId">ID ของ callback</param>
        /// <param name="isSuccess">สถานะความสำเร็จ</param>
        /// <param name="retryCount">จำนวนครั้งที่ลองใหม่</param>
        /// <returns>true ถ้าอัพเดตสำเร็จ, false ถ้าไม่สำเร็จ</returns>
        public async Task<bool> UpdateCallbackProcessingStatusAsync(int callbackId, bool isSuccess, int retryCount)
        {
            var callback = await _context.CallbackQueue
                .FirstOrDefaultAsync(c => c.CallbackID == callbackId);
            if (callback == null)
            {
                return false;
            }

            callback.UpdateProcessingStatus(isSuccess);

            if(!isSuccess && retryCount < RefundConstants.MaxRetryCount)
            {
                // กำหนดเวลาสำหรับการลองใหม่ (เช่น เพิ่มขึ้นตามจำนวนครั้งที่ลองแล้ว)
                var nextScheduledTime = _dateTime.Now.AddMinutes(Math.Pow(2, retryCount));
                callback.IncrementRetryCount(nextScheduledTime);
            }

            var result = await _context.SaveChangesAsync();

            return result > 0;
        }

        // <summary>
        /// อัพเดตสถานะของ callback
        /// </summary>
        /// <param name="callbackId">ID ของ callback</param>
        /// <param name="isSuccess">สถานะความสำเร็จ</param>
        /// <param name="retryCount">จำนวนครั้งที่ลองใหม่</param>
        /// <returns>true ถ้าอัพเดตสำเร็จ, false ถ้าไม่สำเร็จ</returns>
        public async Task<bool> UpdateCallbackStatusAsync(int callbackId, bool isSuccess, int retryCount)
        {
            var callback = await _context.CallbackQueue
                .FirstOrDefaultAsync(c => c.CallbackID == callbackId);
            if (callback == null)
            {
                return false;
            }

            // ถ้าสำเร็จ ให้ตั้งค่าสถานะเป็น "ประมวลผลแล้ว"
            if (isSuccess)
            {
                callback.IsProcessed = true;
                callback.ProcessedTime = _dateTime.Now;
            }
            else
            {
                // ถ้าไม่สำเร็จและยังไม่เกินจำนวนครั้งที่ลองได้ ให้เพิ่มจำนวนครั้ง
                if (retryCount < RefundConstants.MaxRetryCount)
                {
                    callback.RetryCount = retryCount;
                    // กำหนดเวลาสำหรับการลองใหม่ (เช่น เพิ่มขึ้นตามจำนวนครั้งที่ลองแล้ว)
                    callback.ScheduledTime = _dateTime.Now.AddMinutes(Math.Pow(2, retryCount));
                }
                else
                {
                    callback.IsProcessed = true;
                    callback.ProcessedTime = _dateTime.Now;
                }
            }

            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        /// <summary>
        /// อัพเดตสถานะการคืนเงิน
        /// </summary>
        /// <param name="refundId">ID ของรายการคืนเงิน</param>
        /// <param name="status">สถานะใหม่</param>
        /// <param name="modifiedBy">ID ของผู้ทำรายการ</param>
        /// <returns>true ถ้าอัพเดตสำเร็จ, false ถ้าไม่สำเร็จ</returns>
        public async Task<bool> UpdateRefundStatusAsync(int refundId, byte status, int modifiedBy)
        {
            var refund = await _context.OrderRefunds.FindAsync(refundId);
            if (refund == null)
            {
                return false;
            }

            // ตรวจสอบว่าสถานะใหม่มีอยู่ในระบบหรือไม่
            var refundStatus = await _context.OrderRefundStatuses.FindAsync(status);
            if (refundStatus == null)
            {
                return false;
            }

            // อัพเดตสถานะ
            refund.UpdateStatus(status, modifiedBy);

            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}
