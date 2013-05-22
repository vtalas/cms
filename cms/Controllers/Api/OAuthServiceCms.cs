using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Text;
using OAuth2.Mvc;
using WebMatrix.WebData;
using cms.Code.MvcOauth;

namespace cms.Controllers.Api
{
	public class OAuthServiceCms : OAuthServiceBase
	{
		public string Name { get; private set; }
		public string Description { get; private set; }

		public OAuthServiceCms()
		{
			Tokens = new List<DemoToken>();
			RequestTokens = new Dictionary<string, DateTime>();
		}

		public static List<DemoToken> Tokens { get; set; }

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

		public override OAuthResponse AccessToken(string requestToken, string grantType, string userName, string password, bool persistent)
		{
			throw new NotImplementedException();
		}

		public override OAuthResponse AccessToken(OAuthRequest rq)
		{
			if (WebSecurity.Login(rq.UserName, rq.Password, rq.Persistent))
			{
				return CreateAccessToken();
			}
			return new OAuthResponse
			{
				Success = false
			};
		}

		private OAuthResponse CreateAccessToken()
		{
			var token = new DemoToken();
			Tokens.Add(token);
			
			return new OAuthResponse
			{
				AccessToken = token.AccessToken,
				Expires = token.Expire,
				RefreshToken = token.RefreshToken,
				RequireSsl = false,
				Success = true
			};
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

	}
}