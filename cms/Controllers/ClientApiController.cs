using System;
using System.Web.Mvc;

namespace cms.Controllers
{
    public class ClientApiController : ApiControllerBase
    {
		public ActionResult ViewPageJson(string link)
		{
			var a = db.GetGridPage(link);
			return new Code.JSONNetResult(a);
		}

		public ActionResult GetGrid(Guid id)
		{
			var a = db.GetGridPage(id);
			return new Code.JSONNetResult(a);
		}
	}
}
