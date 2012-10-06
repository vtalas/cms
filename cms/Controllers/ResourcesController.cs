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

			ApplicationInfo = new HttpApplicationInfo(VirtualPathUtility.ToAbsolute("~/"),
				Path.Combine(Server.MapPath("~/"), "App_Data\\ApplicationData"));

			var files = (IFileSystemWrapper)BundleTransformerContext.Current.GetFileSystemWrapper();
			Resources = UserResourceManager.Get(ApplicationId, ApplicationInfo, files);

			Resources.IncludeDirectory("gridelements");
			Resources.IncludeDirectory("../_default/gridelements");
		}


		public ActionResult Index()
		{
			
			Resources.Include("aaa.js");

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
			Response.Write(Resources.RenderHtmlTemplates());
			return new EmptyResult();
		}

    }
}
