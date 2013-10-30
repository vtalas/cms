using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;
using cms.data.EF;
using System.Linq;
using cms.data.EF.Initializers;
using cms.data.Migrations;

namespace cms.Controllers
{
	public class SetupController : Controller
	{
		public ActionResult DropAndCreateTables()
		{
			Database.SetInitializer(new DropAndCreateTables());
			using (var a = new EfContext())
			{
				Response.Write(a.ApplicationSettings.ToList()[0].Id);
				Response.Write(" \n");
				Response.Write(WebSecurity.Initialized);
			}
			return View();
		}

		public ActionResult DropAndCreateAlwaysForce()
		{
			Database.SetInitializer(new DropAndCreateAlwaysForce());
			//			Database.Delete("")

			using (var a = new EfContext())
			{
				Xxx.DeleteDatabaseData();
				new DropAndCreateAlwaysForce().InitializeDatabase(a);
				SecurityProvider.EnsureInitialized(true);
				new SampleData(a).Generate();

				Response.Write(a.ApplicationSettings.ToList()[0].Id);
				Response.Write(" \n");
				Response.Write(WebSecurity.Initialized);
			}
			return RedirectToAction("Index");
		}

		public ActionResult MigrateDatabaseToLatestVersion()
		{
			var configuration = new Configuration();
			var migrator = new DbMigrator(configuration);

			
			var pending = migrator.GetPendingMigrations();
			
			if (pending.Any())
			{
				Response.Write("Updated");
				migrator.Update();
			}

			Change1();


//			using (var a = new EfContext())
//			{
//				//Database.SetInitializer(new MigrateDatabaseToLatestVersion<EfContext, data.Migrations.Configuration>());
//				Response.Write(a.ApplicationSettings.ToList());
//			}
			return View();
		}

		void Change1()
		{
			SecurityProvider.EnsureInitialized();
			if (!Roles.RoleExists("superuser"))
			{
				Roles.CreateRole("superuser");
			}
			if (!Roles.IsUserInRole("admin", "superuser"))
			{
				Roles.AddUserToRole("admin", "superuser");
			}
	
		}

		public ActionResult Index()
		{
			return View();
		}
	}
}
