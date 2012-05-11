using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using NodeAssets.AspNet;
using NodeAssets.Core;
using NodeAssets.Core.Commands;
using NodeAssets.Core.Compilers;
using cms.Models;

namespace cms
{
	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801

	public class MvcApplication : System.Web.HttpApplication
	{
		private Dictionary<string, ICompiler> _compilers { get; set; }
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new HandleErrorAttribute());
		}

		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute(
				"applicationAdmin", // Route name
				"admin/{application}/{action}/{id}", // URL with parameters
				new { controller = "admin", action = "Index", application="", id = UrlParameter.Optional } // Parameter defaults
			);
			routes.MapRoute(
				"adminapi", // Route name
				"adminapi/{application}/{action}/{id}", // URL with parameters
				new { controller = "adminapi", action = "error", application="", id = UrlParameter.Optional } // Parameter defaults
			);
			routes.MapRoute(
				"client", // Route name
				"client/{application}/{action}/{id}", // URL with parameters
				new { controller = "Client", action = "Index", id = UrlParameter.Optional } // Parameter defaults
			);
			routes.MapRoute(
				"clientApi", // Route name
				"clientapi/{application}/{action}/{id}", // URL with parameters
				new { controller = "ClientApi", action = "Index", id = UrlParameter.Optional } // Parameter defaults
			);
			routes.MapRoute(
				"Default", // Route name
				"{controller}/{action}/{id}", // URL with parameters
				new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
			);

		}
		//public ICompilerConfiguration WithDefaultConfiguration(string nodeWorkspacePath)
		//{
		//    var passthrough = new PassthroughCompiler();
		//    var executor = new NodeExecutor(nodeWorkspacePath);

		//    // File compilers
		//    _compilers = new Dictionary<string, ICompiler>();
		//    _compilers.Add(".coffee", new CoffeeCompiler(executor));
		//    _compilers.Add(".js", passthrough);
		//    _compilers.Add(".styl", new StylusCompiler(executor, true));
		//    _compilers.Add(".css", passthrough);

		//    // Minification providers
		//    _compilers.Add(".js.min", new UglifyJSCompiler(executor));
		//    _compilers.Add(".css.min", new CssoCompiler(executor));

		//    //return this;
		//}

		
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();

//#if DEBUG
//    bool isProd = false;
//#else
//    bool isProd = true;
//#endif
//            Assets
//                .Initialise(config => config
//                    .ConfigureCompilers(compilers => compilers.WithDefaultConfiguration(Server.MapPath("~/Node")))
//                    .ConfigureSourceManager(source => source.UseDefaultConfiguration(Server.MapPath("~/built"), isProd))
//                    .Cache(isProd)
//                    .Compress(isProd)
//                    .LiveCss(!isProd))
//                .SetupCssPile(pile => pile
//                    .AddFile(Server.MapPath("~/Content/Site.css")))
//                .SetupJavascriptPile(pile =>
//                {
//                    pile.AddFile(Server.MapPath("~/Scripts/jquery-1.6.4.js"));

//                    if (!isProd)
//                    {
//                        pile.AddFile(Server.MapPath("~/Scripts/jquery.signalR.js"));
//                    }

//                    return pile;
//                })
//                .PrepareRoutes(RouteTable.Routes);

			RegisterGlobalFilters(GlobalFilters.Filters);
			RegisterRoutes(RouteTable.Routes);
		}


	}
}