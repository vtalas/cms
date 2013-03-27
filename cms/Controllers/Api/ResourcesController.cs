using System.Web.Http.Controllers;
using BundleTransformer.Core.Web;
using cms.Code.UserResources;

namespace cms.Controllers.Api
{
	public class ResourcesController : WebApiControllerBase
    {
		private IResourceManager Resources { get; set; }
		protected HttpApplicationInfo ApplicationInfo { get; set; }

		protected override void Initialize(HttpControllerContext requestContext)
		{
			base.Initialize(requestContext);

			Resources = UserResourcesManagerProvider.GetApplication(ApplicationId);

			Resources.IncludeDirectory("pageElements");
			Resources.IncludeDirectory("menuElements");
			Resources.IncludeDirectory("../_default/pageElements");
			Resources.IncludeDirectory("../_default/menuElements");
		}

		//public ActionResult Index()
		//{
			
		//	Response.ContentType = "text/javascript";
		//	Response.Write(Resources.RenderScripts());
		//	return new EmptyResult();
		//}


		public string Scripts()
		{
			return Resources.RenderScripts();
		}

		public string GetHtmlTemplates()
		{
			return Resources.RenderHtmlTemplates();
		}

    }
}
