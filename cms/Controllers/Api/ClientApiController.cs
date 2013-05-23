using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using Google.Picasa;
using OAuth2.Mvc;
using cms.Code.LinkAccounts.Picasa;
using cms.data.Dtos;
using cms.data.EF;
using cms.data.EF.DataProviderImplementation;
using cms.data.Shared;

namespace cms.Controllers.Api
{
	public class ClientApiController : CmsWebApiControllerBase
	{
		private static OAuthServiceBase x = new OAuthServiceCms();
		public SessionProvider SessionFactory { get; set; }
		public PicasaServiceProvider PicasaProvider { get; set; }

		public ClientApiController()
		{
			OAuthServiceBase.Instance = x;
		}

		protected override void Initialize(HttpControllerContext requestContext)
		{
			base.Initialize(requestContext);
			SecurityProvider.EnsureInitialized();
			SessionFactory = new SessionProvider(() => new DataEfPublic(ApplicationId, new RepositoryFactory().Create, 0));
			PicasaProvider = new PicasaServiceProvider(() => new PicasaWrapper(SessionFactory));
		}

		public GridPageDto GetPage(string id)
		{
			using (var repo = SessionFactory.CreateSession())
			{
				var page = repo.Session.Page.Get(id);

				if (page.Authorize && !Auth())
				{
					throw new HttpResponseException(HttpStatusCode.Unauthorized); 
				}
				return page;
			}
		}

		private bool Auth()
		{
			var cookies = ControllerContext.Request.Headers.GetCookies();
			var token = cookies.Count > 0 ? cookies[0].Cookies.First(x => x.Name == "oauth_token").Value : null;
			var a = OAuthServiceCms.Tokens.FirstOrDefault(x => x.AccessToken == token && !x.IsAccessExpired);
			return a != null;
		}

		private void Authorize()
		{
			if (!Auth())
			{
				throw new HttpResponseException(HttpStatusCode.Unauthorized);
			}
		}

		public IEnumerable<GridPageDto> GetPages()
		{
			using (var repo = SessionFactory.CreateSession())
			{
				return repo.Session.Page.List();
			}
		}

		public OAuthRequest GetLogin()
		{
			var response = x.RequestToken();
			return new OAuthRequest
			{
				RequestToken = response.RequestToken
			};
		}

		public OAuthResponse PostLogin(OAuthRequest data)
		{
			var accessResponse = x.AccessToken(data);
			if (!accessResponse.Success)
			{
				throw new HttpResponseException(HttpStatusCode.Unauthorized);
			}

			return accessResponse;
		}

		public string PutUserData([FromBody]string userData)
		{
			//Authorize();
			var x = userData;
			return x;

		}

		public IEnumerable<Album> GetAlbums()
		{
			return PicasaProvider.Session.GetAlbums();
		}

		public AlbumDecorator GetAlbum(string id)
		{
			return PicasaProvider.Session.GetAlbum(id);
		}

		public IEnumerable<PhotoDecorator> GetAlbumPhotos(string id)
		{
			return PicasaProvider.Session.GetAlbumPhotos(id);
		}
	}
}
