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
		public AdminController()
		{
		}

		public ActionResult Index()
		{

			var applications = db.Applications();

			return View(applications);
		}
		public ActionResult Show()
		{
			return View();
        }
		public ActionResult Grids()
		{
			return View();
        }
		public ActionResult Template(string id)
		{
			var templateName = id;
			return View("template/" + templateName);
        }
		public ActionResult GridElementTmpl(string type, string skin)
		{
			var settings = new TemplateSettings()
			        	{
			        		TemplateEdit = string.Format("_GridElementTmpl/{0}_edit", type),
			        		TemplateView = string.Format("_GridElementTmpl/{0}_view",type),
			        		Type = type
			        	};
			return View("_GridElementTmpl/GridElement", settings);
        }

    }

	public class TemplateSettings
	{
		public string Type { get; set; }
		public string TemplateEdit { get; set; }
		public string TemplateView { get; set; }

	}
}
