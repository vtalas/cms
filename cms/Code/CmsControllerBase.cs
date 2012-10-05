using System;
using System.Web.Mvc;
using WebMatrix.WebData;
using cms.data.EF;
using cms.data.EF.DataProvider;

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
			ViewBag.ApplicationId = ApplicationId;
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

			SessionProvider = new SessionProvider(() => new DataEfAuthorized(ApplicationId, WebSecurity.CurrentUserId));

	


		}
	}
}