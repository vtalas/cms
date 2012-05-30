using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using cms.Code.Bootstraper;

namespace cms.Controllers
{
	public class BootstrapController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult aaa()
        {
        	var a = "aasdsa";
        	return new JSONNetResult(a);
        }


		
		public ActionResult Current()
		{
			var id = Session.SessionID;
			var jsonString = CurrentConfigJson(id);
			
			return new JSONNetResult(JObject.Parse(jsonString));
        }

		private string bootstrappath = "~/App_Themes/bootstrap/less/user/bootstrap_{0}.less";

		private string GetUserBootstrap(string id)
		{
			var bootstrapfile = Server.MapPath(string.Format(bootstrappath,id));
			if (!System.IO.File.Exists(bootstrapfile))
			{
				System.IO.File.Create(bootstrapfile);
			}
			return bootstrapfile;
		}
		
		private string CurrentConfigJson(string id)
		{
			//var variablesDefaultJson = "~/App_Themes/bootstrap/default_bootstrap.json";
			//var variablesUserJson = string.Format("~/App_Themes/bootstrap/less/user/variables_{0}.json", id);

			var basepath = Server.MapPath("~/App_Data");
			var bb = new BootstrapDataRepositoryImpl(basepath);

			return bb.Get(id);

		}
    }
}
