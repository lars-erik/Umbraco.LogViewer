using Umbraco.Web.WebApi;

namespace Umbraco.LogViewer.Integrated
{
    public class FileLogController : UmbracoApiController
    {
        public object Get()
        {
            return LogReader.GetLast100();
        }
    }
}
