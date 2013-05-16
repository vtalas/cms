using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Controllers;
using Google.Picasa;
using WebMatrix.WebData;
using cms.Code.LinkAccounts.Picasa;
using cms.data.Dtos;
using cms.data.EF;
using cms.data.EF.DataProviderImplementation;
using cms.data.EF.Initializers;
using cms.data.Shared;
using System.Web.Mvc;


namespace cms.Controllers.Api
{
	public class ClientApiController : CmsWebApiControllerBase
    {
		private PicasaWrapper Picasa { get; set; }
		public SessionProvider SessionFactory { get; set; }

		protected override void Initialize(HttpControllerContext requestContext)
		{
			base.Initialize(requestContext);
			SessionFactory = new SessionProvider(() => new DataEfPublic(ApplicationId, new RepositoryFactory().Create, 0));
			SessionFactory = new SessionProvider(() => new DataEfAuthorized(ApplicationId, new RepositoryFactory().Create, 0));
			Picasa = new PicasaWrapper(SessionFactory);
		}
		
		[aaa]
		public GridPageDto GetPage(string id)
		{
			const bool auth = true;
			if (auth && !WebSecurity.IsAuthenticated)
			{
				throw new UnauthorizedAccessException("kbjaskdjbasbdjkabskbdkasbd");
			}

			using (var repo = SessionFactory.CreateSession())
			{
				return  repo.Session.Page.Get(id);
			}
		}
		public IEnumerable<GridPageDto> GetPages()
		{
			using (var repo = SessionFactory.CreateSession())
			{
				return repo.Session.Page.List();
			}
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

	public class aaaAttribute : Attribute
	{

	}
}
