using System.Linq;
using System.Web.Mvc;
using cms.Code;
using cms.data.EF.RepositoryImplementation;

namespace cms.Controllers
{

	[Authorize(Roles = "admin")]
	public class CmsUsersController : CmsControllerBase
	{
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult GetUsers()
		{
			using (var repo = new EfRepositoryApplication(ApplicationId))
			{
				var u = repo.UserProfile.Where(x => x.ApplicationUser.HasValue && x.ApplicationUser.Value > 0);
				return View(u.ToList());
			}

		}
	}
}
