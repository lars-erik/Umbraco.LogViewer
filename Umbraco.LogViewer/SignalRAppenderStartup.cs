﻿using Microsoft.Owin;
using Owin;

[assembly:OwinStartup(typeof(Umbraco.LogViewer.SignalRAppenderStartup))]
namespace Umbraco.LogViewer
{
    public class SignalRAppenderStartup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR<SignalRAppenderConnection>("/App_Plugins/LogViewer/signalrappender");
        }
    }
}
