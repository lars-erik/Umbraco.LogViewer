using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net.Appender;
using log4net.Core;
using Microsoft.SqlServer.Server;

namespace Umbraco.LogViewer
{
    public class SignalRAppender : AppenderSkeleton
    {
        protected override void Append(LoggingEvent loggingEvent)
        {
            var builder = new StringBuilder();
            using (var writer = new StringWriter(builder))
            {
                Layout.Format(writer, loggingEvent);
            }
            SignalRAppenderConnection.Broadcast(builder.ToString());
        }
    }
}
