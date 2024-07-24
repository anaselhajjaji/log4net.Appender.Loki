using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace log4net.Appender.Loki.Labels
{
    public class LokiLabel
    {
        public LokiLabel(string key, string value)
        {
            Key = key;
            Value = value;
        }

        public LokiLabel()
        {
        }

        public string Key { get; set; }

        public string Value { get; set; }
    }
}
