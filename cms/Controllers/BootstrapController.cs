using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using cms.Code.Bootstraper;
using dotless.Core;
using dotless.Core.Importers;
using dotless.Core.Input;
using dotless.Core.Loggers;
using dotless.Core.Parser;

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
			Directory.SetCurrentDirectory(Server.MapPath("~/App_Data/bootstrap/less/"));

			var test = Server.MapPath("~/App_Data/bootstrap/less/bootstrap.less");

			var parser = new Parser();
        	var imp = new Importer(new FileReader(new ServerPathResolver(Server)));

        	parser.Importer = imp;

			var a = new LessEngine
						{
							Logger = new ResponseLogger(Response),
							Parser = parser
						};

			//a.Compress = true;
			var css = a.TransformToCss(FileExts.GetContent(test), test);
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
			var bb = new BootstrapDataRepositoryImpl(basepath);

			return bb.Get(id);

		}
    }

	public class ServerPathResolver : IPathResolver
	{
		private HttpServerUtilityBase Server { get; set; }
		public ServerPathResolver(HttpServerUtilityBase server)
		{
			Server = server;
		}

		public string GetFullPath(string path)
		{
			return Server.MapPath(path);
		}
	}

	public class ResponseLogger : ILogger
	{
		private HttpResponseBase r { get; set; }
		public ResponseLogger(HttpResponseBase response)
		{
			r = response;
		}

		public void Log(LogLevel level, string message)
		{
			throw new NotImplementedException();
		}

		public void Info(string message)
		{
			throw new NotImplementedException();
		}

		public void Debug(string message)
		{
			throw new NotImplementedException();
		}

		public void Warn(string message)
		{
			throw new NotImplementedException();
		}

		public void Error(string message)
		{
			r.Write(message);
		}
	}
}
