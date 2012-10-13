using System;
using System.Collections.Generic;
using System.Configuration;
using WebMatrix.WebData;
using cms.data.EF.DataProvider;
using cms.data.Shared.Models;
using cms.shared;
using System.Linq;
using cms.data.Dtos;

namespace cms.data.EF.Initializers
{
	public static class Chuj
	{
		public static List<Resource> EmptyResource()
		{
			return new List<Resource>();
		}

		public static List<Resource> WithResource(this List<Resource> source, string key, string value, Guid parentId, string culture, int id = 0)
		{
			var res = JsonDataEfHelpers.GetResource(key, value, parentId, culture, id);
			source.Add(res);
			return source;
		}

	}
	public class SampleData
	{
		protected EfContext Context { get; set; }

		//static readonly ILog Log = LogManager.GetLogger<SampleData>();

		public SampleData(EfContext context)
		{
			Context = context;
		}


		public void Generate()
		{
			var application = new ApplicationSetting { Name = "test1", Id = new Guid("c78ee05e-1115-480b-9ab7-a3ab3c0f6643") };

			GenerateUsers();

			Context.ApplicationSettings.Add(application);
			var userid = WebSecurity.GetUserId("admin");
			var user = Context.UserProfile.Single(x => x.Id == userid);
			application.Users.Add(user);

			GenerateGrids(application);

			Context.TemplateTypes.Add(new TemplateType { Name = "novinka" });
			Context.TemplateTypes.Add(new TemplateType { Name = "text" });
			Context.TemplateTypes.Add(new TemplateType { Name = "reference" });


			GenerateMenus(application);

			string defualtjsondata = ConfigurationManager.AppSettings.Get("DefaultJsonBootstrap");
			//TODO pridat log4net
			//if (!File.Exists(defualtjsondata))
			//{
			//    Log.ErrorFormat("{0} not exist..", defualtjsondata);
			//}


			//TODO: opravit
			//Context.Bootstrapgenerators.Add(new Bootstrapgenerator
			//{
			//    Data = FileExts.GetContent(defualtjsondata),
			//    Name = "default",
			//    Status = Boostrapperstatus.Default
			//});


			Context.SaveChanges();
		}



		private void GenerateGrids(ApplicationSetting application)
		{
			var culture = "cs";
			var grids = new Grid
							{
								Id = new Guid("c78ee05e-1115-480b-9ab7-a3ab3c0f6643"),
								Name = "test page",
								GridElements = new List<GridElement>
						                           {
							                           new GridElement {Content = "aaaaaaaa aaa", Position = 0, Width = 12, Type = "text"}
						                           },
								ApplicationSettings = application
							};
			grids.AddResource("link", "linkTestPage");

			Context.Grids.Add(grids);
			Context.Grids.Add(new Grid
								  {
									  Id = new Guid("aa8ee05e-1115-480b-9ab7-a3ab3c0f6643"),
									  Name = "grid Bez elementu",
									  ApplicationSettings = application,
									  Resources = Chuj.EmptyResource()
										 .WithResource("link", "bezelementu", new Guid("aa8ee05e-1115-480b-9ab7-a3ab3c0f6643"), culture)
								  });
			Context.Grids.Add(new Grid
								  {
									  Id = new Guid("ab8ee05e-1115-480b-9ab7-a3ab3c0f6643"),
									  Name = "gallery 1 ",
									  ApplicationSettings = application,
									  Resources = Chuj.EmptyResource()
										 .WithResource("link", "gallery_1", new Guid("ab8ee05e-1115-480b-9ab7-a3ab3c0f6643"), culture)
								  });
			Context.Grids.Add(new Grid
								  {
									  Id = new Guid("ac8ee05e-1115-480b-9ab7-a3ab3c0f6643"),
									  Name = "gallery 1 sub 1",
									  ApplicationSettings = application,
									  Resources = Chuj.EmptyResource()
										 .WithResource("link", "gallery_1", new Guid("ac8ee05e-1115-480b-9ab7-a3ab3c0f6643"), culture)
								  });
			Context.Grids.Add(new Grid
								  {
									  Id = new Guid("bc8ee05e-1115-480b-9ab7-a3ab3c0f6643"),
									  Name = "gallery 1 sub 2 ",
									  ApplicationSettings = application,
									  Resources = Chuj.EmptyResource()
										 .WithResource("link", "gallery_1", new Guid("bc8ee05e-1115-480b-9ab7-a3ab3c0f6643"), culture)
								  });
		}

		private void GenerateUsers()
		{
			SecurityProvider.EnsureInitialized(true);
			WebSecurity.CreateUserAndAccount("admin", "a");
			WebSecurity.CreateUserAndAccount("lades", "a");
			WebSecurity.CreateUserAndAccount("pepa", "a");
		}

		private void GenerateMenus(ApplicationSetting application)
		{
			var items = new List<GridElement>
				            {
					            new GridElement {Parent = null, Content = "", Id = Guid.NewGuid()},
					            new GridElement {Parent = null, Content = "", Id = Guid.NewGuid()},
					            new GridElement {Parent = null, Content = "", Id = Guid.NewGuid()},
					            new GridElement {Parent = null, Content = "", Id = Guid.NewGuid()}
				            };

			//add rootitems 

			var rootitem = new GridElement { Parent = null, Content = "", Id = Guid.NewGuid() };
			items.Add(rootitem);

			items.Add(new GridElement { Parent = rootitem, Content = "", Id = Guid.NewGuid() });
			items.Add(new GridElement { Parent = rootitem, Content = "", Id = Guid.NewGuid() });
			items.Add(new GridElement { Parent = rootitem, Content = "", Id = Guid.NewGuid() });
			items.Add(new GridElement { Parent = rootitem, Content = "", Id = Guid.NewGuid() });
			items.Add(new GridElement { Parent = rootitem, Content = "", Id = Guid.NewGuid() });

			var menu = new Grid
						   {
							   Id = new Guid("eeeee05e-1115-480b-9ab7-a3ab3c0f6643"),
							   Resources = Chuj.EmptyResource()
								   .WithResource("link", "bezelementu", new Guid("eeeee05e-1115-480b-9ab7-a3ab3c0f6643"), "cs"),
							   Name = "test menu",
							   ApplicationSettings = application,
							   Category = CategoryEnum.Menu,
							   GridElements = items
						   };

			Context.Grids.Add(menu);
		}
	}
}