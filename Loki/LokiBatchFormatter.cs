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
        private readonly LokiLabel _globalLabels;

        public LokiBatchFormatter()
        {
        }

        public LokiBatchFormatter(LokiLabel globalLabels)
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
                stream.AddLabel(_globalLabels.Key, _globalLabels.Value);
                stream.AddEntry(localTimeAndOffset, sb.ToString());
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
