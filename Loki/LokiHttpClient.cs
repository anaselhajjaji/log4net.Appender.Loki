using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace log4net.Appender.Loki
{
    public class LokiHttpClient
    {
        protected readonly HttpClient HttpClient;

        public LokiHttpClient(bool trustSelfCignedCerts)
        {
            if (trustSelfCignedCerts)
            {
                var handler = new WebRequestHandler();
                handler.ServerCertificateValidationCallback = (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                };
                HttpClient = new HttpClient(handler);
            }
            else
            {
                HttpClient = new HttpClient();
            }
        }

        public void SetAuthCredentials(LokiCredentials credentials)
        {
            if (!(credentials is BasicAuthCredentials c))
                return;

            var headers = HttpClient.DefaultRequestHeaders;
            if (headers.Any(x => x.Key == "Authorization"))
                return;

            var token = Base64Encode($"{c.Username}:{c.Password}");
            headers.Add("Authorization", $"Basic {token}");
        }

        public virtual Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content)
        {
            content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
            return HttpClient.PostAsync(requestUri, content);
        }

        public virtual void Dispose()
            => HttpClient.Dispose();

        private static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        public void SetTenantId(string tenantId)
        {
            var headers = HttpClient.DefaultRequestHeaders;
            if (headers.Any(x => x.Key == "X-Scope-OrgID"))
                return;
            headers.Add("X-Scope-OrgID", $"{tenantId}");
        }
    }
}
