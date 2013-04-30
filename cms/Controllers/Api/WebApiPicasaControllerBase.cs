using System.Web.Http.Controllers;
using cms.Code.LinkAccounts.Picasa;
using cms.data.EF;
using cms.data.EF.DataProviderImplementation;
using cms.data.Shared;

namespace cms.Controllers.Api
{
	public class WebApiPicasaControllerBase : CmsWebApiControllerBase
	{
		protected PicasaWrapper Picasa { get; set; }

		protected override void Initialize(HttpControllerContext requestContext)
		{
			base.Initialize(requestContext);
			var sess = new SessionProvider(() => new DataEfPublic(ApplicationId, new RepositoryFactory().Create, 0));
			Picasa = new PicasaWrapper(sess);
		}
	}
}