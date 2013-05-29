using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using WebMatrix.WebData;
using cms.data.Dtos;
using cms.data.EF.RepositoryImplementation;
using cms.data.Shared.Models;
using cms.shared;

namespace cms.Controllers.Api
{
	[System.Web.Mvc.Authorize(Roles = "admin")]
	public class CmsUsersApiController : CmsWebApiControllerBase
	{
		public IList<UserProfile> GetUsers()
		{
			using (var repo = new EfRepositoryApplication(ApplicationId))
			{
				var u = repo.UserProfile.Where(x => x.ApplicationUser.HasValue && x.ApplicationUser.Value > 0);
				return u.ToList();
			}
		}

		public HttpStatusCode PostUser(UserProfileDto user)
		{
			if (string.IsNullOrEmpty(user.Password) || string.IsNullOrEmpty(user.UserName))
			{
				throw new HttpResponseException(HttpStatusCode.BadRequest);
			}
			if (WebSecurityApplication.UserExists(ApplicationId, user.UserName))
			{
				throw new HttpResponseException(HttpStatusCode.Conflict);
			}
			WebSecurityApplication.CreateUserAndAccount(ApplicationId, user.UserName, user.Password);

			using (var repo = new EfRepository())
			{
				var userId = WebSecurityApplication.GetUserId(ApplicationId, user.UserName);
				var u = repo.UserProfile.SingleOrDefault(x => x.Id == userId);
				u.ApplicationUser = 1;
				u.Applications.Add(repo.ApplicationSettings.Single(x=>x.Id == ApplicationId));
				repo.SaveChanges();

			}

			return HttpStatusCode.Created;
		}

		public HttpStatusCode PutUser(UserProfileDto user)
		{
			if (string.IsNullOrEmpty(user.Password) || string.IsNullOrEmpty(user.UserName))
			{
				throw new HttpResponseException(HttpStatusCode.BadRequest);
			}

			using (var repo = new EfRepository())
			{
				var userId = WebSecurityApplication.GetUserId(ApplicationId, user.UserName);
				
				WebSecurityApplication.ChangePassword(ApplicationId, user.UserName, TODO, TODO);
				

				WebSecurity.ResetPassword()
				var u = repo.UserProfile.SingleOrDefault(x => x.Id == userId);
				u.ApplicationUser = 1;
				u.Applications.Add(repo.ApplicationSettings.Single(x=>x.Id == ApplicationId));
				repo.SaveChanges();

			}

			return HttpStatusCode.Created;
		}
	}


}
