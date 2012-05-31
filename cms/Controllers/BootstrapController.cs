using System.IO;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using cms.Code.Bootstraper;
using dotless.Core;
using dotless.Core.Importers;
using dotless.Core.Input;
using dotless.Core.Parser;

namespace cms.Controllers
{
	public class BootstrapController : Controller
    {
        //string basepath = 
		public ActionResult Index()
        {
            return View();
        }

		private  string LessToCss(string lessfile)
		{
			var parser = new Parser();
			var imp = new Importer(new FileReader(new ServerPathResolver(Server)));

			parser.Importer = imp;

			var a = new LessEngine
			{
				Logger = new ResponseLogger(Response),
				Parser = parser
			};

			//a.Compress = true;
			return a.TransformToCss(FileExts.GetContent(lessfile), lessfile);
			
		}
		public ActionResult UserBootstrap()
        {
			var userid = Session.SessionID;
			var less = Server.MapPath(string.Format("~/App_Data/userdata/bootstrap_{0}.less",userid));
			var lessVariables = Server.MapPath(string.Format("~/App_Data/userdata/variables_{0}.less",userid));

			if (!System.IO.File.Exists(lessVariables))
			{
				var defaultvars = Server.MapPath("~/App_Data/bootstrap/less/variables.less");
				var content = FileExts.GetContent(defaultvars);
				FileExts.SetContent(lessVariables, content);
			}

			if (!System.IO.File.Exists(less))
			{
				var defaultboot = Server.MapPath("~/App_Data/bootstrap/less/bootstrap.less");
				var content = FileExts.GetContent(defaultboot);
				var updatedBoot = content.Replace("variables.less", string.Format("variables_{0}.less", userid));
				FileExts.SetContent(less,updatedBoot);
			}

			var css = LessToCss(less);

			Response.ContentType = "text/css";
			Response.Write(css);
			Response.Write(Session.SessionID);

        	return new EmptyResult();
        }

		public ActionResult Current()
		{
			var id = Session.SessionID;
			var jsonString = CurrentConfigJson(id);
			
			return new JSONNetResult(JObject.Parse(jsonString));
        }

		private string CurrentConfigJson(string id)
		{
			var basepath = Server.MapPath("~/App_Data");
			var jsondata = new BootstrapDataRepositoryImpl(basepath);
			return jsondata.Get(id);
		}
    }
}
