using System.Linq;

namespace cms.data.Migrations
{
	using System.Data.Entity.Migrations;

	public sealed class Configuration : DbMigrationsConfiguration<EF.EfContext>
	{
		public Configuration()
		{
			AutomaticMigrationsEnabled = true;
		}

		public static void MigrateToLatestVersion()
		{
			var configuration = new Configuration();
			var migrator = new DbMigrator(configuration);

			var pending = migrator.GetPendingMigrations();

			if (pending.Any())
			{
				migrator.Update();
			}
		}

		protected override void Seed(EF.EfContext context)
		{
			//context.Database.ExecuteSqlCommand("ALTER TABLE UserProfile ADD CONSTRAINT DB_ApplicationUser DEFAULT 0 FOR ApplicationUser");

			//  This method will be called after migrating to the latest version.

			//  You can use the DbSet<T>.AddOrUpdate() helper extension method 
			//  to avoid creating duplicate seed data. E.g.
			//
			//    context.People.AddOrUpdate(
			//      p => p.FullName,
			//      new Person { FullName = "Andrew Peters" },
			//      new Person { FullName = "Brice Lambson" },
			//      new Person { FullName = "Rowan Miller" }
			//    );
			//
		}
	}
}
