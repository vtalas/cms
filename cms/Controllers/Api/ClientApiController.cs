using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Controllers;
using Google.Picasa;
using cms.Code.LinkAccounts.Picasa;
using cms.data.DataProvider;
using cms.data.Dtos;
using cms.data.EF;
using cms.data.EF.DataProviderImplementation;
using cms.data.Shared;

namespace cms.Controllers.Api
{
	public class ClientApiController : CmsWebApiControllerBase
    {
		private PicasaWrapper Picasa { get; set; }

		protected override void Initialize(HttpControllerContext requestContext)
		{
			base.Initialize(requestContext);
			var sess = new SessionProvider(() => new DataEfPublic(ApplicationId, new RepositoryFactory().Create, 0));
			Picasa = new PicasaWrapper(sess);
		}

		public GridPageDto GetPage(string id)
		{
			using (var repo = new RepositoryFactory().Create)
			{
				var app = repo.ApplicationSettings.Single(x => x.Id == ApplicationId);
				var session = new PageAbstractImpl(app, repo);

				return session.Get(id);
			}
		}

		public IEnumerable<GridPageDto> GetPages()
		{
			using (var repo = new RepositoryFactory().Create)
			{
				var app = repo.ApplicationSettings.Single(x => x.Id == ApplicationId);
				var session = new PageAbstractImpl(app, repo);
				return session.List();
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
}
