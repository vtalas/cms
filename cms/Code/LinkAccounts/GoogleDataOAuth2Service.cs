using System;
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
//			string accessTokenUrl = "https://www.google.com/accounts/AuthSubRevokeToken";
//			var parameters = this.Storage.Parameters;
//
//			var uri = new Uri(string.Format("{0}?scope={1}", accessTokenUrl, (object)OAuthBase.EncodingPerRFC3986(parameters.Scope)));
//			WebRequest webRequest = WebRequest.Create(new Uri(parameters.TokenUri));
//			webRequest.Method = "POST";
//			webRequest.ContentType = "application/x-www-form-urlencoded";
//			StreamWriter streamWriter = new StreamWriter(webRequest.GetRequestStream());
//			streamWriter.Write(requestBody);
//			((TextWriter)streamWriter).Flush();
//			streamWriter.Close();
//			WebResponse response = webRequest.GetResponse();
//	
//			var x = new StreamReader(response.GetResponseStream()).ReadToEnd();
//
//			if (response == null)
//				return;
//


			//	OAuthUtil.GetUnauthorizedRequestToken(); .GetAccessToken(Storage.Parameters);
			//Google.GData.Client.AuthSubUtil.getRevokeTokenUrl()e();
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