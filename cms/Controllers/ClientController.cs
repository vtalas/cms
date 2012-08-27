using System.Web.Mvc;
using cms.Code;

namespace cms.Controllers
{
	//TODO: je to k necemu ? 19.8.
	public class ClientController : CmsControllerBase
    {
		public ActionResult ViewPage(string link)
		{
			using (var db = SessionProvider.CreateSession)
			{
				var a = db.Page.Get(link);
				return View(ApplicationViewPath("Index"), a);
			}
		}

    }
}
