using System;
using System.Security.Authentication;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;
using cms.data.EF;
using cms.data.EF.Initializers;

namespace cms.Code
{
	public class CmsControllerBase : Controller
	{
		protected SessionProvider SessionProvider { get; set; }
		public Guid ApplicationId { get; private set; }

		protected override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			base.OnActionExecuted(filterContext);
			ViewBag.ApplicationId = ApplicationId;
			using (var db = SessionProvider.CreateSession())
			{
				var appname = db.Session.ApplicationName;
				ViewBag.ApplicationName = appname;
				var x = SiteMap.Provider.FindSiteMapNodeFromKey("xxx");
				if (x != null)
				{
					x.Title = appname;
				}
			}
		}

		protected override void OnException(ExceptionContext filterContext)
		{
			base.OnException(filterContext);

			if (filterContext.Exception.GetType() == typeof(AuthenticationException))
			{
				RedirectToAction("Login", "Account");
			}
			filterContext.ExceptionHandled = true;
			filterContext.HttpContext.Response.Clear();
			filterContext.HttpContext.Response.StatusCode = 500;

			TempData["message"] = filterContext.Exception.Message;
			TempData["ex"] = filterContext.Exception.ToString();
			filterContext.Result = View("CustomError");
			//filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;   
		}

		protected override void Initialize(System.Web.Routing.RequestContext requestContext)
		{
			base.Initialize(requestContext);
			object x = null;

			SecurityProvider.EnsureInitialized();

			if (RouteData.Values.TryGetValue("applicationId", out x))
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
		}
	}
}