using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using NUnit.Framework;
using cms.data.Dtos;
using cms.data.EF;
using cms.data.EF.Initializers;
using cms.data.Shared.Models;
using cms.shared;

namespace cms.data.tests.EF
{
	[TestFixture]
	public class AddGridElementMethodTest
	{
		public AddGridElementMethodTest()
		{
			Database.SetInitializer(new DropAndCreateAlwaysForce());
		}

		[SetUp]
		public void Setup()
		{
			Xxx.DeleteDatabaseDataGenereateSampleData();
		}

		[Test]
		public void AddGridElement()
		{
			using (var checkdb = new EfContext())
			{
				using (var db = new SessionManager().CreateSession)
				{
					var gridpage = db.Page.Add(new GridPageDto { Name = "addgridElement test Gridpage", Category = CategoryEnum.Page });
					var gridDb = db.Page.Get(gridpage.Id);
					var gridelementsBefore = checkdb.GridElements.Count();

					var gridelem = new GridElement()
						               {
							               Content = "XXX",
							               Position = 0,
							               Width = 12,
							               Type = "text",
						               };

					var newgridelem = db.Page.AddToGrid(gridelem, gridDb.Id);

					Assert.AreEqual(gridelementsBefore + 1, checkdb.GridElements.Count());
				}
			}
		}

		[Test]
		public void AddGridElement_AddNewResources_test_same_keys()
		{
			using (var db = new SessionManager().CreateSession)
			{
				var gridpage = db.Page.Get(DataEfHelpers._defaultlink);
				var grid = db.Page.Get(gridpage.Id);

				var gridelem = new GridElement
					               {
						               Content = "XXX",
						               Position = 0,
						               Width = 12,
						               Type = "text",
						               Resources = new List<Resource>
							                           {
								                           new Resource {Culture = "cs", Value = "cesky", Key = "text1"},
								                           new Resource {Culture = "en", Value = "englicky", Key = "text1"},
							                           }
					               };


				var newgridelem = db.Page.AddToGrid(gridelem, grid.Id);

				Assert.IsTrue(newgridelem.Resources.Any());
				Assert.AreEqual(2, newgridelem.Resources.Count(x => x.Key == "text1"));
			}
		}

		[Test]
		public void AddGridElement_AddNew_and_ExistingResources_test_same_keys()
		{
			using (var db = new SessionManager().CreateSession)
			{
				var gridpage = db.Page.Get(DataEfHelpers._defaultlink);
				var grid = db.Page.Get(gridpage.Id);

				var gridelem = new GridElement
					               {
						               Content = "XXX",
						               Position = 0,
						               Width = 12,
						               Type = "text",
						               Resources = new List<Resource>
							                           {
								                           new Resource {Culture = "cs", Value = "cesky", Key = "text1"},
								                           new Resource {Culture = "en", Value = "englicky", Key = "text1"},
							                           }
					               };

				var newgridelem = db.Page.AddToGrid(gridelem, grid.Id);

				Assert.IsTrue(newgridelem.Resources.Any());
				Assert.AreEqual(2, newgridelem.Resources.Count(x => x.Key == "text1"));

			}
		}
	}
}