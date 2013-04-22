using System;
using System.Web.Http;
using System.Web.Http.Controllers;
using WebMatrix.WebData;
using cms.data.EF;
using cms.data.EF.DataProviderImplementation;
using cms.data.EF.Initializers;

namespace cms.Controllers.Api
{
	public class WebApiControllerBase : ApiController
	{
		protected SessionProvider SessionProvider { get; set; }

		public Guid ApplicationId { get; private set; }

		protected override void Initialize(HttpControllerContext requestContext)
		{
			base.Initialize(requestContext);

			ApplicationId = new Guid(requestContext.RouteData.Values["applicationId"].ToString());
			SecurityProvider.EnsureInitialized();
			SessionProvider = new SessionProvider(() => new DataEfAuthorized(ApplicationId, WebSecurity.CurrentUserId), new MigrateInitalizer());
		}
	}
}