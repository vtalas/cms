using System;
using System.Security.Authentication;
using System.Web.Mvc;
using WebMatrix.WebData;
using cms.data.EF;

namespace cms.Code
{
	public class CmsControllerBase : Controller
	{
		protected SessionProvider SessionProvider { get; set; }
		
		public Guid ApplicationId { get;  private set; }

		protected override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			base.OnActionExecuted(filterContext);
			ViewBag.ApplicationId = ApplicationId;
		}

		protected override void OnException(ExceptionContext filterContext)
		{
			base.OnException(filterContext);
			
			if (filterContext.Exception.GetType() == typeof (AuthenticationException))
			{
				RedirectToAction("Login", "Account");
			}

		}

		protected override void Initialize(System.Web.Routing.RequestContext requestContext)
		{
			base.Initialize(requestContext);
			object x = null;

			SecurityProvider.EnsureInitialized();

			if (RouteData.Values.TryGetValue("applicationId", out x  ))
			{
				Guid aaa;
				if (Guid.TryParse(x as string, out aaa))
				{
					ApplicationId = aaa;
				}
			}
			else
			{
				ApplicationId = new Guid("00000000-0000-0000-0000-000000000000");
			}

			SessionProvider = new SessionProvider(ApplicationId, WebSecurity.CurrentUserId);

			using (var db = SessionProvider.CreateSession())
			{
				ViewBag.ApplicationName = db.Session.ApplicationName;
			}


		}
	}
}