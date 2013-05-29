﻿using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Controllers;
using Google.Picasa;
using OAuth2.Mvc;
using WebMatrix.WebData;
using cms.Code.LinkAccounts.Picasa;
using cms.data.Dtos;
using cms.data.EF;
using cms.data.EF.DataProviderImplementation;
using cms.data.EF.RepositoryImplementation;
using cms.data.Extensions;
using cms.data.Shared;
using cms.data.Shared.Models;
using cms.shared;

namespace cms.Controllers.Api
{
	public class ClientApiController : CmsWebApiControllerBase
	{
		private OAuthServiceBase oauthservice { get; set; } 
		public SessionProvider SessionFactory { get; set; }
		public PicasaServiceProvider PicasaProvider { get; set; }


		protected override void Initialize(HttpControllerContext requestContext)
		{
			base.Initialize(requestContext);
			SecurityProvider.EnsureInitialized();
			SessionFactory = new SessionProvider(() => new DataEfPublic(ApplicationId, new RepositoryFactory().Create, 0));
			PicasaProvider = new PicasaServiceProvider(() => new PicasaWrapper(SessionFactory));
			oauthservice = new OAuthServiceCms(ApplicationId);

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
			var a = WebSecurityApplication.GetUserProfile(ApplicationId, token);
			return a != null;
		}

		private void Authorize()
		{
			if (!Auth())
			{
				Thread.Sleep(2000);
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
			var response = oauthservice.RequestToken();
			return new OAuthRequest
			{
				RequestToken = response.RequestToken
			};
		}

		public OAuthResponse PostLogin(OAuthRequest data)
		{
			var accessResponse = oauthservice.AccessToken(data, ApplicationId);
			if (!accessResponse.Success)
			{
				Thread.Sleep(2000);
				throw new HttpResponseException(HttpStatusCode.Unauthorized);
			}

			return accessResponse;
		}

		public string PutUserData([FromBody]string userData)
		{
			Authorize();
			var data = new UserData
			{
				Key = "xxx",
				Value = userData
			};


			using (var repo = new EfRepositoryApplication(ApplicationId))
			{
				var user = WebSecurityApplication.CurrentUser();
				if (user == null)
				{
					throw new HttpResponseException(HttpStatusCode.Unauthorized);
				}
				data.User = user;
				repo.Add(data);
				repo.SaveChanges();
			}
	
			return userData;
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
