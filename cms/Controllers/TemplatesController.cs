using System.Web.Mvc;
using Newtonsoft.Json;
using cms.Code;
using cms.Models;
using cms.Service;
using cms.data.EF;

namespace cms.Controllers
{
	public class TemplatesController : Controller
    {
		public ActionResult GridElementTmpl(string type, string skin)
		{
			//TODO: type_edit.cshtml, view.cshtml ... melo by to byt nejak lip
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
