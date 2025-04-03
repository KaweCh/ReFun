using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaomiReFund.Application.Common.Interfaces
{
    // Interface for abstracting DateTime operations
    public interface IDateTime
    {
        // Current local date and time
        DateTime Now { get; }

        // Current UTC date and time
        DateTime UtcNow { get; }

        // Current local date and time with timezone offset
        DateTimeOffset NowOffset { get; }

        // Current UTC date and time with timezone offset
        DateTimeOffset UtcNowOffset { get; }
    }
}

// Example implementation:
// public class SystemDateTime : IDateTime
// {
//     public DateTime Now => DateTime.Now;
//     public DateTime UtcNow => DateTime.UtcNow;
//     public DateTimeOffset NowOffset => DateTimeOffset.Now;
//     public DateTimeOffset UtcNowOffset => DateTimeOffset.UtcNow;
// }