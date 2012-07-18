using System.Web.Mvc;

namespace cms.Controllers
{
	public class ClientController : ControllerBase
    {
		public ActionResult ViewPage(string link)
		{
			var a = db.GetGridPage(link);
			return View(ApplicationViewPath("ViewPage"), a);
        }

    }
}
