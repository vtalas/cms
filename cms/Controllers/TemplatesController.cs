using System.Web.Mvc;
using cms.Models;

namespace cms.Controllers
{
	public class TemplatesController : Controller
	{
		public ActionResult Index(string id)
		{
			return View(id);
		}

		public ActionResult get(string template)
		{
			return View(template);
		}
	}
}