using System.Collections.Generic;
using System.Linq;
using cms.data.Dtos;
using cms.data.EF;
using cms.data.EF.DataProviderImplementation;
using cms.data.EF.RepositoryImplementation;

namespace cms.Controllers.Api
{
	public class ClientApiController : CmsWebApiControllerBase
    {
		public GridPageDto GetPage(string id)
		{
			using (var repo = new EfRepository(new EfContext()))
			{
				var app = repo.ApplicationSettings.Single(x => x.Id == ApplicationId);
				var session = new PageAbstractImpl(app, repo);

				return session.Get(id);
			}
		}

		public IEnumerable<GridPageDto> GetPages()
		{
			using (var repo = new EfRepository(new EfContext()))
			{
				var app = repo.ApplicationSettings.Single(x => x.Id == ApplicationId);
				var session = new PageAbstractImpl(app, repo);
				return session.List();
			}
		}
	}
}
