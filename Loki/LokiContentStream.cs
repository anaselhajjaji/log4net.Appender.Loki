using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace log4net.Appender.Loki
{
    using System.Collections.Generic;
    using System.Text;
    using Labels;
    using Newtonsoft.Json;

    internal class LokiContentStream
    {
        [JsonProperty("stream")]
        public Dictionary<string, string> Labels { get; set; } = new Dictionary<string, string>();

        [JsonIgnore]
        public List<LokiEntry> Entries { get; set; } = new List<LokiEntry>();

        [JsonProperty("values")]
        public IEnumerable<List<string>> Values
        {
            get
            {
                for (int i = 0; i < Entries.Count; i++)
                {
                    yield return Entries[i].ToStringList();
                }
            }
        }
    }
}
