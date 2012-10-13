using System.Linq;
using NUnit.Framework;
using cms.data.Dtos;
using cms.data.EF;
using cms.data.EF.DataProvider;
using cms.data.Shared.Models;
using cms.data.tests.EF;
using cms.data.tests.Helpers;
using cms.shared;

namespace cms.data.tests.UpdateResourceList
{
	[TestFixture]
	public class UpdateResourceList_Test_WithDB
	{
		const string CultureCs = "cs";
		const string CultureEn = "en";

		public UpdateResourceList_Test_WithDB()
		{
			//Database.SetInitializer(new DropAndCreate());
		}

		[SetUp]
		public void Setup()
		{
			Xxx.DeleteDatabaseDataGenereateSampleData();
			SharedLayer.Init();
		}

		Grid SimpleGridPage(string name)
		{
			var grid = new Grid
				{
					Category = "page",
					Name = name
				};
			return grid;
		}

		[Test]
		public void Value_AddNew_Test()
		{
			var grid1 = SimpleGridPage("prd").AddToDb();

			var newResources = ResourcesHelper.EmptyResourcesDto()
								  .WithResource("link", "newlinkvalue", 0);

			using (var db = new EfContext())
			{
				var before = db.Resources.Count();
				var repo = new EfRepository(db);

				Assert.AreEqual(0, grid1.Resources.Count);
				grid1.UpdateResourceList(newResources, CultureCs, repo);
				Assert.AreEqual(1, grid1.Resources.Count);

				grid1.CheckResource("link")
					 .ValueIs("newlinkvalue")
				     .OwnerIs(grid1.Id);

				Assert.AreEqual(before + 1, db.Resources.Count());
			}
		}
		
		[Test]
		public void AddReference_Test()
		{
			var grid1 = SimpleGridPage("prd").AddToDb();

			var grid2 = SimpleGridPage("prd2")
				.WithResource("link", "dbvalueLink222").AddToDb();

			using (var db = new EfContext())
			{
				var before = db.Resources.Count();

				var repo = new EfRepository(db);

				Assert.AreEqual(0, grid1.Resources.Count);
				Assert.AreEqual(1, grid2.Resources.Count);

				grid1.UpdateResourceList(grid2.Resources.ToDtos(), CultureCs, repo);

				Assert.AreEqual(1, grid1.Resources.Count);
				grid1.CheckResource("link")
				     .ValueIs("dbvalueLink222")
				     .OwnerIs(grid2.Id);

				Assert.AreEqual(before, db.Resources.Count());

			}
		}

		[Test]
		public void Reference_Replace_byReference_Test()
		{
			var grid1 = SimpleGridPage("prd")
				.WithResource("link", "dbvalueLink111").AddToDb();

			var grid2 = SimpleGridPage("prd2")
				.WithResource("link", "dbvalueLink222").AddToDb();

			using (var db = new EfContext())
			{
				var before = db.Resources.Count();
				var repo = new EfRepository(db);

				Assert.AreEqual(1, grid1.Resources.Count);
				Assert.AreEqual(1, grid2.Resources.Count);

				grid1.UpdateResourceList(grid2.Resources.ToDtos(), CultureCs, repo);

				Assert.AreEqual(1, grid1.Resources.Count);
				grid1.CheckResource("link")
				     .ValueIs("dbvalueLink222")
				     .OwnerIs(grid2.Id);

				grid2.CheckResource("link")
				     .ValueIs("dbvalueLink222")
				     .OwnerIs(grid2.Id);

				Assert.AreEqual(before, db.Resources.Count());

			}
		}

		[Test]
		public void ReplaceReference_ByNewResource_Test()
		{
			var grid1 = SimpleGridPage("prd");

			var grid2 = SimpleGridPage("prd2")
				.WithResource("link", "dbvalueLink222").AddToDb();

			var newResources = ResourcesHelper.EmptyResourcesDto()
			                                  .WithResource("link", "newlinkvalue", 0);

			using (var db = new EfContext())
			{
				var before = db.Resources.Count();
				var repo = new EfRepository(db);

				//add reference
				grid1.UpdateResourceList(grid2.Resources.ToDtos(), CultureCs, repo);

				grid1.UpdateResourceList(newResources, CultureCs, repo);

				Assert.AreEqual(1, grid1.Resources.Count);
				grid1.CheckResource("link")
				     .ValueIs("newlinkvalue")
				     .OwnerIs(grid1.Id);

				grid2.CheckResource("link")
					 .ValueIs("dbvalueLink222")
				     .OwnerIs(grid2.Id);

				Assert.AreEqual(before + 1, db.Resources.Count());

			}
		}
	}
}