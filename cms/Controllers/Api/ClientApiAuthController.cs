using System.Web.Http;
using WebMatrix.WebData;

namespace cms.Controllers.Api
{
	[Authorize]
	public class ClientApiAuthController : WebApiControllerBase
	{
		public string GetUser()
		{
			var x = "kjbasdjkbasd" + WebSecurity.CurrentUserId.ToString();
			return x;
		}

		[AllowAnonymous]
		public string Login()
		{
			var x = "kjbasdjkbasd" + WebSecurity.CurrentUserId.ToString();
			return x;
		}
	
	}

}
