using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using cms.data.EF.Bootstrap;
using cms.data.Shared;
using cms.data.Shared.Models;

namespace cms.data.EF.Initializers
{
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
		//static readonly ILog Log = LogManager.GetLogger<SampleData>();

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


			string defualtjsondata = ConfigurationManager.AppSettings.Get("DefaultJsonBootstrap");
			//TODO pridat log4net
			//if (!File.Exists(defualtjsondata))
			//{
			//    Log.ErrorFormat("{0} not exist..", defualtjsondata);
			//}

			Context.Bootstrapgenerators.Add(new Bootstrapgenerator
			{
				Data = FileExts.GetContent(defualtjsondata),
				Name = "default",
				Status = Boostrapperstatus.Default
			});


			Context.SaveChanges();

		}

	}

}