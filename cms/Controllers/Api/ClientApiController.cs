using System;
using System.Linq;
using System.Web.Mvc;
using cms.data.Dtos;
using cms.data.EF;
using cms.data.EF.DataProviderImplementation;
using cms.data.EF.RepositoryImplementation;

namespace cms.Controllers.Api
{
    public class ClientApiController : WebApiControllerBase
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
		//public ActionResult GetGrid(Guid id)
		//{
		//	using (var db = SessionProvider.CreateSession)
		//	{
		//		var a = db.Page.Get(id);
		//		return new Code.JSONNetResult(a);
		//	}
		//}
	}
}
