using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace XiaomiReFund.Application.Common.Models
{
    /// <summary>
    /// แบบจำลองสำหรับการตอบกลับ API ภายในแอปพลิเคชัน
    /// </summary>
    /// <typeparam name="T">ประเภทข้อมูลที่จะส่งกลับ</typeparam>
    public class ApiResponse<T>
    {
        /// <summary>
        /// ข้อมูลผลลัพธ์ของการตอบกลับ
        /// </summary>
        public ResultData Result { get; set; }

        /// <summary>
        /// ข้อมูลที่ส่งกลับ
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// สร้าง ApiResponse ใหม่
        /// </summary>
        public ApiResponse()
        {
            Result = new ResultData();
        }

        /// <summary>
        /// สร้าง ApiResponse ด้วยสถานะและข้อความ
        /// </summary>
        public ApiResponse(int statusCode, string status, string message)
        {
            Result = new ResultData
            {
                StatusCode = statusCode,
                Status = status,
                Msg = message
            };
        }

        /// <summary>
        /// สร้าง ApiResponse ด้วยสถานะ ข้อความ และข้อมูล
        /// </summary>
        public ApiResponse(int statusCode, string status, string message, T data)
            : this(statusCode, status, message)
        {
            Data = data;
        }

        /// <summary>
        /// สร้าง ApiResponse สำหรับการตอบกลับสำเร็จ
        /// </summary>
        public static ApiResponse<T> Success(string message = "Success", T data = default)
        {
            return new ApiResponse<T>(200, "Success", message, data);
        }

        /// <summary>
        /// สร้าง ApiResponse สำหรับการตอบกลับการยอมรับ
        /// </summary>
        public static ApiResponse<T> Accepted(string message = "Your request has been accepted", T data = default)
        {
            return new ApiResponse<T>(200, "Accepted", message, data);
        }

        /// <summary>
        /// สร้าง ApiResponse สำหรับการตอบกลับคำขอที่ไม่ถูกต้อง
        /// </summary>
        public static ApiResponse<T> BadRequest(string message = "JSON format is invalid")
        {
            return new ApiResponse<T>(400, "Bad Request", message);
        }

        /// <summary>
        /// สร้าง ApiResponse สำหรับการตอบกลับการไม่ได้รับอนุญาต
        /// </summary>
        public static ApiResponse<T> Unauthorized(string message = "Invalid Token or Token Expired")
        {
            return new ApiResponse<T>(401, "Unauthorized", message);
        }

        /// <summary>
        /// สร้าง ApiResponse สำหรับการตอบกลับไม่พบข้อมูล
        /// </summary>
        public static ApiResponse<T> NotFound(string message = "Resource not found")
        {
            return new ApiResponse<T>(404, "Not Found", message);
        }

        /// <summary>
        /// สร้าง ApiResponse สำหรับการตอบกลับมีข้อมูลซ้ำ
        /// </summary>
        public static ApiResponse<T> Conflict(string message = "Already Requested")
        {
            return new ApiResponse<T>(409, "Existed", message);
        }

        /// <summary>
        /// สร้าง ApiResponse สำหรับการตอบกลับเกิดข้อผิดพลาดภายในเซิร์ฟเวอร์
        /// </summary>
        public static ApiResponse<T> InternalServerError(string message = "Internal server error")
        {
            return new ApiResponse<T>(500, "Internal Server Error", message);
        }
    }
}