using System.Web.Mvc;
using cms.data.Dtos;

namespace cms.Controllers.Api
{
    public class MenuApiController : ApiControllerBase
    {
		[HttpPost]
		public ActionResult Add(GridPageDto data)
		{
			using (var db = SessionProvider.CreateSession)
			{
				var newgrid = db.Add(data);
				return new JSONNetResult(newgrid);
			}

		}

		[HttpGet]
		public ActionResult Get(string id)
		{
			return new EmptyResult();
		}

		[HttpPost]
		public ActionResult Delete(string id)
		{
			return new EmptyResult();
		}

		[HttpPost]
		public ActionResult Update(string id)
		{
			return new EmptyResult();
		}


	}
}
