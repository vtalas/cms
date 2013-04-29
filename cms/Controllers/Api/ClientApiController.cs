using System.Collections.Generic;
using System.Linq;
using cms.Code.LinkAccounts.Picasa;
using cms.data.DataProvider;
using cms.data.Dtos;
using cms.data.EF;
using cms.data.EF.DataProviderImplementation;
using cms.data.EF.RepositoryImplementation;
using cms.data.Shared;

namespace cms.Controllers.Api
{
	public class ClientApiController : CmsWebApiControllerBase
    {
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

		//public AlbumDecorator GetAlbum(string id)
		//{
		//	return Picasa.GetAlbum(id);
		//}

		public IEnumerable<GridPageDto> GetAlbum()
		{
			using (var repo = new RepositoryFactory().Create)
			{
				var app = repo.ApplicationSettings.Single(x => x.Id == ApplicationId);
				var session = new Settings(app, repo);

				return null;
			}
		}
	}
}
