using System.Reflection;
using System.Threading.Tasks;
using log4net;
using log4net.Core;
using Microsoft.AspNet.SignalR;

namespace Umbraco.LogViewer
{
    public class SignalRAppenderConnection : PersistentConnection
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        protected override Task OnReceived(IRequest request, string connectionId, string data)
        {
            Log.Info(data);
            return base.OnReceived(request, connectionId, data);
        }

        public static void Broadcast(string message)
        {
            var context = GlobalHost.ConnectionManager.GetConnectionContext<SignalRAppenderConnection>();
            //var context = GlobalHost.ConnectionManager.GetHubContext<SignalRAppenderConnection>();
            context.Connection.Broadcast(message);
        }
    }
}
