﻿using System.Web.Http.Controllers;
using cms.Code.LinkAccounts;

namespace cms.Controllers.Api
{
	public class WebApiPicasaControllerBase : WebApiControllerBase
	{
		protected PicasaServiceFactory Picasa { get; set; }

		protected override void Initialize(HttpControllerContext requestContext)
		{
			base.Initialize(requestContext);
			var gdataAuth = new GoogleDataOAuth2(OAuth2ParametersStorageFactory.Storage(ApplicationId));
			Picasa = new PicasaServiceFactory(gdataAuth.GetRequestDataFactoryInstance("https://picasaweb.google.com/data"));
		}

	}
}