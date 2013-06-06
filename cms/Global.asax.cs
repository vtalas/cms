using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using OAuth2.Mvc.Module;
using cms.shared;

namespace cms
{
	public class MvcApplication : HttpApplication
	{
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new HandleErrorAttribute());
		}

		public static void RegisterRoutes(RouteCollection routes)
		{

			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute(
				"applicationAdmin",
				"render/{applicationId}/{controller}/{action}/{id}",
				new { controller = "admin", action = "Index", id = UrlParameter.Optional }
			);

			routes.MapHttpRoute(
				"clientApi",
				"clientapi/{applicationId}/{action}/{id}",
				new { controller = "ClientApi", action = "Index", id = RouteParameter.Optional, enableMvcOAuth = true }
			);

			routes.MapHttpRoute(
				"DefaultApi",
				"api/{applicationId}/{controller}/{action}/{id}",
				new { controller = "adminapi", action = "index", id = RouteParameter.Optional }
			);

			routes.MapRoute(
				"Default",
				"{controller}/{action}/{id}",
				new { controller = "Home", action = "Index", id = UrlParameter.Optional }
			);

		}

		protected void Application_Start()
		{
			data.Migrations.Configuration.MigrateToLatestVersion();

			var transforms = new IBundleTransform[] { new JsMinify() };
			var libs = new Bundle("~/Scripts/libs", transforms)
				.Include("~/Scripts/angular.js")
				.Include("~/Scripts/angular-ui.js")
				.IncludeDirectory("~/Scripts/", "*.js");
			//.IncludeDirectory("~/Scripts/", "*.ts");

			var adminBundle = new Bundle("~/Scripts/admin_scripts", transforms)
				.IncludeDirectory("~/Scripts/Admin", "*.js")
				.IncludeDirectory("~/Scripts/Admin", "*.coffee")
				.IncludeDirectory("~/Scripts/Controllers", "*.js")
				.IncludeDirectory("~/Scripts/Controllers", "*.coffee")
				.IncludeDirectory("~/Scripts/modules", "*.js");


			BundleTable.Bundles.Add(libs);
			BundleTable.Bundles.Add(adminBundle);

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