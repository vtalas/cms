using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using cms.data;
using cms.data.EF;
using cms.data.Files;
using cms.data.Models;

namespace cms.Controllers
{
	public class AdminController : ApiControllerBase
    {
		private JsonDataProvider db { get; set; }

		public AdminController()
		{
			db = new JsonDataFiles("xxx");
		}

		public ActionResult Index()
		{
			if (!string.IsNullOrEmpty(Application))
			{
				return RedirectToAction("Show");
			}

			var applications = db.Applications();

			return View(applications);
		}
		
		public ActionResult Show()
		{
			return View();
        }

    }
}
