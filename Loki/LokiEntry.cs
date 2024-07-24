using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace log4net.Appender.Loki
{
    internal class LokiEntry
    {
        public List<String> Values { get; set; } // = new List<string{ get; set; }

        public LokiEntry(string ts, string line)
        {
            Ts = ts;
            Line = line;
        }

        [JsonProperty("ts")]
        public string Ts { get; set; }

        [JsonProperty("line")]
        public string Line { get; set; }
    }
}
