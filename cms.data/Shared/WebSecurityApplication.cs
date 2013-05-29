using System;
using System.Linq;
using System.Web.Security;
using WebMatrix.WebData;
using cms.data.EF.RepositoryImplementation;
using cms.data.Shared.Models;

namespace cms.data.Shared
{
	public class WebSecurityApplication
	{
		public static string AccessToken { get; private set; }

		public static bool Login(Guid id, string username, string password, bool persisetent)
		{
			var appUserName = ApplicationUserName(id, username);
			return WebSecurity.Login(appUserName, password, persisetent);
		}
		
		public static string CreateUserAndAccount(Guid id, string username, string password, bool persistent=false)
		{
			var appUserName = ApplicationUserName(id, username);
			var ret =  WebSecurity.CreateUserAndAccount(appUserName, password, persistent);
			Roles.AddUserToRole(appUserName, "applicationUser");
			return ret;
		}

		private static string ApplicationUserName(Guid appId, string username)
		{
			return string.Format("{0}_{1}", appId, username);
		}

		public static int GetUserId(Guid applicationId, string username)
		{
			var appUserName = ApplicationUserName(applicationId, username);
			return WebSecurity.GetUserId(appUserName);
		}

		public static bool UserExists(Guid id, string username)
		{
			var appUserName = ApplicationUserName(id, username);
			return WebSecurity.UserExists(appUserName);
		}

		public static bool ChangePassword(Guid applicationId, string userName, string currentPassword, string newPassword)
		{
			var appUserName = ApplicationUserName(applicationId, userName);
			return WebSecurity.ChangePassword(appUserName, currentPassword, newPassword);
		}

		public static bool SetPassword(Guid applicationId, string userName,string newPassword)
		{
			var appUserName = ApplicationUserName(applicationId, userName);
			var token = WebSecurity.GeneratePasswordResetToken(appUserName);
			return WebSecurity.ResetPassword(token, newPassword);
		}

		public static UserProfile GetUserProfile(Guid applicationId, string token)
		{
			using (var repo = new EfRepositoryApplication(applicationId))
			{
				var userprofile =  repo.UserProfile.FirstOrDefault(x => x.AccessToken == token);
				if (userprofile != null && userprofile.IsAccessTokenExpired)
				{
					return null;
				}

				AccessToken = token;
				ApplicationId = applicationId;
				return userprofile;
			}
		}

		public static Guid ApplicationId { get; private set; }

		public static UserProfile CurrentUser()
		{
			using (var repo = new EfRepositoryApplication(ApplicationId))
			{
				return repo.UserProfile.FirstOrDefault(x => x.AccessToken == AccessToken);
			}
		}

	}
}