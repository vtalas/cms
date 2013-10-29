using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Controllers;
using cms.Code.Graphic;
using cms.Controllers.Api.Filters;
using Google.Picasa;
using OAuth2.Mvc;
using cms.Code.LinkAccounts.Picasa;
using cms.data.Dtos;
using cms.data.EF;
using cms.data.EF.DataProviderImplementation;
using cms.data.EF.RepositoryImplementation;
using cms.data.Shared;
using cms.data.Shared.Models;
using WebAPI.OutputCache;

namespace cms.Controllers.Api
{
	[ExceptionFilter]
	public class ClientApiController : CmsWebApiControllerBase
	{
		private OAuthServiceBase Oauthservice { get; set; }
		public SessionProvider SessionFactory { get; set; }
		public PicasaServiceProvider PicasaProvider { get; set; }

		protected override void Initialize(HttpControllerContext requestContext)
		{
			base.Initialize(requestContext);
			SecurityProvider.EnsureInitialized();
			SessionFactory = new SessionProvider(() => new DataEfPublic(ApplicationId, new RepositoryFactory().Create, 0));
			PicasaProvider = new PicasaServiceProvider(() => new PicasaWrapper(SessionFactory));
			Oauthservice = new OAuthServiceCms(ApplicationId);
		}

		private bool Auth()
		{
			var cookies = ControllerContext.Request.Headers.GetCookies();
			var token = cookies.Count > 0 ? cookies[0].Cookies.First(x => x.Name == "oauth_token").Value : null;
			var a = WebSecurityApplication.GetUserProfile(ApplicationId, token);
			return a != null;
		}

		private void Authorize()
		{
			if (!Auth())
			{
				throw new HttpResponseException(HttpStatusCode.Unauthorized);
			}
		}

		[CacheOutput(ClientTimeSpan = 3600, ServerTimeSpan = 3600)]
		public GridPageDto GetPage(string id)
		{
			using (var repo = SessionFactory.CreateSession())
			{
				var page = repo.Session.Page.Get(id);

				if (page.Authorize && !Auth())
				{
					//throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "this item does not exist");
					throw new HttpResponseException(HttpStatusCode.Unauthorized);
				}
				return page;
			}
		}

		[CacheOutput(ClientTimeSpan = 3600, ServerTimeSpan = 3600)]
		public IEnumerable<GridPageDto> GetPages()
		{
			using (var repo = SessionFactory.CreateSession())
			{
				return repo.Session.Page.List();
			}
		}

		public OAuthRequest GetLogin()
		{
			var response = Oauthservice.RequestToken();
			return new OAuthRequest
			{
				RequestToken = response.RequestToken
			};
		}

		public OAuthResponse PostLogin(OAuthRequest data)
		{
			var accessResponse = Oauthservice.AccessToken(data, ApplicationId);
			if (!accessResponse.Success)
			{
				Thread.Sleep(2000);
				throw new HttpResponseException(HttpStatusCode.Unauthorized);
			}

			return accessResponse;
		}

		public string PostUserData([FromBody]string data, string id)
		{
			Authorize();
			var userData = new UserData
			{
				Key = string.IsNullOrEmpty(id) ? "default" : id,
				Value = data
			};



			using (var repo = new EfRepositoryApplication(ApplicationId))
			{
				var user = repo.UserProfile.FirstOrDefault(x => x.AccessToken == WebSecurityApplication.AccessToken);
				if (user == null)
				{
					throw new HttpResponseException(HttpStatusCode.Unauthorized);
				}
				userData.User = user;
				repo.Add(userData);
				repo.SaveChanges();
			}

			return data;
		}

		[CacheOutput(ClientTimeSpan = 3600, ServerTimeSpan = 3600)]
		public IEnumerable<Album> GetAlbums()
		{
			return PicasaProvider.Session.GetAlbums();
		}

		[CacheOutput(ClientTimeSpan = 3600, ServerTimeSpan = 3600)]
		public AlbumDecorator GetAlbum(string id, int? size = null, bool isSquare = true, ImageSizeType x = ImageSizeType.MaxWidth)
		{
			if (size.HasValue)
			{
				var settings = new List<GdataPhotosSettings>()
				{
					new GdataPhotosSettings(x, size.Value, isSquare)
				};
				return PicasaProvider.Session.GetAlbum(id, settings);
			}
			return PicasaProvider.Session.GetAlbum(id);
		}

		[CacheOutput(ClientTimeSpan = 3600, ServerTimeSpan = 3600)]
		public IEnumerable<PhotoDecorator> GetAlbumPhotos(string id)
		{
			return PicasaProvider.Session.GetAlbumPhotos(id);
		}

		[CacheOutput(ClientTimeSpan = 3600, ServerTimeSpan = 3600)]
		public IEnumerable<PhotoDecorator> GetPhotos()
		{
			return PicasaProvider.Session.GetPhotos();
		}

	}
}
