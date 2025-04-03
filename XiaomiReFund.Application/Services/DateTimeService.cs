using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XiaomiReFund.Application.Common.Interfaces;

namespace XiaomiReFund.Application.Services
{
    /// <summary>
    /// บริการวันเวลา
    /// </summary>
    public class DateTimeService : IDateTime
    {
        /// <summary>
        /// รับเวลาปัจจุบันในโซนเวลาเครื่อง
        /// </summary>
        public DateTime Now => DateTime.Now;

        /// <summary>
        /// รับเวลาปัจจุบันในโซนเวลา UTC
        /// </summary>
        public DateTime UtcNow => DateTime.UtcNow;

        /// <summary>
        /// รับเวลาปัจจุบันในโซนเวลาเครื่องพร้อมข้อมูลโซนเวลา
        /// </summary>
        public DateTimeOffset NowOffset => DateTimeOffset.Now;

        /// <summary>
        /// รับเวลาปัจจุบันในโซนเวลา UTC พร้อมข้อมูลโซนเวลา
        /// </summary>
        public DateTimeOffset UtcNowOffset => DateTimeOffset.UtcNow;
    }
}
