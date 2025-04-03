using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaomiReFund.Application.Interfaces.External
{
    /// <summary>
    /// อินเทอร์เฟซสำหรับบริการ HTTP client
    /// </summary>
    public interface IHttpClientService
    {
        /// <summary>
        /// ส่งคำขอ GET
        /// </summary>
        /// <typeparam name="T">ประเภทข้อมูลที่คาดหวังจะได้รับ</typeparam>
        /// <param name="uri">URI ปลายทาง</param>
        /// <param name="headers">ส่วนหัวของคำขอ (ถ้ามี)</param>
        /// <returns>ข้อมูลที่ได้รับ</returns>
        Task<T> GetAsync<T>(string uri, Dictionary<string, string> headers = null);

        /// <summary>
        /// ส่งคำขอ POST
        /// </summary>
        /// <typeparam name="TRequest">ประเภทข้อมูลที่ส่ง</typeparam>
        /// <typeparam name="TResponse">ประเภทข้อมูลที่คาดหวังจะได้รับ</typeparam>
        /// <param name="uri">URI ปลายทาง</param>
        /// <param name="data">ข้อมูลที่ส่ง</param>
        /// <param name="headers">ส่วนหัวของคำขอ (ถ้ามี)</param>
        /// <returns>ข้อมูลที่ได้รับ</returns>
        Task<TResponse> PostAsync<TRequest, TResponse>(string uri, TRequest data, Dictionary<string, string> headers = null);

        /// <summary>
        /// ส่งคำขอ PUT
        /// </summary>
        /// <typeparam name="TRequest">ประเภทข้อมูลที่ส่ง</typeparam>
        /// <typeparam name="TResponse">ประเภทข้อมูลที่คาดหวังจะได้รับ</typeparam>
        /// <param name="uri">URI ปลายทาง</param>
        /// <param name="data">ข้อมูลที่ส่ง</param>
        /// <param name="headers">ส่วนหัวของคำขอ (ถ้ามี)</param>
        /// <returns>ข้อมูลที่ได้รับ</returns>
        Task<TResponse> PutAsync<TRequest, TResponse>(string uri, TRequest data, Dictionary<string, string> headers = null);

        /// <summary>
        /// ส่งคำขอ DELETE
        /// </summary>
        /// <typeparam name="T">ประเภทข้อมูลที่คาดหวังจะได้รับ</typeparam>
        /// <param name="uri">URI ปลายทาง</param>
        /// <param name="headers">ส่วนหัวของคำขอ (ถ้ามี)</param>
        /// <returns>ข้อมูลที่ได้รับ</returns>
        Task<T> DeleteAsync<T>(string uri, Dictionary<string, string> headers = null);

        /// <summary>
        /// ส่งคำขอแบบกำหนดเอง
        /// </summary>
        /// <typeparam name="TResponse">ประเภทข้อมูลที่คาดหวังจะได้รับ</typeparam>
        /// <param name="request">คำขอ HTTP</param>
        /// <returns>ข้อมูลที่ได้รับ</returns>
        Task<TResponse> SendAsync<TResponse>(HttpRequestMessage request);

        /// <summary>
        /// ตั้งค่า base address
        /// </summary>
        /// <param name="baseAddress">Base address สำหรับคำขอทั้งหมด</param>
        void SetBaseAddress(string baseAddress);

        /// <summary>
        /// ตั้งค่า timeout
        /// </summary>
        /// <param name="timeout">ระยะเวลา timeout</param>
        void SetTimeout(TimeSpan timeout);
    }
}
