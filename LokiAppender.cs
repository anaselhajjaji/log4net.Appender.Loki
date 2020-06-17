using log4net.Appender.Loki;
using log4net.Appender.Loki.Labels;
using log4net.Core;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;

namespace log4net.Appender
{
    public class LokiAppender : BufferingAppenderSkeleton
    {
        public string ServiceUrl { get; set; }
        public string BasicAuthUserName { get; set; }
        public string BasicAuthPassword { get; set; }
        public bool TrustSelfCignedCerts { get; set; }

        private readonly LokiLabel[] labels = new LokiLabel[]
        {
            new LokiLabel("host", Environment.MachineName),
            new LokiLabel("app", Process.GetCurrentProcess().ProcessName)
        };

        private void PostLoggingEvent(LoggingEvent[] loggingEvents)
        {
            var formatter = new LokiBatchFormatter(labels);
            var httpClient = new LokiHttpClient(TrustSelfCignedCerts);

            if (httpClient is LokiHttpClient c)
            {
                LokiCredentials credentials;

                if (!string.IsNullOrEmpty(BasicAuthUserName) && !string.IsNullOrEmpty(BasicAuthPassword))
                {
                    credentials = new BasicAuthCredentials(ServiceUrl, BasicAuthUserName, BasicAuthPassword);
                }
                else
                {
                    credentials = new NoAuthCredentials(ServiceUrl);
                }

                c.SetAuthCredentials(credentials);
            }

            using (MemoryStream ms = new MemoryStream())
            using (var sc = new StreamWriter(ms))
            {
                formatter.Format(loggingEvents, sc);
                sc.Flush();
                ms.Position = 0;
                var content = new StreamContent(ms);
                var contentStr = content.ReadAsStringAsync().Result; // TO VERIFY
                httpClient.PostAsync(LokiRouteBuilder.BuildPostUri(ServiceUrl), content);
            }
        }

        protected override void SendBuffer(LoggingEvent[] events)
        {
            PostLoggingEvent(events);
        }
    }
}
