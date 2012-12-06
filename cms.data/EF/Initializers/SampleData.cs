using System;
using System.Collections.Generic;
using System.Configuration;
using WebMatrix.WebData;
using cms.data.Extensions;
using cms.data.Shared.Models;
using cms.shared;
using System.Linq;

namespace cms.data.EF.Initializers
{
	public class SampleData
	{
		protected EfContext Context { get; set; }
		private const string CultureCs = "cs";
		private const string CultureEn = "en";

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

			Context.Grids.Add(GridExtensions.CreateGrid(new Guid("c78ee05e-1115-480b-9ab7-a3ab3c0f6643"), application)
										.WithGridElement(new GridElement { Content = "", Position = 0, Width = 12, Type = "text" })
										.WithResource("link", "testPage_link", CultureCs)
										.WithResource("name", "testovaci stranka", CultureCs)
										.WithResource("link", "testPage_link", CultureEn)
										.WithResource("name", "Test Page", CultureEn)
								);

			Context.Grids.Add(GridExtensions.CreateGrid(new Guid("aa8ee05e-1115-480b-9ab7-a3ab3c0f6643"), application)
										.WithResource("link", "bezelementu", CultureCs)
										.WithResource("name", "grid Bez elementu", CultureCs)
										.WithResource("link", "without_any_element", CultureEn)
										.WithResource("name", "Without Any Element", CultureEn)
								);

			Context.Grids.Add(GridExtensions.CreateGrid(new Guid("ab8ee05e-1115-480b-9ab7-a3ab3c0f6643"), application)
										.WithResource("link", "gallery_1", CultureCs)
										.WithResource("name", "galerie 1", CultureCs)
								);

			Context.Grids.Add(GridExtensions.CreateGrid(new Guid("ac8ee05e-1115-480b-9ab7-a3ab3c0f6643"), application)
										.WithResource("link", "gallery_1_sub_1", CultureCs)
										.WithResource("name", "subgalerie galerie 1", CultureCs)
								);

			Context.Grids.Add(GridExtensions.CreateGrid(new Guid("bc8ee05e-1115-480b-9ab7-a3ab3c0f6643"), application)
										.WithResource("link", "gallery_1_2", CultureCs)
										.WithResource("name", "gallery 1 sub 2 ", CultureCs)
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

			var menu = GridExtensions.CreateGrid(new Guid("eeeee05e-1115-480b-9ab7-a3ab3c0f6643"), application)
								.WithCategory(CategoryEnum.Menu)
								.WithGridElements(items)
								.WithResource("link", "test_menu_link", CultureCs)
								.WithResource("name", "test menu", CultureCs);

			Context.Grids.Add(menu);
		}
	}
}