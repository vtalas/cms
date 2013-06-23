using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Security;
using WebMatrix.WebData;
using cms.data.Extensions;
using cms.data.Shared;
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
			SecurityProvider.EnsureInitialized(true);
		}
		
		public ApplicationSetting CreateApplication(string name, Guid id)
		{
			var application = new ApplicationSetting { Name = name, Id = id };
			return application;
		}

		public void Generate()
		{
			var application = CreateApplication( "test1", new Guid("c78ee05e-1115-480b-9ab7-a3ab3c0f6643"));

			GenerateUsers(application);
			GenerateUser(application, "admin", "Peklo123", "superuser");
			Roles.AddUserToRole("admin", "admin");

			GenerateUser(application, "dalik", "Peklo123", "admin");

			WebSecurity.CreateUserAndAccount("lades", "a");
			WebSecurity.CreateUserAndAccount("pepa", "a");

			Context.ApplicationSettings.Add(application);


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
			var markdowntext = "# nadpis H1 \n## nadpis H2\n### nadpis H3\n###### nadpis H6\n" +
							   "tohle je **tučně**, tohle  *kurzíva*.  Jde to i takto: __tučně__, _kurzíva_ \n" +
			                   " a **jde to i _kombinovat_**\n" +
			                   "\n" +
			                   "mnoho dalšího na [zde](https://github.com/adam-p/markdown-here/wiki/Markdown-Cheatsheet) \n";
			Context.Grids.Add(AGrid(new Guid("c78ee05e-1115-480b-9ab7-a3ab3c0f6643"), application)
										.WithGridElement(
											AGridElement("simplehtml", application)
												.WithResource("text", markdowntext)
												.WithResource("text", markdowntext, CultureEn)
												)
										.WithGridElement(
											AGridElement("text", application)
												.WithResource("text", "český text bez formatování")
												.WithResource("text", "english text without formating", CultureEn)
												)
										.WithResource(SpecialResourceEnum.Link, "testPage_link")
										.WithResource("name", "testovaci stranka", CultureCs)
										.WithResource("name", "Test Page", CultureEn)
								);

			Context.Grids.Add(AGrid(new Guid("bb8ee05e-1115-480b-9ab7-a3ab3c0f6643"), application, true)
										.WithResource(SpecialResourceEnum.Link, "aaa")
										.WithResource("name", "Uživatelská část", CultureCs)
										.WithResource("name", "User profile", CultureEn)
										.WithGridElement(
											AGridElement("clientspecific", application).WithSkin("postdataform")
											)
										.WithGridElement(
											AGridElement("simplehtml", application)
												.WithResource("text", "### tuto stránku vidí jenom příhlášení uživatelé")
												.WithResource("text", "### this page is visible only for authenticated users", CultureEn)
												)

								);

			Context.Grids.Add(AGrid(new Guid("ab8ee05e-1115-480b-9ab7-a3ab3c0f6643"), application)
										.WithResource(SpecialResourceEnum.Link, "gallery_1")
										.WithResource("name", "galerie 1", CultureCs)
										.WithResource("name", "gallery 1", CultureEn)
								);
		}

		private void GenerateUsers(ApplicationSetting application)
		{
			Roles.CreateRole("admin");
			Roles.CreateRole("superuser");
			Roles.CreateRole("applicationUser");
			
			GenerateApplicationUser(application, "pepa", "b");
		}

		private void GenerateUser(ApplicationSetting application, string username, string passsword, string role)
		{
			WebSecurity.CreateUserAndAccount(username, passsword);
			var userid = WebSecurity.GetUserId(username);
			var user = Context.UserProfile.Single(x => x.Id == userid);
			application.Users.Add(user);
			

			Roles.AddUserToRole(username, role);
		}

		private void GenerateApplicationUser(ApplicationSetting application, string username, string password)
		{
			WebSecurityApplication.CreateUserAndAccount(application.Id, username, password);
			var userid = WebSecurityApplication.GetUserId(application.Id, username);
			var user = Context.UserProfile.Single(x => x.Id == userid);
			user.ApplicationUser = 1;
			application.Users.Add(user);
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