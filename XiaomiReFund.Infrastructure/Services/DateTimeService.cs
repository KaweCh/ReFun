using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XiaomiReFund.Application.Common.Interfaces;

namespace XiaomiReFund.Infrastructure.Services
{
    /// <summary>
    /// บริการวันเวลา
    /// </summary>
    public class DateTimeService : IDateTime
    {
        /// <inheritdoc/>
        public DateTime Now => DateTime.Now;

        /// <inheritdoc/>
        public DateTime UtcNow => DateTime.UtcNow;

        /// <inheritdoc/>
        public DateTimeOffset NowOffset => DateTimeOffset.Now;

        /// <inheritdoc/>
        public DateTimeOffset UtcNowOffset => DateTimeOffset.UtcNow;
    }
}
