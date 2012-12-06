using System;
using System.Collections.Generic;
using System.Configuration;
using WebMatrix.WebData;
using cms.data.EF.Repository;
using cms.data.Shared.Models;
using cms.shared;
using System.Linq;

namespace cms.data.EF.Initializers
{
	public static class Chuj
	{
		public static Grid WithResource(this Grid source, string key, string value, int id = 0)
		{
			return source.WithResource(key, value, SharedLayer.Culture, id);
		}
		public static Grid WithResource(this Grid source, string key, string value,string culture, int id = 0)
		{
			var res = JsonDataEfHelpers.GetResource(key, value, source.Id, culture, id);
			source.Resources.Add(res);
			return source;
		}

		public static Grid WithCategory(this Grid source, string category)
		{
			source.Category = category;
			return source;
		}

		public static Grid WithGridElements(this Grid source, List<GridElement> gridElements)
		{
			source.GridElements = gridElements;
			return source;
		}

		public static Grid WithGridElement(this Grid source, GridElement gridElement)
		{
			source.GridElements.Add(gridElement);
			return source;
		}

		public static Grid CreateGrid(Guid id, ApplicationSetting application)
		{
			var grid = new Grid
				{
					Id = id,
					ApplicationSettings = application,
				};
			return grid;
		}

	}
	public class SampleData
	{
		protected EfContext Context { get; set; }
		private const string Culture = "cs";

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

			Context.Grids.Add(Chuj.CreateGrid(new Guid("c78ee05e-1115-480b-9ab7-a3ab3c0f6643"), application)
										.WithGridElement(new GridElement { Content = "", Position = 0, Width = 12, Type = "text" })
										.WithResource("link", "testPage_link", Culture)
										.WithResource("name", "testovaci stranka", Culture)
								);

			Context.Grids.Add(Chuj.CreateGrid(new Guid("aa8ee05e-1115-480b-9ab7-a3ab3c0f6643"), application)
										.WithResource("link", "bezelementu", Culture)
										.WithResource("name", "grid Bez elementu", Culture)
								);

			Context.Grids.Add(Chuj.CreateGrid(new Guid("ab8ee05e-1115-480b-9ab7-a3ab3c0f6643"), application)
										.WithResource("link", "gallery_1", Culture)
										.WithResource("name", "galerie 1", Culture)
								);

			Context.Grids.Add(Chuj.CreateGrid(new Guid("ac8ee05e-1115-480b-9ab7-a3ab3c0f6643"), application)
										.WithResource("link", "gallery_1_sub_1", Culture)
										.WithResource("name", "subgalerie galerie 1", Culture)
								);

			Context.Grids.Add(Chuj.CreateGrid(new Guid("bc8ee05e-1115-480b-9ab7-a3ab3c0f6643"), application)
										.WithResource("link", "gallery_1_2", Culture)
										.WithResource("name", "gallery 1 sub 2 ", Culture)
								);
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

			var menu = Chuj.CreateGrid(new Guid("eeeee05e-1115-480b-9ab7-a3ab3c0f6643"), application)
								.WithCategory(CategoryEnum.Menu)
								.WithGridElements(items)
								.WithResource("link", "test_menu_link", Culture)
								.WithResource("name", "test menu", Culture);

			Context.Grids.Add(menu);
		}
	}
}