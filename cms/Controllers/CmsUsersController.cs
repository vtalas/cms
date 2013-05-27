using System.Web.Mvc;
using cms.Code;

namespace cms.Controllers
{

	[Authorize(Roles = "admin")]
	public class CmsUsersController : CmsControllerBase
	{
		public ActionResult Index()
		{
			return View();
		}
	}
}
