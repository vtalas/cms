using System;
using System.Web.Mvc;
using cms.data;
using cms.data.EF;

namespace cms.Controllers
{
	public class ControllerBase : Controller
	{
		protected JsonDataProvider db { get; set; }

		public string ApplicationViewPath(string view)
		{
			return string.Format("{0}/{1}", Application, view);
		}
		public string Application { get;  private set; }
		public Guid ApplicationId { get;  private set; }

		protected override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			base.OnActionExecuted(filterContext);
			ViewBag.Application = Application;
			ViewBag.ApplicationId = ApplicationId;
		}

		protected override void Initialize(System.Web.Routing.RequestContext requestContext)
		{
			base.Initialize(requestContext);
			var a = this.RouteData;
			Application = a.Values["application"].ToString();
			db = new JsonDataEf(Application);
			ApplicationId = db.ApplicationId;

		}
	}
}