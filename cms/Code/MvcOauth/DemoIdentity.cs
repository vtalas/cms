using System.Linq;
using OAuth2.Mvc;

namespace cms.Code.MvcOauth
{
    public class DemoIdentity : OAuthIdentityBase
    {
        public DemoIdentity(IOAuthProvider provider, string token)
            : base(provider)
        {
            Token = token;
            Realm = "Demo";
        }

        protected override void Load()
        {
            var token = DemoService.Tokens.FirstOrDefault(t => t.AccessToken == Token && !t.IsAccessExpired);
            if (token == null)
                return;

            IsAuthenticated = true;
        }
    }
}