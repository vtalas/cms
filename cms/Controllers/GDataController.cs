using System.Web.Mvc;
using cms.Code;
using cms.Code.LinkAccounts;
using cms.Code.UserResources;

namespace cms.Controllers
{
	[Authorize]
	public class GDataController : CmsControllerBase
	{
		private const string ClientId = "568637062174-4s9b1pohf5p0hkk8o4frvesnhj8na7ug.apps.googleusercontent.com";
		private const string ClientSecret = "YxM0MTKCTSBV5vGlU05Yj63h";
		GdataJsonFileStorage storage()
		{
			var resources = UserResourcesManagerProvider.GetApplication(ApplicationId);

			var gg = new GdataJsonFileStorage(resources)
			{
				ClientId = ClientId,
				ClientSecret = ClientSecret
			};
			return gg;
		}

		public ActionResult Index()
		{
			var x = new GoogleDataOAuth2(storage());
			return View(x);
		}

		[HttpPost]
		public ActionResult RedirectToAuthenticate(GdataJsonFileStorage model)
		{
			var x = new GoogleDataOAuth2(model);
			return Redirect(x.OAuth2AuthorizationUrl());
		}

		[HttpGet]
		public ActionResult Authenticate(string code)
		{
			var s = storage();

			s.AccessCode = code;
			s.RedirectUrl = "http://localhost:62728/api/c78ee05e-1115-480b-9ab7-a3ab3c0f6643/GData/Authenticate";

			var x = new GoogleDataOAuth2(s);
			var param = x.GetAccessToken();

			return new EmptyResult();
		}
	}
}
