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

			GenerateGridPages(application);

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

		private void GenerateGridPages(ApplicationSetting application)
		{
			Context.Grids.Add(GridExt.CreateGrid(new Guid("c78ee05e-1115-480b-9ab7-a3ab3c0f6643"), application)
										.WithGridElement(
											GridExt.CreateGridElement("text")
												.WithResource("text", "český text")
												.WithResource("text", "english text", CultureEn)
												)
										.WithResource(SpecialResourceEnum.Link, "testPage_link")
										.WithResource("name", "testovaci stranka", CultureCs)
										.WithResource("name", "Test Page", CultureEn)  as Grid 
								);


			Context.Grids.Add(GridExt.CreateGrid(new Guid("aa8ee05e-1115-480b-9ab7-a3ab3c0f6643"), application)
										.WithResource(SpecialResourceEnum.Link, "bezelementu")
										.WithResource("name", "grid Bez elementu", CultureCs)
										.WithResource("name", "Without Any Element", CultureEn)
								);

			Context.Grids.Add(GridExt.CreateGrid(new Guid("ab8ee05e-1115-480b-9ab7-a3ab3c0f6643"), application)
										.WithResource(SpecialResourceEnum.Link, "gallery_1")
										.WithResource("name", "galerie 1", CultureCs)
										.WithResource("name", "gallery 1", CultureEn)
								);

			Context.Grids.Add(GridExt.CreateGrid(new Guid("ac8ee05e-1115-480b-9ab7-a3ab3c0f6643"), application)
										.WithResource(SpecialResourceEnum.Link, "gallery_1_sub_1", CultureCs)
										.WithResource("name", "subgalerie galerie 1", CultureCs)
								);

			Context.Grids.Add(GridExt.CreateGrid(new Guid("bc8ee05e-1115-480b-9ab7-a3ab3c0f6643"), application)
										.WithResource(SpecialResourceEnum.Link, "gallery_1_2", CultureCs)
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
			var rootitem = GridExt.CreateGridElement("menutext");
			
			var items = new List<GridElement>
				            {
								GridExt.CreateGridElement("menutext"),
								GridExt.CreateGridElement("menutext"),
								GridExt.CreateGridElement("menutext"),
								GridExt.CreateGridElement("menutext"),
								rootitem,
								GridExt.CreateGridElement("menutext").WithParent(rootitem),
								GridExt.CreateGridElement("menutext").WithParent(rootitem),
								GridExt.CreateGridElement("menutext").WithParent(rootitem),
				            };

			var menu = GridExt.CreateGrid(new Guid("eeeee05e-1115-480b-9ab7-a3ab3c0f6643"), application)
								.WithCategory(CategoryEnum.Menu)
								.WithGridElements(items)
								.WithResource(SpecialResourceEnum.Link, "test_menu_link", CultureCs)
								.WithResource("name", "test menu", CultureCs);

			Context.Grids.Add(menu);
		}
	}
}