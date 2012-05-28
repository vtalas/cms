using System.Web.Mvc;
using Newtonsoft.Json;
using cms.data.EF;

namespace cms.Controllers
{
	public class AdminController : ControllerBase
    {

		public ActionResult Index()
		{
			var applications = JsonDataEf.Applications();
			ViewBag.Json = JSONNetResult.ToJson(applications);

			return View();
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
}
