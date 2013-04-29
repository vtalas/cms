using System.Web.Http.Controllers;
using cms.Code.LinkAccounts;
using cms.Code.LinkAccounts.Picasa;

namespace cms.Controllers.Api
{
	public class WebApiPicasaControllerBase : WebApiControllerBase
	{
		protected PicasaWrapper Picasa { get; set; }

		protected override void Initialize(HttpControllerContext requestContext)
		{
			base.Initialize(requestContext);
			Picasa = new PicasaWrapper(SessionProvider);
		}
	}
}