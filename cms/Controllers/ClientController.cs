using System.Web.Mvc;

namespace cms.Controllers
{
	public class ClientController : ApiControllerBase
    {
        public ActionResult Index()
        {
			return View(ViewPath("index"));
        }
        public ActionResult Service()
        {
			return View(ViewPath("index"));
        }

    }
}
