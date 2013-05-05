using System.Linq;
using System.Web.Mvc;
using cms.Code;
using cms.data.Extensions;

namespace cms.Controllers
{
	[Authorize]
	public class AdminController : CmsControllerBase
	{
		public ActionResult Index()
		{
			using (var db = SessionProvider.CreateSession())
			{
				var applications = db.Session.Applications();

				if (applications.Count() == 1)
				{
					return RedirectToAction("Grids", new {applicationId = applications.First().Id});
				}

				ViewBag.Json = JSONNetResult.ToJson(applications.ToDtos());
				return View();
			}
		}

		public ActionResult Grids()
		{
			return View();
		}

		public ActionResult Dashboard()
		{
			return View();
		}
		public ActionResult DataContent()
		{
			return View();
		}

		//TODO: tohle asi hodit do template controlleru 
		public ActionResult Template(string id)
		{
			var templateName = id;
			return View("template/" + templateName);
		}

	}
}
