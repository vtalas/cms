using System;
using System.Collections.Specialized;
using System.Configuration.Provider;
using cms.data.Shared.Models;

namespace OAuth2.Mvc
{
    public interface IOAuthService 
    {
        OAuthResponse RequestToken();
		OAuthResponse AccessToken(OAuthRequest rq, Guid applicationId);

		OAuthResponse RefreshToken(string refreshToken);
        bool UnauthorizeToken(string token);

        void Initialize(string name, NameValueCollection config);
        string Name { get; }
        string Description { get; }
    }

    public abstract class OAuthServiceBase : ProviderBase, IOAuthService
    {
        public static IOAuthService Instance { get; set; }

        public abstract OAuthResponse RequestToken();

		public abstract OAuthResponse AccessToken(OAuthRequest rq, Guid applicationId);

        public abstract OAuthResponse RefreshToken(string refreshToken);

        public abstract bool UnauthorizeToken(string token);
    }
}