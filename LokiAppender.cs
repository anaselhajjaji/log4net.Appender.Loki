using log4net.Appender.Loki;
using log4net.Appender.Loki.Labels;
using System;
using System.Diagnostics;
using System.IO;
using log4net.Core;
using System.Net.Http;
using System.Collections.Generic;

namespace log4net.Appender
{
    public class LokiAppender : BufferingAppenderSkeleton
    {
        public string ServiceUrl { get; set; }
        public string BasicAuthUserName { get; set; }
        public string BasicAuthPassword { get; set; }
        public bool TrustSelfCignedCerts { get; set; }
        public string TenantID { get; set; }

        private readonly LokiLabel label = new LokiLabel();

        private void PostLoggingEvent(LoggingEvent[] loggingEvents)
        {
            var formatter = new LokiBatchFormatter(label);
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

                if (!string.IsNullOrEmpty(TenantID))
                {
                    c.SetTenantId(TenantID);
                }
            }

            using (MemoryStream ms = new MemoryStream())
            using (var sc = new StreamWriter(ms))
            {
                formatter.Format(loggingEvents, sc);
                sc.Flush();
                ms.Position = 0;
                var content = new StreamContent(ms);
                var contentStr = content.ReadAsStringAsync().Result; // TO VERIFY
                var result = httpClient.PostAsync(LokiRouteBuilder.BuildPostUri(ServiceUrl), content).Result;
            }
        }

        protected override void SendBuffer(LoggingEvent[] events)
        {
            PostLoggingEvent(events);
        }

        public void SetLabel(string key, string value)
        {
            label.Key = key;
            label.Value = value;
        }
    }
}
