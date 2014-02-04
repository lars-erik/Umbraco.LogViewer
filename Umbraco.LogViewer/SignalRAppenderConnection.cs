using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using log4net;
using log4net.Core;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;

namespace Umbraco.LogViewer
{
    public class SignalRAppenderConnection : PersistentConnection
    {
        public static Func<IConnection> GetConnection = GetConnectionImpl; 
        static Dictionary<string, Action<string>> logMethods = new Dictionary<string, Action<string>>
        {
            {"Info", (m) => Log.Info(m) },
            {"Debug", (m) => Log.Debug(m) },
            {"Error", (m) => Log.Error(m) },
        };
        
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        protected override Task OnReceived(IRequest request, string connectionId, string data)
        {
            var dto = JsonConvert.DeserializeObject<LogDto>(data);
            logMethods[dto.Level](dto.Message);
            return base.OnReceived(request, connectionId, data);
        }

        public static void Broadcast(object logDto)
        {
            var connection = GetConnection();
            connection.Broadcast(logDto);
        }

        public static void Reset()
        {
            GetConnection = GetConnectionImpl;
        }

        private static IConnection GetConnectionImpl()
        {
            var context = GlobalHost.ConnectionManager.GetConnectionContext<SignalRAppenderConnection>();
            var connection = context.Connection;
            return connection;
        }
    }
}
