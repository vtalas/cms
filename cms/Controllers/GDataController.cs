using System;
using System.Web.Mvc;
using Google.GData.Client;
using cms.Code;
using cms.Code.LinkAccounts;

namespace cms.Controllers
{
	[Authorize(Roles = "admin")]
	public class GDataController : CmsControllerBase
	{
		private Func<GoogleDataOAuth2Service> _gdataAuthFnc;
		private GoogleDataOAuth2Service GdataAuth { get; set; }

		protected override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			GdataAuth = _gdataAuthFnc.Invoke();
			if (GdataAuth.IsAuhtorized() && !GdataAuth.IsValid())
			{
				try
				{
					GdataAuth.RefreshAccessToken();
				}
				catch (Exception ex)
				{
					
				}
			}
		}

		protected override void Initialize(System.Web.Routing.RequestContext requestContext)
		{
			base.Initialize(requestContext);
			_gdataAuthFnc = () => new GoogleDataOAuth2Service(OAuth2ParametersStorageFactory.StorageDatabase(SessionProvider));
		}

		public ActionResult Index()
		{
			return View(_gdataAuthFnc.Invoke());
		}

		[HttpPost]
		public ActionResult RedirectToAuthenticate(OAuth2Parameters model)
		{
			GdataAuth.Storage.Parameters = model;

			var u = HttpContext.Request.Url;
			
			GdataAuth.Storage.Parameters.RedirectUri = string.Format("{0}://{1}/GdataAuth/Authenticate", u.Scheme, u.Authority);
			GdataAuth.Storage.Parameters.State = ApplicationId.ToString();

			return Redirect(GdataAuth.OAuth2AuthorizationUrl());
		}

		[HttpGet]
		public ActionResult Authenticate(string code)
		{
			GdataAuth.Storage.Parameters.AccessCode = code;
			var u = HttpContext.Request.Url;
			
			GdataAuth.Storage.Parameters.RedirectUri = string.Format("{0}://{1}/GdataAuth/Authenticate", u.Scheme, u.Authority);
			
			GdataAuth.GetAccessToken();
			if (GdataAuth.IsAuhtorized())
			{
				TempData["message"] = "Úspěšně připojeno na Google Picasa.";
			}
			return RedirectToAction("Index");
		}
	
		[HttpPost]
		public ActionResult Revoke(string code)
		{
			GdataAuth.RevokeToken();
			return RedirectToAction("Index");
		}
		
		[HttpGet]
		public ActionResult RefreshToken()
		{
			GdataAuth.RefreshAccessToken();
			return RedirectToAction("Index");
		}
	}

	public enum ViewStatus
	{
		Success,
		Error,
		Info

	}
}
