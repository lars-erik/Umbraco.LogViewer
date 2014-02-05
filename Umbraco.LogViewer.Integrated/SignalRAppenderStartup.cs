using System.Web.Routing;
using Umbraco.Core;

namespace Umbraco.LogViewer.Integrated
{
    public class SignalRAppenderStartup : ApplicationEventHandler
    {
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            base.ApplicationStarted(umbracoApplication, applicationContext);

            RouteTable.Routes.MapConnection<SignalRAppenderConnection>("signalrappender", "/App_Plugins/LogViewer/signalrappender");
        }
    }
}
