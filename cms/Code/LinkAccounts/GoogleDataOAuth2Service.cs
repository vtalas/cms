using System;
using System.Net;
using Google.GData.Client;

namespace cms.Code.LinkAccounts
{
	public class GoogleDataOAuth2Service
	{
		public OAuth2ParametersStorageAbstract Storage { get; set; }

		public GoogleDataOAuth2Service(OAuth2ParametersStorageAbstract storage)
		{
			Storage = storage;
		}

		public IGDataRequestFactory GetRequestDataFactoryInstance(string scope)
		{
			return new GOAuth2RequestFactory(scope, "ccc ccc aaa", GetValidOAuth2Parameters());
		}

		public OAuth2Parameters GetValidOAuth2Parameters()
		{
			if (IsAuhtorized() && !IsValid())
			{
				RefreshAccessToken();
			}
			return Storage.Parameters;
		}

		public GoogleDataOAuth2Status GetStatus()
		{
			if (IsAuhtorized())
			{
				return GoogleDataOAuth2Status.Authorized;
			}

			if (NeedReAuthorize())
			{
				return GoogleDataOAuth2Status.NeedRefresh;
			}

			return GoogleDataOAuth2Status.NotAuthorized;

		}

		public bool IsAuhtorized()
		{
			return !string.IsNullOrEmpty(Storage.Parameters.AccessToken) && !string.IsNullOrEmpty(Storage.Parameters.RefreshToken);
		}

		public bool NeedReAuthorize()
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
			Storage.Save();
		}

		public bool IsValid()
		{
			return ExpirationInSeconds() > 10;
		}

		public int ExpirationInSeconds()
		{
			var timespan = Storage.Parameters.TokenExpiry.Subtract(DateTime.Now);
			return (int)timespan.TotalSeconds;
		}

		public void GetAccessToken()
		{
			OAuthUtil.GetAccessToken(Storage.Parameters);
			Storage.Save();
		}

		public void RevokeToken()
		{
			const string accessTokenUrl = " https://accounts.google.com/o/oauth2/revoke";

			var url = string.Format("{0}?token={1}", accessTokenUrl, Storage.Parameters.AccessToken);
			var request = (HttpWebRequest)WebRequest.Create(url);
			
			try
			{
				using (var response = request.GetResponse() as HttpWebResponse)
				{

				}
			}
			catch (WebException ex)
			{
				throw new Exception(ex.Message);
			}

			Storage.Parameters = null;
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