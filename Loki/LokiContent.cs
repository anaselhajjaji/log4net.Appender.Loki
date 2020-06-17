using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace log4net.Appender.Loki
{
    using System.Collections.Generic;
    using System.IO;
    using Newtonsoft.Json;

    internal class LokiContent
    {
        [JsonProperty("streams")]
        public List<LokiContentStream> Streams { get; set; } = new List<LokiContentStream>();

        public string Serialize()
        {
            JsonSerializer serializer = new JsonSerializer();
            TextWriter writer = new StringWriter();
            serializer.Serialize(writer, this);
            return writer.ToString();
        }
    }
}
