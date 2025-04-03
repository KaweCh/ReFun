using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using XiaomiReFund.Application.Common.Interfaces;
using XiaomiReFund.Application.Interfaces.External;

namespace XiaomiReFund.Application.Services
{
    /// <summary>
    /// บริการ HTTP client
    /// </summary>
    public class HttpClientService : IHttpClientService
    {
        private readonly HttpClient _httpClient;
        private readonly ILoggerService _logger;

        /// <summary>
        /// สร้าง HttpClientService ใหม่
        /// </summary>
        /// <param name="logger">บริการบันทึกข้อมูล</param>
        public HttpClientService(ILoggerService logger)
        {
            _httpClient = new HttpClient();
            _logger = logger;
        }

        /// <summary>
        /// ส่งคำขอ GET
        /// </summary>
        /// <typeparam name="T">ประเภทข้อมูลที่คาดหวังจะได้รับ</typeparam>
        /// <param name="uri">URI ปลายทาง</param>
        /// <param name="headers">ส่วนหัวของคำขอ (ถ้ามี)</param>
        /// <returns>ข้อมูลที่ได้รับ</returns>
        public async Task<T> GetAsync<T>(string uri, Dictionary<string, string> headers = null)
        {
            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Get, uri);

                // เพิ่มส่วนหัว
                AddHeaders(request, headers);

                // ส่งคำขอ
                var response = await _httpClient.SendAsync(request);

                // ตรวจสอบสถานะ
                response.EnsureSuccessStatusCode();

                // อ่านเนื้อหา
                var content = await response.Content.ReadAsStringAsync();

                // แปลงข้อมูล
                return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (Exception ex)
            {
                _logger.LogError("HttpClientGet", ex, 0);
                throw;
            }
        }

