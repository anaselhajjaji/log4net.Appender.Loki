using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace log4net.Appender.Loki
{
    internal class LokiEntry
    {
        public LokiEntry(DateTime timestamp, string line)
        {
            Timestamp = timestamp;
            Line = line;
        }

        public DateTime Timestamp { get; set; }

        public string Line { get; set; }

        private long UnixTimeInNanoseconds(DateTime fromDate)
        {
            DateTime epochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return (fromDate - epochStart).Ticks * 100;
        }

        public List<string> ToStringList()
        {
            return new List<string>() { UnixTimeInNanoseconds(Timestamp).ToString(), Line };
        }
    }
}
