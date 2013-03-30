using System.Data.Entity;
using System.Web.Mvc;
using WebMatrix.WebData;
using cms.data.EF;
using System.Linq;
using cms.data.EF.Initializers;

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
			using (var a = new EfContext())
			{
				Database.SetInitializer(new MigrateDatabaseToLatestVersion<EfContext, data.Migrations.Configuration>());
				Response.Write(a.ApplicationSettings.ToList());
			}
			return View();
		}
    	public ActionResult Index()
        {
			return View();
        }
    }
}
