﻿using System.Web.Mvc;
using Google.GData.Client;
using cms.Code;
using cms.Code.LinkAccounts;
using cms.Code.UserResources;

namespace cms.Controllers
{
	[Authorize]
	public class GDataController : CmsControllerBase
	{
		private GoogleDataOAuth2 GdataAuth { get; set; }
		private const string ClientId = "568637062174-4s9b1pohf5p0hkk8o4frvesnhj8na7ug.apps.googleusercontent.com";
		private const string ClientSecret = "YxM0MTKCTSBV5vGlU05Yj63h";
		
		protected override void Initialize(System.Web.Routing.RequestContext requestContext)
		{
			base.Initialize(requestContext);
			GdataAuth = new GoogleDataOAuth2(storage());
		}

		OAuth2ParametersStorageAbstract storage()
		{
			var resources = UserResourcesManagerProvider.GetApplication(ApplicationId);
			var parameters = new OAuth2Parameters()
				{
					ClientId = ClientId,
					ClientSecret = ClientSecret,
					Scope = "https://picasaweb.google.com/data",
					RedirectUri = "http://localhost:62728/api/c78ee05e-1115-480b-9ab7-a3ab3c0f6643/GData/Authenticate"
				};

			var gg = new OAuth2ParametersStorageJson(resources);
			gg.Load();
			return gg;
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
			GdataAuth.Storage.Parameters.RedirectUri = "http://localhost:62728/api/c78ee05e-1115-480b-9ab7-a3ab3c0f6643/GData/Authenticate";

			GdataAuth.GetAccessToken();

			return RedirectToAction("Index");
		}
	}
}
