using System;
using System.Data.Entity;
using System.Web.Mvc;
using cms.data.EF;
using cms.data.EF.Initializers;

namespace cms.Code
{
	public class CmsControllerBase : Controller
	{
		protected SessionProvider SessionProvider { get; set; }

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

				
			//SessionProvider = new SessionProvider(Application, new MigrateInitalizer());
			SessionProvider = new SessionProvider(()=> new JsonDataEf(Application), new MigrateInitalizer());

			using (var db = SessionProvider.CreateSession)
			{
				ApplicationId = db.ApplicationId;
			}

		}
	}
}