        /// <summary>
        /// ส่งคำขอ POST
        /// </summary>
        /// <typeparam name="TRequest">ประเภทข้อมูลที่ส่ง</typeparam>
        /// <typeparam name="TResponse">ประเภทข้อมูลที่คาดหวังจะได้รับ</typeparam>
        /// <param name="uri">URI ปลายทาง</param>
        /// <param name="data">ข้อมูลที่ส่ง</param>
        /// <param name="headers">ส่วนหัวของคำขอ (ถ้ามี)</param>
        /// <returns>ข้อมูลที่ได้รับ</returns>
        public async Task<TResponse> PostAsync<TRequest, TResponse>(string uri, TRequest data, Dictionary<string, string> headers = null)
        {
            try
            {
                // แปลงข้อมูลเป็น JSON
                var content = new StringContent(
                    JsonSerializer.Serialize(data),
                    Encoding.UTF8,
                    "application/json");

                using var request = new HttpRequestMessage(HttpMethod.Post, uri)
                {
                    Content = content
                };

                // เพิ่มส่วนหัว
                AddHeaders(request, headers);

                // ส่งคำขอ
                var response = await _httpClient.SendAsync(request);

                // ตรวจสอบสถานะ
                response.EnsureSuccessStatusCode();

                // อ่านเนื้อหา
                var responseContent = await response.Content.ReadAsStringAsync();

                // แปลงข้อมูล
                return JsonSerializer.Deserialize<TResponse>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (Exception ex)
            {
                _logger.LogError("HttpClientPost", ex, 0);
                throw;
            }
        }

        /// <summary>
        /// ส่งคำขอ PUT
        /// </summary>
        /// <typeparam name="TRequest">ประเภทข้อมูลที่ส่ง</typeparam>
        /// <typeparam name="TResponse">ประเภทข้อมูลที่คาดหวังจะได้รับ</typeparam>
        /// <param name="uri">URI ปลายทาง</param>
        /// <param name="data">ข้อมูลที่ส่ง</param>
        /// <param name="headers">ส่วนหัวของคำขอ (ถ้ามี)</param>
        /// <returns>ข้อมูลที่ได้รับ</returns>
        public async Task<TResponse> PutAsync<TRequest, TResponse>(string uri, TRequest data, Dictionary<string, string> headers = null)
        {
            try
            {
                // แปลงข้อมูลเป็น JSON
                var content = new StringContent(
                    JsonSerializer.Serialize(data),
                    Encoding.UTF8,
                    "application/json");

                using var request = new HttpRequestMessage(HttpMethod.Put, uri)
                {
                    Content = content
                };

                // เพิ่มส่วนหัว
                AddHeaders(request, headers);

                // ส่งคำขอ
                var response = await _httpClient.SendAsync(request);

                // ตรวจสอบสถานะ
                response.EnsureSuccessStatusCode();

                // อ่านเนื้อหา
                var responseContent = await response.Content.ReadAsStringAsync();

                // แปลงข้อมูล
                return JsonSerializer.Deserialize<TResponse>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (Exception ex)
            {
                _logger.LogError("HttpClientPut", ex, 0);
                throw;
            }
        }

        /// <summary>
        /// ส่งคำขอ DELETE
        /// </summary>
        /// <typeparam name="T">ประเภทข้อมูลที่คาดหวังจะได้รับ</typeparam>
        /// <param name="uri">URI ปลายทาง</param>
        /// <param name="headers">ส่วนหัวของคำขอ (ถ้ามี)</param>
        /// <returns>ข้อมูลที่ได้รับ</returns>
        public async Task<T> DeleteAsync<T>(string uri, Dictionary<string, string> headers = null)
        {
            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Delete, uri);

                // เพิ่มส่วนหัว
                AddHeaders(request, headers);

                // ส่งคำขอ
                var response = await _httpClient.SendAsync(request);

                // ตรวจสอบสถานะ
                response.EnsureSuccessStatusCode();

                // อ่านเนื้อหา
                var content = await response.Content.ReadAsStringAsync();

                // แปลงข้อมูล
                return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (Exception ex)
            {
                _logger.LogError("HttpClientDelete", ex, 0);
                throw;
            }
        }

        /// <summary>
        /// ส่งคำขอแบบกำหนดเอง
        /// </summary>
        /// <typeparam name="TResponse">ประเภทข้อมูลที่คาดหวังจะได้รับ</typeparam>
        /// <param name="request">คำขอ HTTP</param>
        /// <returns>ข้อมูลที่ได้รับ</returns>
        public async Task<TResponse> SendAsync<TResponse>(HttpRequestMessage request)
        {
            try
            {
                // ส่งคำขอ
                var response = await _httpClient.SendAsync(request);

                // ตรวจสอบสถานะ
                response.EnsureSuccessStatusCode();

                // อ่านเนื้อหา
                var content = await response.Content.ReadAsStringAsync();

                // แปลงข้อมูล
                return JsonSerializer.Deserialize<TResponse>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (Exception ex)
            {
                _logger.LogError("HttpClientSend", ex, 0);
                throw;
            }
        }

        /// <summary>
        /// ตั้งค่า base address
        /// </summary>
        /// <param name="baseAddress">Base address สำหรับคำขอทั้งหมด</param>
        public void SetBaseAddress(string baseAddress)
        {
            _httpClient.BaseAddress = new Uri(baseAddress);
        }

        /// <summary>
        /// ตั้งค่า timeout
        /// </summary>
        /// <param name="timeout">ระยะเวลา timeout</param>
        public void SetTimeout(TimeSpan timeout)
        {
            _httpClient.Timeout = timeout;
        }

        /// <summary>
        /// เพิ่มส่วนหัวให้กับคำขอ
        /// </summary>
        /// <param name="request">คำขอ HTTP</param>
        /// <param name="headers">ส่วนหัวที่ต้องการเพิ่ม</param>
        private void AddHeaders(HttpRequestMessage request, Dictionary<string, string> headers)
        {
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    if (header.Key.Equals("Content-Type", StringComparison.OrdinalIgnoreCase))
                    {
                        // Content-Type ควรถูกตั้งค่าที่ content โดยตรง
                        continue;
                    }

                    request.Headers.Add(header.Key, header.Value);
                }
            }
        }
    }
}
