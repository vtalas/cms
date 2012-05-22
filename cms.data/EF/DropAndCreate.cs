using System.Collections.Generic;
using System.Data.Entity;
using cms.data.Models;

namespace cms.data.EF
{
	public class CreateIfNotExists : IDatabaseInitializer<EfContext>
	{
		public void InitializeDatabase(EfContext context)
		{
			if (!context.Database.Exists())
			{
				context.Database.Create();
				Seed(context);
				context.SaveChanges();
			}
		}

		protected virtual void Seed(EfContext context)
		{
			var sampledata = new SampleData(context);
			sampledata.Generate();
		}
	}
	public class DropAndCreate : IDatabaseInitializer<EfContext>
	{

		public void InitializeDatabase(EfContext context)
		{
			if (context.Database.Exists())
			{
				Database.SetInitializer<EfContext>(null);
				context.Database.ExecuteSqlCommand("ALTER DATABASE " + context.Database.Connection.Database + " SET SINGLE_USER WITH ROLLBACK IMMEDIATE");
				context.Database.Delete();
			}
			context.Database.Create();

			Seed(context);
			context.SaveChanges();

		}
		protected virtual void Seed(EfContext context)
		{
			var sampledata = new SampleData(context);
			sampledata.Generate();
		}

	}

	public class SampleData
	{
		public SampleData(EfContext context)
		{
			Context = context;
		}

		protected EfContext Context { get; set; }

		public void Generate()
		{
			var application = new ApplicationSetting
			                  	{
			                  		Name = "test1"
			                  	};

			Context.ApplicationSettings.Add(application);

			var grids = new Grid()
			            	{
			            		Id = 1,
			            		Link = "linkTestPage",
			            		Name = "test page",
			            		GridElements = new List<GridElement>()
			            		               	{
													//new GridElement{Content = "aaa",Line = 0,Position = 0,Width = 3,Type = "text"},
													//new GridElement{Content = "aaa",Line = 0,Position = 3,Width = 3,Type = "text"},
													//new GridElement{Content = "aaa",Line = 0,Position = 6,Width = 3,Type = "text"},
													//new GridElement{Content = "aaa",Line = 2,Position = 5,Width = 3,Type = "text"},
													//new GridElement{Content = "aaa",Line = 3,Position = 0,Width = 12,Type = "text"}
													new GridElement{Content = "akjhasbdsa", Line = 1,Width = 12,Type = "text"}

			            		               	},
			            		ApplicationSettings = application

			            	};

			Context.Grids.Add(grids);
			Context.Grids.Add(new Grid
			                  	{
			                  		Id = 2,
			                  		Link = "sdakjs",
			                  		Name = "grid Bez elementu",
									ApplicationSettings = application
			                  	});

			Context.TemplateTypes.Add(new TemplateType {Name = "novinka"});
			Context.TemplateTypes.Add(new TemplateType {Name = "text"});

			Context.SaveChanges();

		}

	}

}