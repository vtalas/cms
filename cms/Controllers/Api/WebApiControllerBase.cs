using System.Web.Http.Controllers;
using WebMatrix.WebData;
using cms.data.EF;
using cms.data.EF.DataProviderImplementation;
using cms.data.EF.Initializers;

namespace cms.Controllers.Api
{
	/// <summary>
	/// autentifikovane requesty
	/// 
	/// </summary>
	public class WebApiControllerBase : CmsWebApiControllerBase
	{
		protected SessionProvider SessionProvider { get; set; }

		protected override void Initialize(HttpControllerContext requestContext)
		{
			base.Initialize(requestContext);
			SecurityProvider.EnsureInitialized();
			SessionProvider = new SessionProvider(() => new DataEfAuthorized(ApplicationId, WebSecurity.CurrentUserId), new MigrateInitalizer());
		}
	}
}