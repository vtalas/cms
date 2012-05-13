using System.Collections.Generic;
using System.Data.Entity;
using cms.data.Models;

namespace cms.data.EF
{
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
			                  		Name = "test1"
			                  	};

			context.ApplicationSettings.Add(application);

			var grids = new Grid()
			            	{
			            		Id = 1,
								Link = "linkTestPage",
			            		Name = "test page",
			            		GridElements = new List<GridElement>()
			            		               	{
			            		               		new GridElement{Content = "aaa",Line = 0,Position = 0,Width = 3,Type = "text"},
			            		               		new GridElement{Content = "aaa",Line = 0,Position = 3,Width = 3,Type = "text"},
			            		               		new GridElement{Content = "aaa",Line = 0,Position = 6,Width = 3,Type = "text"},
			            		               		new GridElement{Content = "aaa",Line = 2,Position = 5,Width = 3,Type = "text"},
			            		               		new GridElement{Content = "aaa",Line = 3,Position = 0,Width = 12,Type = "text"}
			            		               	},
								ApplicationSettings = application

							};

			context.Grids.Add(grids);
			context.Grids.Add(new Grid
			                  	{
			                  		Id = 2,
									Link = "sdakjs",
			                  		Name = "grid Bez elementu"
			                  	});

			context.TemplateTypes.Add(new TemplateType { Name = "novinka" });
			context.TemplateTypes.Add(new TemplateType { Name = "text" });

			context.SaveChanges();
		}

	}
}