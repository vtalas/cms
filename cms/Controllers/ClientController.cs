using System.Web.Mvc;

namespace cms.Controllers
{
	public class ApiController : ApiControllerBase
    {
        public ActionResult Index()
        {
			return View(ViewPath("index"));
        }

    }
}
