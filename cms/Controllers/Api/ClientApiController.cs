using System;
using System.Web.Mvc;
using cms.data.Dtos;

namespace cms.Controllers.Api
{
    public class ClientApiController : WebApiControllerBase
    {
		public GridPageDto GetPage(string id)
		{
			using (var db = SessionProvider.CreateSession)
			{
				return db.Page.Get(id);
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
