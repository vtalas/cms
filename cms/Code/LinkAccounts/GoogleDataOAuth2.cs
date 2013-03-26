using System;
using Google.GData.Client;
using Google.GData.Photos;

namespace cms.Code.LinkAccounts
{
	public class GoogleDataOAuth2
	{
		public IGdataStorage Storage { get; set; }

		public GoogleDataOAuth2(IGdataStorage storage)
		{
			Storage = storage;
		}

		public PicasaService Picasa {get
		{
			var auth = new OAuth2Parameters
			{
				ClientId = Storage.ClientId,
				ClientSecret = Storage.ClientSecret,
				AccessToken = Storage.AccessToken

			};

			var requstFactory = new GOAuth2RequestFactory("https://picasaweb.google.com/data", "testtesds", auth);
			var client = new PicasaService("appname") {RequestFactory = requstFactory};
			return client;
		}}

		public bool IsAuhtorized()
		{
			return !string.IsNullOrEmpty(Storage.AccessToken) && !string.IsNullOrEmpty(Storage.RefreshToken);
		}

		public string OAuth2AuthorizationUrl()
		{
			var auth = new OAuth2Parameters
				{
					ClientId = Storage.ClientId,
					ClientSecret = Storage.ClientSecret,
					RedirectUri = Storage.RedirectUrl,
					Scope = "https://picasaweb.google.com/data"
				};
			return OAuthUtil.CreateOAuth2AuthorizationUrl(auth);
		}

		public void RefreshAccessToken()
		{
			var auth = new OAuth2Parameters
				{
					ClientId = Storage.ClientId,
					ClientSecret = Storage.ClientSecret,
					RefreshToken = Storage.RefreshToken,
				};
			OAuthUtil.RefreshAccessToken(auth);
		}

		public bool isExpired()
		{
			
			return false;
		}
		public OAuth2Parameters GetAccessToken()
		{
			var auth = new OAuth2Parameters
				{
					ClientId = Storage.ClientId,
					ClientSecret = Storage.ClientSecret,
					AccessCode = Storage.AccessCode,
					RedirectUri = Storage.RedirectUrl,
				};
			OAuthUtil.GetAccessToken(auth);
			Storage.AccessToken = auth.AccessToken;
			Storage.TokenExpiry = auth.TokenExpiry;
			Storage.RefreshToken = auth.RefreshToken;
			return auth;
		}
	}

	public interface IGdataStorage
	{
		string ClientId { get; set; }
		string ClientSecret { get; set; }
		string AccessToken { get; set; }
		string RefreshToken { get; set; }
		DateTime TokenExpiry { get; set; }
		string RedirectUrl { get; set; }
		string AccessCode { get; set; }
		void Save();
	}
}