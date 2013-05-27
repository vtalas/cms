using System.Linq;
using System.Net;
using System.Web.Http;
using cms.data.Dtos;
using cms.data.EF.RepositoryImplementation;
using cms.data.Shared.Models;
using cms.shared;

namespace cms.Controllers.Api
{
	[System.Web.Mvc.Authorize(Roles = "admin")]
	public class CmsUsersApiController : WebApiControllerBase
	{
		public IQueryable<UserProfile> GetUsers()
		{
			using (var repo = new EfRepositoryApplication(ApplicationId))
			{
				var u = repo.UserProfile.Where(x => x.ApplicationUser.HasValue && x.ApplicationUser.Value > 0);
				return u;
			}
		}

		public HttpStatusCode PostUser(UserProfileDto user)
		{
			if (WebSecurityApplication.UserExists(ApplicationId, user.UserName))
			{
				throw new HttpResponseException(HttpStatusCode.Conflict);
			}
			WebSecurityApplication.CreateUserAndAccount(ApplicationId, user.UserName, user.Password);
			return HttpStatusCode.Created;
		}
	}


}
