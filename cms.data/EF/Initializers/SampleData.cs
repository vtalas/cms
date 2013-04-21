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
	public class SampleData : ObjectBuilder
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
			var gdataSettings = new ApplicationSettingStorage()
				{
					AppliceSetting = application,
					Key = "GdataPicasa.json",
					Value =
						"{\"ClientId\":\"568637062174-4s9b1pohf5p0hkk8o4frvesnhj8na7ug.apps.googleusercontent.com\",\"ClientSecret\":\"YxM0MTKCTSBV5vGlU05Yj63h\",\"RedirectUri\":\"http://localhost:62728/render/c78ee05e-1115-480b-9ab7-a3ab3c0f6643/Gdata/Authenticate\",\"AccessType\":\"offline\",\"ResponseType\":\"code\",\"ApprovalPrompt\":\"auto\",\"State\":null,\"Scope\":\"https://picasaweb.google.com/data\",\"TokenUri\":\"https://accounts.google.com/o/oauth2/token\",\"AuthUri\":\"https://accounts.google.com/o/oauth2/auth\",\"AccessCode\":\"4/QLbO_XmRyWhrLhCQZfeFBw_tOEO-.YjhDDUv4vvASOl05ti8ZT3aR9rZ2ewI\",\"AccessToken\":\"ya29.AHES6ZRHtQgmJdwI83C1ZGLfXcFT1UpO8KFaQoxewvZslPo\",\"TokenType\":\"Bearer\",\"RefreshToken\":\"1/JjvMzAvTWDI1jQ3SJ-tL-W2xfMGgKCNpfnPFC7YLXZQ\",\"TokenExpiry\":\"2013-03-30T14:50:52.0350746+01:00\"}"
				};

			GenerateUsers();

			Context.ApplicationSettings.Add(application);
			Context.ApplicationSettingStorage.Add(gdataSettings);
			var userid = WebSecurity.GetUserId("admin");
			var user = Context.UserProfile.Single(x => x.Id == userid);
			application.Users.Add(user);

			GenerateGridPages(application);

			Context.TemplateTypes.Add(new TemplateType { Name = "novinka" });
			Context.TemplateTypes.Add(new TemplateType { Name = "text" });
			Context.TemplateTypes.Add(new TemplateType { Name = "reference" });

//			GenerateMenus(application);

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
			Context.Grids.Add(AGrid(new Guid("c78ee05e-1115-480b-9ab7-a3ab3c0f6643"), application)
										.WithGridElement(
											AGridElement("text", application)
												.WithResource("text", "český text")
												.WithResource("text", "english text", CultureEn)
												)
										.WithResource(SpecialResourceEnum.Link, "testPage_link")
										.WithResource("name", "testovaci stranka", CultureCs)
										.WithResource("name", "Test Page", CultureEn)
								);

			Context.Grids.Add(AGrid(new Guid("aa8ee05e-1115-480b-9ab7-a3ab3c0f6643"), application)
										.WithResource(SpecialResourceEnum.Link, "bezelementu")
										.WithResource("name", "grid Bez elementu", CultureCs)
										.WithResource("name", "Without Any Element", CultureEn)
								);

			Context.Grids.Add(AGrid(new Guid("ab8ee05e-1115-480b-9ab7-a3ab3c0f6643"), application)
										.WithResource(SpecialResourceEnum.Link, "gallery_1")
										.WithResource("name", "galerie 1", CultureCs)
										.WithResource("name", "gallery 1", CultureEn)
								);

			Context.Grids.Add(AGrid(new Guid("ac8ee05e-1115-480b-9ab7-a3ab3c0f6643"), application)
										.WithResource(SpecialResourceEnum.Link, "gallery_1_sub_1", CultureCs)
										.WithResource("name", "subgalerie galerie 1", CultureCs)
								);

			Context.Grids.Add(AGrid(new Guid("bc8ee05e-1115-480b-9ab7-a3ab3c0f6643"), application)
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
			var rootitem = AGridElement("menutext", application, position: 2);
			
			var items = new List<GridElement>
				            {
								AGridElement("menutext", application, position: 0),
								AGridElement("menutext", application, position: 1),
								AGridElement("menutext", application, position: 3),
								AGridElement("menutext", application, position: 4),
								rootitem,
								AGridElement("menutext", application, position: 0).WithParent(rootitem),
								AGridElement("menutext", application, position: 1).WithParent(rootitem),
								AGridElement("menutext", application, position: 2).WithParent(rootitem),
				            };

			var menu = AGrid(new Guid("eeeee05e-1115-480b-9ab7-a3ab3c0f6643"), application)
								.WithCategory(CategoryEnum.Menu)
								.WithGridElements(items)
								.WithResource(SpecialResourceEnum.Link, "test_menu_link", CultureCs)
								.WithResource("name", "test menu", CultureCs);

			Context.Grids.Add(menu);
		}
	}
}