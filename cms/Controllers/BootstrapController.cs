using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using cms.Code.Bootstraper;
using dotless.Core;

namespace cms.Controllers
{
	public class BootstrapController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult css()
        {
        	var css = "body{color:yellow;}";

        	var a = new LessEngine();



			//this.Engine.TransformToCss(this.FileReader.GetFileContents(localPath), localPath))

			Response.ContentType = "text/css";
			Response.Write(css);

        	return new EmptyResult();
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

			//string bbb = "~/Content/aaa.less"+DateTime.Now;
			//var aaa = Server.MapPath(bbb);
			//SetContent(aaa,"body{color:red}");

			var basepath = Server.MapPath("~/App_Data");
			var bb = new BootstrapDataRepositoryImpl(basepath);

			return bb.Get(id);

		}
		private void SetContent(string file, string content)
		{
			var target = new StreamWriter(file);
			target.Write(content);
			target.Close();
		}


    }
}
