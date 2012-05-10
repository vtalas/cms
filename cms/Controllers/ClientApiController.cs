using System.Web.Mvc;

namespace cms.Controllers
{
    public class ClientApiController : ApiControllerBase
    {
        public ActionResult Index()
        {
			return View();
        }
    }
}
