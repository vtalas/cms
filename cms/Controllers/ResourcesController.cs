using System.Web.Mvc;
using BundleTransformer.Core.Assets;
using cms.Controllers.Api;

namespace cms.Controllers
{
	public class ResourcesController : ApiControllerBase
    {

		public ActionResult Index()
		{
			var x = new BundleTransformer.CoffeeScript.Translators.CoffeeScriptTranslator();
			var xxx = x.Translate(new Asset(Server.MapPath("~/Scripts/angular-templates-ext.coffee")));
			


			Response.ContentType = "text/javascript";
			Response.Write("alert('landlans')");
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
