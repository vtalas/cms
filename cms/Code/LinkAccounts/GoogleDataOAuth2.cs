using Google.GData.Client;
using Google.GData.Photos;

namespace cms.Code.LinkAccounts
{
	public class GoogleDataOAuth2
	{
		public OAuth2ParametersStorageAbstract Storage { get; set; }

		public GoogleDataOAuth2(OAuth2ParametersStorageAbstract storage)
		{
			Storage = storage;
		}

		public PicasaService Picasa {get
		{
			var requstFactory = new GOAuth2RequestFactory("https://picasaweb.google.com/data", "testtesds", Storage.Parameters);
			var client = new PicasaService("appname") {RequestFactory = requstFactory};
			return client;
		}}

		public GoogleDataOAuth2Status GetStatus()
		{
			if (IsAuhtorized())
			{
				return GoogleDataOAuth2Status.Authorized;
			}

			if (NeedRefresh())
			{
				return GoogleDataOAuth2Status.NeedRefresh;
			}

			return GoogleDataOAuth2Status.NotAuthorized;

		}

		public bool IsAuhtorized()
		{
			return !string.IsNullOrEmpty(Storage.Parameters.AccessToken) && !string.IsNullOrEmpty(Storage.Parameters.RefreshToken);
		}

		public bool NeedRefresh()
		{
			return !string.IsNullOrEmpty(Storage.Parameters.AccessToken) && string.IsNullOrEmpty(Storage.Parameters.RefreshToken);
		}

		public string OAuth2AuthorizationUrl()
		{
			return OAuthUtil.CreateOAuth2AuthorizationUrl(Storage.Parameters);
		}

		public void RefreshAccessToken()
		{
			OAuthUtil.RefreshAccessToken(Storage.Parameters);
		}

		public bool isExpired()
		{
			return false;
		}
		public void GetAccessToken()
		{
			OAuthUtil.GetAccessToken(Storage.Parameters);
			Storage.Save();
		}
	}

	public enum GoogleDataOAuth2Status
	{
		NeedRefresh = 0,
		Authorized = 1,
		NotAuthorized = 2
	}
}