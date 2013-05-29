using System;
using WebMatrix.WebData;
using System.Web.Security;

namespace cms.shared
{
	public class WebSecurityApplication
	{
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

		public static int GetUserId(Guid id, string username)
		{
			var appUserName = ApplicationUserName(id, username);
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
	}
}