using log4net.Appender.Loki.Labels;
using log4net.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace log4net.Appender.Loki
{
    internal class LokiBatchFormatter
    {
        private readonly IList<LokiLabel> _globalLabels;

        public LokiBatchFormatter()
        {
            _globalLabels = new List<LokiLabel>();
        }

        public LokiBatchFormatter(IList<LokiLabel> globalLabels)
        {
            _globalLabels = globalLabels;
        }

        public void Format(IEnumerable<LoggingEvent> logEvents, TextWriter output)
        {
            if (logEvents == null)
                throw new ArgumentNullException(nameof(logEvents));
            if (output == null)
                throw new ArgumentNullException(nameof(output));


            List<LoggingEvent> logs = logEvents.ToList();
            if (!logs.Any())
                return;

            var content = new LokiContent();

            foreach (LoggingEvent logEvent in logs)
            {
                var stream = new LokiContentStream();
                content.Streams.Add(stream);

                stream.Labels.Add(new LokiLabel("level", GetLevel(logEvent.Level)));
                foreach (LokiLabel globalLabel in _globalLabels)
                    stream.Labels.Add(new LokiLabel(globalLabel.Key, globalLabel.Value));

                foreach (var key in logEvent.Properties.GetKeys())
                {
                    // Some enrichers pass strings with quotes surrounding the values inside the string,
                    // which results in redundant quotes after serialization and a "bad request" response.
                    // To avoid this, remove all quotes from the value.
                    stream.Labels.Add(new LokiLabel(key, logEvent.Properties[key].ToString().Replace("\"", "")));
                }

                var localTime = DateTime.Now;
                var localTimeAndOffset = new DateTimeOffset(localTime, TimeZoneInfo.Local.GetUtcOffset(localTime));
                var time = localTimeAndOffset.ToString("o");

                var sb = new StringBuilder();
                sb.AppendLine(logEvent.RenderedMessage);
                if (logEvent.ExceptionObject != null)
                {
                    var e = logEvent.ExceptionObject;
                    while (e != null)
                    {
                        sb.AppendLine(e.Message);
                        sb.AppendLine(e.StackTrace);
                        e = e.InnerException;
                    }
                }

                stream.Entries.Add(new LokiEntry(time, sb.ToString()));
            }
            
            if (content.Streams.Count > 0)
                output.Write(content.Serialize());
        }

        private static string GetLevel(Level level)
        {
            if (level == Level.Info)
                return "info";

            return level.ToString().ToLower();
        }
    }
}
