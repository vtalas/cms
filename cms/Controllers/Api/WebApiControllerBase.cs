using System.Web.Http.Controllers;
using WebMatrix.WebData;
using cms.data.EF;

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
			SessionProvider = new SessionProvider(ApplicationId, WebSecurity.CurrentUserId);
		}
	}
}