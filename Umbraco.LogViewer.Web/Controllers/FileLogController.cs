using System.Web.Http;

namespace Umbraco.LogViewer.Web.Controllers
{
    public class FileLogController : ApiController
    {
        // GET api/<controller>
        public object Get()
        {
            return LogReader.GetLast100();
        }
    }
}