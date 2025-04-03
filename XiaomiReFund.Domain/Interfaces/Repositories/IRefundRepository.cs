using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XiaomiReFund.Domain.Entities;

namespace XiaomiReFund.Domain.Interfaces.Repositories
{
    /// <summary>
    /// อินเทอร์เฟซสำหรับการเข้าถึงข้อมูลการคืนเงิน
    /// </summary>
    public interface IRefundRepository : IBaseRepository<rms_OrderRefund>
    {
        /// <summary>
        /// ค้นหาข้อมูลการคืนเงินตาม Terminal ID และ Request ID
        /// </summary>
        /// <param name="terminalId">Terminal ID</param>
        /// <param name="requestId">Request ID</param>
        /// <returns>ข้อมูลการคืนเงิน หรือ null ถ้าไม่พบ</returns>
        Task<rms_OrderRefund> GetByTerminalAndRequestIdAsync(string terminalId, string requestId);

        /// <summary>
        /// ค้นหาข้อมูลการคืนเงินตาม Terminal ID และ Transaction ID
        /// </summary>
        /// <param name="terminalId">Terminal ID</param>
        /// <param name="transactionId">Transaction ID</param>
        /// <returns>ข้อมูลการคืนเงิน หรือ null ถ้าไม่พบ</returns>
        Task<rms_OrderRefund> GetRefundByTerminalAndTransactionIdAsync(string terminalId, string transactionId);

        /// <summary>
        /// สร้างข้อมูลการคืนเงินใหม่
        /// </summary>
        /// <param name="terminalId">Terminal ID</param>
        /// <param name="transactionDate">วันที่ทำรายการ</param>
        /// <param name="transactionId">Transaction ID</param>
        /// <param name="paymentType">Payment Type</param>
        /// <param name="refundAmount">จำนวนเงินที่คืน</param>
        /// <param name="requestId">Request ID</param>
        /// <param name="clientId">ID ของลูกค้า</param>
        /// <param name="modifiedBy">ID ของผู้ทำรายการ</param>
        /// <returns>ID ของรายการคืนเงินที่สร้างขึ้น</returns>
        Task<int> CreateRefundAsync(
            string terminalId,
            DateTime transactionDate,
            string transactionId,
            string paymentType,
            decimal refundAmount,
            string requestId,
            int clientId,
            int modifiedBy);

        /// <summary>
        /// อัพเดตสถานะการคืนเงิน
        /// </summary>
        /// <param name="refundId">ID ของรายการคืนเงิน</param>
        /// <param name="status">สถานะใหม่</param>
        /// <param name="modifiedBy">ID ของผู้ทำรายการ</param>
        /// <returns>true ถ้าอัพเดตสำเร็จ, false ถ้าไม่สำเร็จ</returns>
        Task<bool> UpdateRefundStatusAsync(int refundId, byte status, int modifiedBy);

        /// <summary>
        /// ค้นหาข้อมูลการคืนเงินที่อยู่ระหว่างดำเนินการ
        /// </summary>
        /// <returns>รายการการคืนเงินที่อยู่ระหว่างดำเนินการ</returns>
        Task<IReadOnlyList<rms_OrderRefund>> GetPendingRefundsAsync();

        /// <summary>
        /// ค้นหาข้อมูลการคืนเงินตามช่วงวันที่
        /// </summary>
        /// <param name="startDate">วันที่เริ่มต้น</param>
        /// <param name="endDate">วันที่สิ้นสุด</param>
        /// <returns>รายการการคืนเงินในช่วงวันที่ที่กำหนด</returns>
        Task<IReadOnlyList<rms_OrderRefund>> GetRefundsByDateRangeAsync(DateTime startDate, DateTime endDate);

        /// <summary>
        /// ค้นหาข้อมูลการคืนเงินตามช่วงวันที่และสถานะ
        /// </summary>
        /// <param name="startDate">วันที่เริ่มต้น</param>
        /// <param name="endDate">วันที่สิ้นสุด</param>
        /// <param name="status">สถานะที่ต้องการค้นหา</param>
        /// <returns>รายการการคืนเงินในช่วงวันที่และสถานะที่กำหนด</returns>
        Task<IReadOnlyList<rms_OrderRefund>> GetRefundsByDateRangeAndStatusAsync(
            DateTime startDate,
            DateTime endDate,
            byte status);

        /// <summary>
        /// ค้นหาข้อมูลสถานะการคืนเงิน
        /// </summary>
        /// <param name="status">รหัสสถานะ</param>
        /// <returns>ข้อมูลสถานะการคืนเงิน</returns>
        Task<rms_OrderRefundStatus> GetRefundStatusAsync(byte status);

        /// <summary>
        /// ดึงข้อมูลสถานะการคืนเงินทั้งหมด
        /// </summary>
        /// <returns>รายการสถานะการคืนเงินทั้งหมด</returns>
        Task<IReadOnlyList<rms_OrderRefundStatus>> GetAllRefundStatusesAsync();

        /// <summary>
        /// ดึง URL ปลายทางของ callback จากเทอร์มินัล
        /// </summary>
        /// <param name="terminalId">Terminal ID</param>
        /// <returns>URL ปลายทางของ callback</returns>
        Task<string> GetCallbackUrlAsync(string terminalId);

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
        Task<int> SaveCallbackToQueueAsync(
            int refundId,
            string terminalId,
            DateTime transactionDate,
            string transactionId,
            decimal refundAmount,
            string requestId,
            string status,
            string statusMessage,
            string paymentType,
            int retryCount,
            DateTime scheduledTime);

        /// <summary>
        /// ดึงข้อมูล callback ที่รอการส่ง
        /// </summary>
        /// <param name="count">จำนวนที่ต้องการดึง</param>
        /// <returns>รายการข้อมูล callback ที่รอการส่ง</returns>
        Task<SendCallbackRequest[]> GetPendingCallbacksForProcessingAsync(int count);

        /// <summary>
        /// อัพเดตสถานะของ callback
        /// </summary>
        /// <param name="callbackId">ID ของ callback</param>
        /// <param name="isSuccess">สถานะความสำเร็จ</param>
        /// <param name="retryCount">จำนวนครั้งที่ลองใหม่</param>
        /// <returns>true ถ้าอัพเดตสำเร็จ, false ถ้าไม่สำเร็จ</returns>
        Task<bool> UpdateCallbackStatusAsync(int callbackId, bool isSuccess, int retryCount);

        /// <summary>
        /// อัพเดตสถานะการประมวลผล callback
        /// </summary>
        /// <param name="callbackId">ID ของ callback</param>
        /// <param name="isSuccess">สถานะความสำเร็จ</param>
        /// <param name="retryCount">จำนวนครั้งที่ลองใหม่</param>
        /// <returns>true ถ้าอัพเดตสำเร็จ, false ถ้าไม่สำเร็จ</returns>
        Task<bool> UpdateCallbackProcessingStatusAsync(int callbackId, bool isSuccess, int retryCount);
    }
}
