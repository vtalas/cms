using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Authentication;
using System.Web.Http.Controllers;
using Google.GData.Client;
using Google.Picasa;
using Microsoft.Ajax.Utilities;
using WebMatrix.WebData;
using cms.Code.LinkAccounts.Picasa;
using cms.data.Dtos;
using cms.data.EF;
using cms.data.EF.DataProviderImplementation;
using cms.data.Shared;


namespace cms.Controllers.Api
{
	public class ClientApiController : CmsWebApiControllerBase
    {
		private PicasaWrapper Picasa { get; set; }
		public SessionProvider SessionFactory { get; set; }

		protected override void Initialize(HttpControllerContext requestContext)
		{
			base.Initialize(requestContext);
			SecurityProvider.EnsureInitialized();
			SessionFactory = new SessionProvider(() => new DataEfPublic(ApplicationId, new RepositoryFactory().Create, 0));
			Picasa = new PicasaWrapper(SessionFactory);
		}
		
		public GridPageDto GetPage(string id)
		{
			const bool auth = true;

			using (var repo = SessionFactory.CreateSession())
			{
				var page = repo.Session.Page.Get(id);

				if (auth && (!WebSecurity.IsAuthenticated || !repo.Session.IsUserAuthorized(WebSecurity.CurrentUserId)))
				{
					throw new System.Security.Authentication.AuthenticationException("log_in");
				}

				return page;
			}
		}


		public IEnumerable<GridPageDto> GetPages()
		{
			using (var repo = SessionFactory.CreateSession())
			{
				return repo.Session.Page.List();
			}
		}

		[System.Web.Mvc.HttpPost]
		public int PostLogin(LoginModelxx data)
		{
			if (ModelState.IsValid && WebSecurity.Login(data.UserName, data.Password))
			{
				return WebSecurity.CurrentUserId;
			}
			return -10;
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

	public class LoginModelxx
	{
		public string UserName { get; set; }
		public string Password { get; set; }
	}

}
