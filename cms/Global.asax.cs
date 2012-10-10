using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using cms.Models;
using cms.shared;

namespace cms
{
	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801

	public class MvcApplication : System.Web.HttpApplication
	{

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
				new { controller = "admin", action = "Index", application = "00000000-0000-0000-0000-000000000000", id = UrlParameter.Optional } // Parameter defaults
			);

			//TODO: predelat api routes na tento 
			routes.MapRoute(
				"api", // Route name
				"api/{applicationId}/{controller}/{action}/{id}", // URL with parameters
				new { controller = "adminapi", action = "index", id = UrlParameter.Optional } // Parameter defaults
			);

			routes.MapRoute(
				"adminapi", // Route name
				"adminapi/{applicationId}/{action}/{id}", // URL with parameters
				new { controller = "adminapi", action = "error", id = UrlParameter.Optional } // Parameter defaults
			);

			routes.MapRoute(
				"ViewPage", // Route name
				"client/{application}/{link}", // URL with parameters
				new { controller = "Client", action = "ViewPage", link = UrlParameter.Optional }
			);

			routes.MapRoute(
				"clientApiJson", // Route name
				"client/json/{applicationId}/{link}", // URL with parameters
				new { controller = "ClientApi", action = "ViewPageJson", link = UrlParameter.Optional } // Parameter defaults
			);

			routes.MapRoute(
				"clientApi", // Route name
				"clientapi/{applicationId}/{action}/{id}", // URL with parameters
				new { controller = "ClientApi", action = "Index", id = UrlParameter.Optional } // Parameter defaults
			);



			routes.MapRoute(
				"Default", // Route name
				"{controller}/{action}/{id}", // URL with parameters
				new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
			);

		}

		protected void Application_Start()
		{
			var transforms = new IBundleTransform[] {new JsMinify()};

			var testbundle = new Bundle("~/Scripts/libs", transforms)
				.Include("~/Scripts/angular.js")
				.Include("~/Scripts/angular-ui.js")
				.IncludeDirectory("~/Scripts/", "*.js")
				.IncludeDirectory("~/Scripts/", "*.coffee");
				//.IncludeDirectory("~/Scripts/", "*.ts");

			var adminBundle = new Bundle("~/Scripts/admin", transforms)
				.IncludeDirectory("~/Scripts/Admin", "*.js")
				.IncludeDirectory("~/Scripts/Admin", "*.coffee");
			
			var controllers = new Bundle("~/Scripts/Controllers", transforms)
				.IncludeDirectory("~/Scripts/Controllers", "*.js")
				.IncludeDirectory("~/Scripts/Controllers", "*.coffee");


			BundleTable.Bundles.Add(testbundle);
			BundleTable.Bundles.Add(adminBundle);
			BundleTable.Bundles.Add(controllers);

			AreaRegistration.RegisterAllAreas();
			RegisterGlobalFilters(GlobalFilters.Filters);
			RegisterRoutes(RouteTable.Routes);
			SharedLayer.Init();
		}

		protected void Session_Start(Object sender, EventArgs e)
		{
			/*
			 When using cookie-based session state, ASP.NET does not allocate storage for session data until the Session object is used. As a result, a new session ID is generated for each page request until the session object is accessed. If your application requires a static session ID for the entire session, you can either implement the Session_Start method in the application's Global.asax file and store data in the Session object to fix the session ID, or you can use code in another part of your application to explicitly store data in the Session object.
			 */
			Session["init"] = 0;
		}
	}
}