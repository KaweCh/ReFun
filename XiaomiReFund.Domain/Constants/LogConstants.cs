using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaomiReFund.Domain.Constants
{
    /// <summary>
    /// ค่าคงที่สำหรับการบันทึกข้อมูล
    /// </summary>
    public static class LogConstants
    {
        // ประเภทของการบันทึกข้อมูล
        public static class LogType
        {
            public const string Request = "request";
            public const string Response = "response";
            public const string Error = "error";
            public const string Callback = "callback";
            public const string CallbackResponse = "callback_response";
            public const string CallbackRetry = "callback_retry";
            public const string System = "system";
            public const string Debug = "debug";
        }

        // ระดับความสำคัญของการบันทึกข้อมูล
        public static class LogLevel
        {
            public const string Debug = "DEBUG";
            public const string Info = "INFO";
            public const string Warning = "WARNING";
            public const string Error = "ERROR";
            public const string Critical = "CRITICAL";
        }

        // รูปแบบการบันทึกข้อมูล
        public static class LogFormat
        {
            public const string DateTimeFormat = "yyyy-MM-dd HH:mm:ss.fff";
            public const string LogFileDateFormat = "yyyyMMdd";
            public const string LogFilePattern = "{0}_{1}.log"; // {0} = log type, {1} = date
        }
    }
}
