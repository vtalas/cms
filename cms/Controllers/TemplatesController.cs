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

		public ActionResult GridElementTmpl(string type, string skin)
		{
			//TODO: type_edit.cshtml, view.cshtml ... melo by to byt nejak lip
			var settings = new TemplateSettings
				               {
					               TemplateEdit = string.Format("_GridElementTmpl/{0}_edit", type),
					               TemplateView = string.Format("_GridElementTmpl/{0}_view", type),
					               Type = type
				               };
			return View("_GridElementTmpl/GridElement", settings);
		}

		public ActionResult MenuItemTemplate(string type, string skin)
		{
			var settings = new TemplateSettings
				               {
								   TemplateEdit = string.Format("_MenuItemTmpl/{0}_edit", type),
								   TemplateView = string.Format("_MenuItemTmpl/{0}_view", type),
					               Type = type
				               };
			return View("_MenuItemTmpl/Menuitem", settings);
		}
	}
}