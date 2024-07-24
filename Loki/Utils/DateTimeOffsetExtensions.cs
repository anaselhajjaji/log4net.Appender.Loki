using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace log4net.Appender.Loki.Loki.Utils
{
    internal static class DateTimeOffsetExtensions
    {
        private const long NanosecondsPerTick = 100;
        private static readonly DateTimeOffset s_unixEpoch = new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero);

        internal static string ToUnixNanosecondsString(this DateTimeOffset offset) =>
            ((offset - s_unixEpoch).Ticks * NanosecondsPerTick).ToString();
    }
}
