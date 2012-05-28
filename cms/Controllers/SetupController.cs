﻿using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Web.Mvc;
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
			using (var a = new EfContext())
			{
				Database.SetInitializer(new DropAndCreateTables());
				a.ApplicationSettings.ToList();
			}
			return View();

		}
		public ActionResult MigrateDatabaseToLatestVersion()
		{
			using (var a = new EfContext())
			{
				Database.SetInitializer(new MigrateDatabaseToLatestVersion<EfContext,data.Migrations.Configuration>());
				a.ApplicationSettings.ToList();
			}
			return View();
		}

    	public ActionResult Index()
        {
			return View();
        }

    }
}
