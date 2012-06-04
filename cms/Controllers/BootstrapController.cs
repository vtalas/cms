﻿using System;
using System.IO;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using cms.Code.Bootstraper;
using log4net;

namespace cms.Controllers
{
	public class BootstrapController : Controller
    {
		private static readonly ILog Log = LogManager.GetLogger(typeof(BootstrapController));
		
		private BootstrapDataRepository JsonRepository { get; set; }
		private string UserId { get; set; }

		protected override void Initialize(System.Web.Routing.RequestContext requestContext)
		{
			var basepath = requestContext.HttpContext.Server.MapPath("~/App_Data");
			JsonRepository = new BootstrapDataRepositoryImpl(basepath);
			UserId = requestContext.HttpContext.Session != null ? requestContext.HttpContext.Session.SessionID : Guid.NewGuid().ToString();

			base.Initialize(requestContext);
		}


		public ActionResult Index()
        {
			return View();
        }

		public ActionResult UserBootstrap()
        {
			var less = Server.MapPath(string.Format("~/App_Data/userdata/bootstrap_{0}.less", UserId));
			var lessVariables = Server.MapPath(string.Format("~/App_Data/userdata/variables_{0}.less", UserId));

			Log.Info("kjbasdkjbsajkdbasjkbdkjbsad");

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
				var updatedBoot = content.Replace("bootstrap/less/variables.less", string.Format("userdata/variables_{0}.less", UserId));
				FileExts.SetContent(less,updatedBoot);
			}

			var lesak = new Lessaak(new ResponseLogger(Response), new ServerPathResolver(Server));
			
			var css =lesak.LessToCss(less);

			Response.ContentType = "text/css";
			Response.Write(css);
			Response.Write(Session.SessionID);

        	return new EmptyResult();
        }

		public ActionResult Current()
		{
			return new JSONNetResult(JsonRepository.Get(UserId));
        }

		
		[JObjectFilter(Param = "data")]
		public ActionResult Refresh(JObject data)
		{
			JsonRepository.Save(UserId, data);

			var lessVariables = Server.MapPath(string.Format("~/App_Data/userdata/variables_{0}.less", UserId));
			
			FileExts.SetContent(lessVariables,data.ToLess());

			return new EmptyResult();
		}


		 public class JObjectFilter : ActionFilterAttribute {

			public string Param { get; set; }

			public override void OnActionExecuting(ActionExecutingContext filterContext)
			{
				if ((filterContext.HttpContext.Request.ContentType ?? string.Empty).Contains("application/json"))
				{
					var stream = filterContext.HttpContext.Request.InputStream;
					stream.Position = 0;

					var streamReader = new StreamReader(stream);
					
					var x = JsonConvert.DeserializeObject<JObject>(streamReader.ReadToEnd());
					filterContext.ActionParameters[Param] = x;
				}
			}
		 }



    }
}
