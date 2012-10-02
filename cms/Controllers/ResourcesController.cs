using System.IO;
using System.Resources;
using System.Web;
using System.Web.Mvc;
using BundleTransformer.Core;
using BundleTransformer.Core.Assets;
using BundleTransformer.Core.FileSystem;
using BundleTransformer.Core.Web;
using cms.Code.UserResources;
using cms.Controllers.Api;

namespace cms.Controllers
{
	public class ResourcesController : ApiControllerBase
    {

		public ActionResult Index()
		{
			
			var applicationInfo = new HttpApplicationInfo(VirtualPathUtility.ToAbsolute("~/"), 
				Path.Combine(Server.MapPath("~/"), "App_Data\\ApplicationData"));
			
			var files = (IFileSystemWrapper) BundleTransformerContext.Current.GetFileSystemWrapper();

			var res = UserResourceManager.Get(ApplicationId, applicationInfo, files);
			
			res.Include("aaa.js");

			Response.ContentType = "text/javascript";
			Response.Write(res.Combine());
			return new EmptyResult();
		}
		public ActionResult Js()
		{
			Response.Write(ApplicationId);
			Response.Write(" xxxas knaslkdn lkasnkd  ");
			return new EmptyResult();
		}

    }
}
