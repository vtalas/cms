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

			var oauth2ParametersStorage = OAuth2ParametersStorageFactory.StorageDatabase(SessionProvider);

			//var gdataAuth = new GoogleDataOAuth2(OAuth2ParametersStorageFactory.StorageJsonFile(ApplicationId));
			var gdataAuth = new GoogleDataOAuth2(oauth2ParametersStorage);
			var picasaFactory = new PicasaServiceFactory(gdataAuth.GetRequestDataFactoryInstance("https://picasaweb.google.com/data"));
			Picasa = new PicasaWrapper(picasaFactory.GetService(), gdataAuth.Storage.Parameters);
		}

	}
}