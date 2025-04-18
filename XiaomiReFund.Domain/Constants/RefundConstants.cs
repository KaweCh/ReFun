using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaomiReFund.Domain.Constants
{
    /// <summary>
    /// ค่าคงที่สำหรับการคืนเงิน
    /// </summary>
    public static class RefundConstants
    {
        // ค่าคงที่สำหรับ API Response
        public static class ApiStatusCode
        {
            public const int Success = 200;
            public const int BadRequest = 400;
            public const int Unauthorized = 401;
            public const int NotFound = 404;
            public const int Conflict = 409;
        }

        // ค่าคงที่สำหรับข้อความ response
        public static class ApiStatusMessage
        {
            public const string Success = "Accepted";
            public const string BadRequest = "Bad Request";
            public const string Unauthorized = "Unauthorized";
            public const string TokenExpired = "Token Expired";
            public const string Existed = "Already Requested";
        }

        // ค่าคงที่สำหรับข้อความ response เพิ่มเติม
        public static class ApiResponseMessage
        {
            public const string RequestAccepted = "Your request has been accepted";
            public const string InvalidJson = "JSON format is invalid";
            public const string InvalidToken = "Invalid Token or Token Expired";
            public const string AlreadyRequested = "Already Requested";
        }

        // ค่าคงที่สำหรับสถานะ Callback
        public static class CallbackStatus
        {
            public const string Approved = "Approved";
            public const string Rejected = "Rejected";
            public const string Processing = "Processing";
        }

        // ค่าคงที่ Transaction Status
        public static class TransactionStatus
        {
            // สถานะต่างๆ ที่ใช้ใน API
            // ค่าเหล่านี้มาจากฐานข้อมูล rms_OrderRefundStatus
            public const byte Pending = 0;
            public const byte Processing = 1;
            public const byte Approved = 2;
            public const byte Rejected = 3;
            public const byte Failed = 4;
        }

        // ค่าคงที่อื่นๆ
        public const int MaxRetryCount = 5;
        public const int CallbackTimeoutSeconds = 30;
        public const int TokenExpirationSeconds = 14400; // 4 hours
    }
}
