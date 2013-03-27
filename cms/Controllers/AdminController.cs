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
			using (var db = SessionProvider.CreateSession)
			{
				var applications = db.Applications();

				ViewBag.Json = JSONNetResult.ToJson(applications.ToDtos());
				return View();
			}
		}

		public ActionResult Grids()
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
