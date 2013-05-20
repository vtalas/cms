using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using Google.GData.Client;
using Google.Picasa;
using Microsoft.Ajax.Utilities;
using OAuth2.Mvc;
using WebMatrix.WebData;
using cms.Code.LinkAccounts.Picasa;
using cms.Code.MvcOauth;
using cms.data.Dtos;
using cms.data.EF;
using cms.data.EF.DataProviderImplementation;
using cms.data.Shared;


namespace cms.Controllers.Api
{
	public class ClientApiController : CmsWebApiControllerBase
	{
		private static OAuthServiceBase x = new OAuthServiceCms();
		private PicasaWrapper Picasa { get; set; }
		public SessionProvider SessionFactory { get; set; }

		public ClientApiController()
		{
			OAuthServiceBase.Instance = x;
		}

		protected override void Initialize(HttpControllerContext requestContext)
		{
			base.Initialize(requestContext);
			SecurityProvider.EnsureInitialized();
			SessionFactory = new SessionProvider(() => new DataEfPublic(ApplicationId, new RepositoryFactory().Create, 0));
			//Picasa = new PicasaWrapper(SessionFactory);
			Picasa = null;
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

		public IEnumerable<GridPageDto> GetPages()
		{
			using (var repo = SessionFactory.CreateSession())
			{
				return repo.Session.Page.List();
			}
		}

		public string GetLogin()
		{
			var response = x.RequestToken();
			return response.RequestToken;
		}

		public OAuthResponse PostLogin(OAuthRequest data)
		{
			var accessResponse = x.AccessToken(data);
			return accessResponse;
		}

		public IEnumerable<Album> GetAlbums()
		{
			return Picasa.GetAlbums();
		}

		public AlbumDecorator GetAlbum(string id)
		{
			return Picasa.GetAlbum(id);
		}

		public IEnumerable<PhotoDecorator> GetAlbumPhotos(string id)
		{
			return Picasa.GetAlbumPhotos(id);
		}



	}

	public class OAuthServiceCms : OAuthServiceBase
	{
		public OAuthServiceCms()
		{
			Tokens = new List<DemoToken>();
			RequestTokens = new Dictionary<string, DateTime>();
		}

		public static List<DemoToken> Tokens { get; set; }

		public static Dictionary<String, DateTime> RequestTokens { get; set; }

		public override OAuthResponse RequestToken()
		{
			var token = Guid.NewGuid().ToString("N");
			var expire = DateTime.Now.AddMinutes(5);
			RequestTokens.Add(token, expire);

			return new OAuthResponse
			{
				Expires = (int)expire.Subtract(DateTime.Now).TotalSeconds,
				RequestToken = token,
				RequireSsl = false,
				Success = true
			};
		}

		public override OAuthResponse AccessToken(OAuthRequest rq)
		{
			if (WebSecurity.Login(rq.UserName, rq.Password, rq.Persistent))
			{
				return CreateAccessToken(rq.UserName);
			}
			return new OAuthResponse
			{
				Success = false
			};
		}

		private OAuthResponse CreateAccessToken(string name)
		{
			var token = new DemoToken(name);
			Tokens.Add(token);

			return new OAuthResponse
			{
				AccessToken = token.AccessToken,
				Expires = token.ExpireSeconds,
				RefreshToken = token.RefreshToken,
				RequireSsl = false,
				Success = true
			};
		}

		public override OAuthResponse RefreshToken(string refreshToken)
		{
			throw new System.NotImplementedException();
		}

		public override bool UnauthorizeToken(string token)
		{
			throw new System.NotImplementedException();
		}

		public override void Initialize(string name, NameValueCollection config)
		{
			throw new System.NotImplementedException();
		}

		public string Name { get; private set; }
		public string Description { get; private set; }
	}



}
