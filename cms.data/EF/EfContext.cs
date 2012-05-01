using System.Collections.Generic;
using System.Data.Entity;
using cms.data.Models;

namespace cms.data.EF
{
	public class EfContext :DbContext
	{
		public DbSet<ApplicationSetting> ApplicationSettings { get; set; }
		public DbSet<Grid> Grids { get; set; }
		public DbSet<GridElement> GridElements { get; set; }
	}

	public class DropAndCreate : IDatabaseInitializer<EfContext>
	{

		public void InitializeDatabase(EfContext context)
		{
			if (context.Database.Exists())
			{
				using (var ctx = new EfContext())
				{
					Database.SetInitializer<EfContext>(null);
					ctx.Database.ExecuteSqlCommand("ALTER DATABASE " + ctx.Database.Connection.Database + " SET SINGLE_USER WITH ROLLBACK IMMEDIATE");
				}
				context.Database.Delete();
			}
			context.Database.Create();
			this.Seed(context);
			context.SaveChanges();

		}
		protected virtual void Seed(EfContext context)
		{
			var application = new ApplicationSetting
			                  	{
			                  		Name = "testovaci aplikacka"
			                  	};

			context.ApplicationSettings.Add(application);

			var grids = new Grid()
			{
				Link = "gridSelementama",
				Name = "test page",
				GridElems = new List<GridElement>()
							            	{
												new GridElement{Content = "aaa",Line = 0,Position = 0,Width = 3,Type = "text"},
												new GridElement{Content = "aaa",Line = 0,Position = 3,Width = 3,Type = "text"},
												new GridElement{Content = "aaa",Line = 0,Position = 6,Width = 3,Type = "text"},
												new GridElement{Content = "aaa",Line = 2,Position = 5,Width = 3,Type = "text"},
												new GridElement{Content = "aaa",Line = 3,Position = 0,Width = 12,Type = "text"}
							            	}
			};

			context.Grids.Add(grids);
			context.Grids.Add(new Grid
			{
				Link = "sdakjs",
				Name = "grid Bez elementu"
			});

			context.SaveChanges();


		}

	}
}