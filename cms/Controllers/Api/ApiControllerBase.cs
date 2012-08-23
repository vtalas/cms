using System;
using System.Web.Mvc;
using Newtonsoft.Json;
using cms.data.EF;
using cms.data.EF.Initializers;

namespace cms.Controllers.Api
{
	public class ApiControllerBase : Controller
	{
		protected SessionProvider SessionProvider { get; set; }
		
		public Guid ApplicationId { get;  private set; }

		protected override void Initialize(System.Web.Routing.RequestContext requestContext)
		{
			base.Initialize(requestContext);
			var a = this.RouteData;

			ApplicationId = new Guid( a.Values["applicationId"].ToString());
			SessionProvider = new SessionProvider(()=>new JsonDataEf(ApplicationId), new MigrateInitalizer());
		}

		protected override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			base.OnActionExecuted(filterContext);
			ViewBag.ApplicationId = ApplicationId.ToString("N");
		}

		public class JSONNetResult : ActionResult
		{
			private readonly object _data;
			public JSONNetResult(object data)
			{
				_data = data;
			}

			public override void ExecuteResult(ControllerContext context)
			{
				var response = context.HttpContext.Response;
				response.ContentType = "application/json";
				var settings = new JsonSerializerSettings()
				               	{
				               		ReferenceLoopHandling = ReferenceLoopHandling.Ignore
				               	};
				response.Write(JsonConvert.SerializeObject(_data, Formatting.Indented,settings));
			}
		}
	}
}