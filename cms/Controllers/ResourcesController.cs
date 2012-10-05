using System.IO;
using System.Web;
using System.Web.Mvc;
using BundleTransformer.Core;
using BundleTransformer.Core.FileSystem;
using BundleTransformer.Core.Web;
using cms.Code.UserResources;
using cms.Controllers.Api;

namespace cms.Controllers
{
	public class ResourcesController : ApiControllerBase
    {
		private IResourceManager Resources { get; set; }
		protected HttpApplicationInfo ApplicationInfo { get; set; }

		protected override void Initialize(System.Web.Routing.RequestContext requestContext)
		{
			base.Initialize(requestContext);

			Resources = UserResourcesManagerProvider.GetApplication(ApplicationId);

			Resources.IncludeDirectory("gridelements");
			Resources.IncludeDirectory("../_default/gridelements");
		}

		public ActionResult Index()
		{
			
			Response.ContentType = "text/javascript";
			Response.Write(Resources.RenderScripts());
			return new EmptyResult();
		}


		public ActionResult Scripts()
		{
			Response.ContentType = "text/javascript";
			Response.Write(Resources.RenderScripts());
			return new EmptyResult();
		}

		public ActionResult HtmlTemplates()
		{
			var response = Resources.RenderHtmlTemplates();
			Response.Write(response);
			return new EmptyResult();
		}

    }
}
