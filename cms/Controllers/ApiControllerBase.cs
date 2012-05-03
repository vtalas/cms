using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using cms.data;
using cms.data.EF;
using cms.data.Files;

namespace cms.Controllers
{
	public class ApiControllerBase : Controller
	{
		protected JsonDataProvider db { get; set; }

		public string ViewPath(string view)
		{
			return string.Format("{0}/{1}", Application, view);
		}
		public string Application { get;  private set; }

		protected override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			base.OnActionExecuted(filterContext);
			ViewBag.Application = Application;
		}

		protected override void Initialize(System.Web.Routing.RequestContext requestContext)
		{
			base.Initialize(requestContext);
			var a = this.RouteData;
			Application = a.Values["application"].ToString();

			db = new JsonDataEf(Application);

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