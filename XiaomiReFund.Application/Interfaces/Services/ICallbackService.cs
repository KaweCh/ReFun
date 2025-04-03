using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XiaomiReFund.Application.DTOs.Callback;

namespace XiaomiReFund.Application.Interfaces.Services
{
    /// <summary>
    /// อินเทอร์เฟซสำหรับบริการ callback
    /// </summary>
    public interface ICallbackService
    {
        /// <summary>
        /// ส่ง callback ไปยังปลายทาง
        /// </summary>
        /// <param name="request">ข้อมูลคำขอส่ง callback</param>
        /// <returns>ผลลัพธ์การส่ง callback</returns>
        Task<CallbackResponse> SendCallbackAsync(SendCallbackRequest request);

        /// <summary>
        /// เพิ่ม callback เข้าคิวสำหรับส่งในภายหลัง
        /// </summary>
        /// <param name="request">ข้อมูลคำขอเพิ่ม callback เข้าคิว</param>
        /// <returns>ผลลัพธ์การเพิ่ม callback เข้าคิว</returns>
        Task<EnqueueCallbackResponse> EnqueueCallbackAsync(EnqueueCallbackRequest request);

        /// <summary>
        /// ประมวลผล callback ที่อยู่ในคิว
        /// </summary>
        /// <returns>จำนวน callback ที่ประมวลผลสำเร็จ</returns>
        Task<int> ProcessCallbackQueueAsync();

        /// <summary>
        /// ดึงข้อมูล callback ที่ยังไม่ได้ส่ง
        /// </summary>
        /// <param name="count">จำนวนที่ต้องการดึง</param>
        /// <returns>คำขอส่ง callback ที่ยังไม่ได้ส่ง</returns>
        Task<SendCallbackRequest[]> GetPendingCallbacksAsync(int count = 10);

        /// <summary>
        /// อัปเดตสถานะของ callback
        /// </summary>
        /// <param name="callbackId">รหัส callback</param>
        /// <param name="isSuccess">สถานะความสำเร็จ</param>
        /// <param name="retryCount">จำนวนครั้งที่ลองใหม่</param>
        /// <returns>ผลลัพธ์การอัปเดตสถานะ</returns>
        Task<bool> UpdateCallbackStatusAsync(int callbackId, bool isSuccess, int retryCount);
    }
}
