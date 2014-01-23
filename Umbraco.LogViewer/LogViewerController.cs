using System;
using System.Threading;
using System.Web.Mvc;

namespace Umbraco.LogViewer
{
    public class LogViewerController : Controller
    {
        private readonly TimeSpan max = new TimeSpan(0,0,5);
        private readonly TimeSpan sleepFor = new TimeSpan(0,0,0,0,50);

        private static string content = null;

        public ContentResult LogEntries()
        {
            TimeSpan elapsed = TimeSpan.Zero;
            while (elapsed < max)
            {
                if (content != null)
                {
                    var it = content;
                    content = null;
                    return Content("{\"data\":\"" + it + "\"}", "application/json");
                }

                Thread.Sleep(sleepFor);
                elapsed += sleepFor;
            }

            throw new Exception("No content");
        }

        public ContentResult Set(string data)
        {
            content = String.IsNullOrWhiteSpace(data) ? null : data;
            return Content("{\"ok\":true}", "application/json");
        }
    }
}
