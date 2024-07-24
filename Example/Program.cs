using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Example
{
    class Program
    {
        // private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static void Main(string[] args)
        {
            // or wherever your file is
            // var fileInfo = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config");
            // if (fileInfo.Exists)
            //     log4net.Config.XmlConfigurator.Configure(fileInfo);
            // else
            //     throw new InvalidOperationException("No log config file found");

            int count = 0;
            var lokiAppender = new LokiAppender();
            lokiAppender.ServiceUrl = "http://localhost:3100/";
            lokiAppender.BufferSize = 5;
            lokiAppender.TenantID = "tenant1";
            lokiAppender.TrustSelfCignedCerts = false;
            lokiAppender.SetLabel("app", "example");
            lokiAppender.ActivateOptions();
            log4net.Config.BasicConfigurator.Configure(lokiAppender);

            var log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            while (true)
            {
                Thread.Sleep(2000);
                log.Info($"Log number { count++ }");
            }
        }
    }
}
