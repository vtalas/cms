using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcSiteMapProvider;
using MvcSiteMapProvider.Filters;
using cms.Code;
using cms.data.Dtos;
using cms.data.EF;
using cms.data.Extensions;

namespace cms.Controllers
{
	[Authorize]
	public class AdminController : CmsControllerBase
	{
		private IList<ApplicationSettingDto> ApplicationSettings()
		{
			using (var db = SessionProvider.CreateSession())
			{
				var applications = db.Session.Applications();
				return applications.ToDtos();
			}
		}

		public ActionResult Index()
		{

			var applications = ApplicationSettings();

			if (applications.Count() == 1)
			{
				return RedirectToAction("Dashboard", new { applicationId = applications.First().Id });
			}

			ViewBag.Json = JSONNetResult.ToJson(applications);
			return View("Applications");
		}


		public ActionResult Applications()
		{
			var applications = ApplicationSettings();
			ViewBag.Json = JSONNetResult.ToJson(applications);
			return View();
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
