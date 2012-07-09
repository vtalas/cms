using System;
using System.IO;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using cms.Code.Bootstraper;
using cms.Code.Bootstraper.DataRepository;
using cms.Code.Bootstraper.Less;
using log4net;

namespace cms.Controllers
{
	public class BootstrapController : Controller
    {
		static readonly ILog Log = LogManager.GetLogger(typeof(BootstrapController));
		
		BootstrapGenerator Bg { get; set; }
		string UserId { get; set; }
		string BasePath { get; set; }

		protected override void Initialize(System.Web.Routing.RequestContext requestContext)
		{
			BasePath = requestContext.HttpContext.Server.MapPath("~/App_Data");
			UserId = requestContext.HttpContext.Session != null ? requestContext.HttpContext.Session.SessionID : Guid.NewGuid().ToString();

			var bootstrapDataRepository = new BootstrapDataRepositoryImpl(BasePath);
			Bg = new BootstrapGenerator(BasePath, UserId, bootstrapDataRepository);
			base.Initialize(requestContext);
		}

		public ActionResult Index()
        {
			return View(Bg.GetUserVariablesJson());
        }

		public ActionResult UserBootstrap(bool? compress)
		{
			var c = compress.HasValue && compress.Value;

			var lesak = new Lessaak(new ResponseLogger(Response), new ServerPathResolver(Server));
			
			var css = Bg.GetUserBootstrapCss(lesak, c);
			
			Response.ContentType = "text/css";
			Response.Write(css);
			Response.Write(Session.SessionID);

        	return new EmptyResult();
        }

		
		[JObjectFilter(Param = "data")]
		public ActionResult Refresh(JObject data)
		{
			Bg.SetUserVariablesJson(data);
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
