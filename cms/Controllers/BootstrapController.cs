using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using cms.Code.Bootstraper;
using dotless.Core;
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
			var test = Server.MapPath("~/App_Data/bootstrap/less/bootstrap.less");


			var a = new LessEngine
						{
							Logger = new ResponseLogger(Response)
						};


			var css = a.TransformToCss(FileExts.GetContent(test), null);

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

		private string CurrentConfigJson(string id)
		{
			var basepath = Server.MapPath("~/App_Data");
			var bb = new BootstrapDataRepositoryImpl(basepath);

			return bb.Get(id);

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
