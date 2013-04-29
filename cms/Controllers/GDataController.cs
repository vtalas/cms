using System.Web.Mvc;
using Google.GData.Client;
using cms.Code;
using cms.Code.LinkAccounts;

namespace cms.Controllers
{
	[Authorize]
	public class GDataController : CmsControllerBase
	{
		private GoogleDataOAuth2Service GdataAuth { get; set; }
		
		protected override void Initialize(System.Web.Routing.RequestContext requestContext)
		{
			base.Initialize(requestContext);
			GdataAuth = new GoogleDataOAuth2Service(OAuth2ParametersStorageFactory.StorageDatabase(SessionProvider));
		
		}
		
		public ActionResult Index()
		{
			return View(GdataAuth);
		}

		[HttpPost]
		public ActionResult RedirectToAuthenticate(OAuth2Parameters model)
		{
			GdataAuth.Storage.Parameters = model;
			return Redirect(GdataAuth.OAuth2AuthorizationUrl());
		}

		[HttpGet]
		public ActionResult Authenticate(string code)
		{
			GdataAuth.Storage.Parameters.AccessCode = code;
			GdataAuth.Storage.Parameters.RedirectUri = "http://localhost:62728/render/c78ee05e-1115-480b-9ab7-a3ab3c0f6643/Gdata/Authenticate";

			GdataAuth.GetAccessToken();

			return RedirectToAction("Index");
		}

		[HttpGet]
		public ActionResult Revoke(string code)
		{
			//GdataAuth.GetAccessToken();
			return RedirectToAction("Index");
		}
		
		[HttpGet]
		public ActionResult RefreshToken()
		{
			GdataAuth.RefreshAccessToken();
			return RedirectToAction("Index");
		}
	}
}
