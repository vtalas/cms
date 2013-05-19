using System;
using System.Security.Principal;
using System.Web;
using OAuth2.Mvc;

namespace cms.Code.MvcOauth
{
    public class DemoProvider : OAuthProviderBase
    {
        public override IIdentity RetrieveIdentity(HttpContext context)
        {
            var token = context.Request.GetToken();
            return String.IsNullOrEmpty(token)
                ? null
                : new DemoIdentity(this, token);
        }

        public override IPrincipal CreatePrincipal(IIdentity identity)
        {
            return identity == null || !(identity is DemoIdentity)
                ? null
                : new DemoPrincipal(this, identity);
        }
	}

    public class CmsProvider : OAuthProviderBase
    {
        public override IIdentity RetrieveIdentity(HttpContext context)
        {
            var token = context.Request.GetToken();
            return String.IsNullOrEmpty(token)
                ? null
                : new DemoIdentity(this, token);
        }

        public override IPrincipal CreatePrincipal(IIdentity identity)
        {
            return identity == null || !(identity is DemoIdentity)
                ? null
                : new DemoPrincipal(this, identity);
        }
    }
}