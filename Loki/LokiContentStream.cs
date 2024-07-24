using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net.Appender.Loki.Loki.Utils;

namespace log4net.Appender.Loki
{
    using System.Collections.Generic;
    using System.Text;
    using Labels;
    using Newtonsoft.Json;

    internal class LokiContentStream
    {
        [JsonProperty("stream")]
        public Dictionary<string, string> Labels { get; } = new Dictionary<string, string>();

        [JsonProperty("values")]
        public IList<IList<string>> Entries { get; set; } = new List<IList<string>>();

        public void AddLabel(string key, string value)
        {
            Labels[key] = value;
        }

        public void AddEntry(DateTimeOffset timestamp, string entry)
        {
            Entries.Add(new[] {timestamp.ToUnixNanosecondsString(), entry});
        }
    }
}
