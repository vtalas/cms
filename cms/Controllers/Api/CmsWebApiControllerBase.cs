using System;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace cms.Controllers.Api
{
	/// <summary>
	/// 
	/// </summary>
	public class CmsWebApiControllerBase : ApiController
	{
		public Guid ApplicationId { get; private set; }

		protected override void Initialize(HttpControllerContext requestContext)
		{
			base.Initialize(requestContext);
			ApplicationId = new Guid(requestContext.RouteData.Values["applicationId"].ToString());
		}
	}
}