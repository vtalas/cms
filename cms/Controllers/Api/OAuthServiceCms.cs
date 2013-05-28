using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using OAuth2.Mvc;
using cms.Code.MvcOauth;
using cms.data.EF.RepositoryImplementation;
using cms.data.Shared.Models;
using cms.shared;

namespace cms.Controllers.Api
{
	public class OAuthServiceCms : OAuthServiceBase
	{
		public Guid ApplicationId { get; set; }
		public string Name { get; private set; }
		public string Description { get; private set; }

		public OAuthServiceCms(Guid applicationId)
		{
			ApplicationId = applicationId;
			RequestTokens = new Dictionary<string, DateTime>();
		}

		public static Dictionary<String, DateTime> RequestTokens { get; set; }

		public override OAuthResponse RequestToken()
		{
			var token = Guid.NewGuid().ToString("N");
			var expire = DateTime.Now.AddMinutes(5);
			
			RequestTokens.Add(token, expire);

			return new OAuthResponse
			{
				Expires = (int)expire.Subtract(DateTime.Now).TotalSeconds,
				RequestToken = token,
				RequireSsl = false,
				Success = true
			};
		}

		public override OAuthResponse AccessToken(OAuthRequest rq, Guid applicationId)
		{
			if (WebSecurityApplication.Login(applicationId, rq.UserName, rq.Password, rq.Persistent))
			{
				return CreateAccessToken(applicationId, rq.UserName);
			}
			return new OAuthResponse
			{
				Success = false
			};
		}

		private OAuthResponse CreateAccessToken(Guid applicationId, string username)
		{
			var token = new DemoToken();
			using (var repo = new EfRepositoryApplication(applicationId))
			{
				var userId = WebSecurityApplication.GetUserId(applicationId, username);
				var user = repo.UserProfile.Single(x => x.Id == userId);

				user.AccessToken = token.AccessToken;
				user.RefreshToken = token.RefreshToken;
				user.Expire = token.Expire;
				repo.SaveChanges();
			
				return new OAuthResponse
				{
					AccessToken = token.AccessToken,
					Expires = token.Expire,
					RefreshToken = user.RefreshToken,
					RequireSsl = false,
					Success = true
				};
			}
		}

		public override OAuthResponse RefreshToken(string refreshToken)
		{
			throw new System.NotImplementedException();
		}

		public override bool UnauthorizeToken(string token)
		{
			throw new System.NotImplementedException();
		}

		public override void Initialize(string name, NameValueCollection config)
		{
			throw new System.NotImplementedException();
		}

		public override UserProfile GetAcccesToken(string token)
		{
			using (var repo = new EfRepositoryApplication(ApplicationId))
			{
				var userprofile =  repo.UserProfile.FirstOrDefault(x => x.AccessToken == token);
				return userprofile != null && !userprofile.IsAccessTokenExpired ? userprofile : null;
			}
		}


	}
}