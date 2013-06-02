using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;
using Newtonsoft.Json.Serialization;
using cms.data.Dtos;
using cms.data.EF.RepositoryImplementation;
using cms.data.Extensions;
using cms.data.Shared;
using cms.data.Shared.Models;
using cms.shared;

namespace cms.Controllers.Api
{
	[System.Web.Mvc.Authorize(Roles = "admin")]
	public class CmsUsersApiController : CmsWebApiControllerBase
	{
		public IList<UserProfileDto> GetUsers()
		{
			using (var repo = new EfRepositoryApplication(ApplicationId))
			{
				var u = repo.UserProfile.Where(x => x.ApplicationUser.HasValue && x.ApplicationUser.Value > 0);
				return u.ToDtos();
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
				u.Applications.Add(repo.ApplicationSettings.Single(x => x.Id == ApplicationId));
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

			WebSecurityApplication.SetPassword(ApplicationId, user.UserName, user.Password);

			return HttpStatusCode.OK;
		}

		public IList<UserDataDto> GetUserData()
		{
			using (var repo = new EfRepositoryApplication(ApplicationId))
			{

				return repo.UserData.ToList().ToDtos();
			}
		}
		public HttpResponseMessage GetUserDataJSON()
		{
			using (var repo = new EfRepositoryApplication(ApplicationId))
			{
				var resp = new HttpResponseMessage
				{
					Content = new ObjectContent<IList<UserDataDto>>(repo.UserData.ToList().ToDtos(), new JsonMediaTypeFormatter())
				};

				resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
				return resp;
			}
		}
	}


}
