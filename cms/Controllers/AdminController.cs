using System.Web.Mvc;
using Newtonsoft.Json;
using cms.Code;
using cms.Models;
using cms.Service;
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
		
		
		//TODO: tohle asi hodit do template controlleru 
		public ActionResult Template(string id)
		{
			var templateName = id;
			return View("template/" + templateName);
		}

    }
}
