using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Moq;
using NUnit.Framework;
using cms.data.Dtos;
using cms.data.EF;
using cms.data.EF.DataProvider;
using cms.data.EF.Initializers;
using cms.data.Shared.Models;
using cms.shared;

namespace cms.data.tests.EF
{
	[TestFixture]
	public class UpdateGridElementMethodTest
	{
		public UpdateGridElementMethodTest()
		{
			Database.SetInitializer(new DropAndCreate());
		}

		[SetUp]
		public void Setup()
		{
			Xxx.DeleteDatabaseDataGenereateSampleData();
			SharedLayer.Init();
		}

		[Test]
		public void Basics_test()
		{
			using (var db = new SessionManager().CreateSession)
			{
				var newitem = DataEfHelpers.AddDefaultGridElement();

				newitem.Content = "newcontent";
				newitem.Width = 0;
				newitem.Position = 111;
				newitem.Skin = "aaa";

				db.GridElement.Update(newitem.ToDto());

				var updated = db.GridElement.Get(newitem.Id);

				Assert.AreEqual(0, updated.Width);
				Assert.AreEqual(111, updated.Position);
				Assert.AreEqual("aaa", updated.Skin);
				Assert.AreEqual("newcontent", updated.Content);
			}
		}

		[Test]
		public void UpdateGridElement_Resources_UpdateResourceValue_and_OnlyValue()
		{
			using (var checkdb = new EfContext())
			{
				using (var db = new SessionManager().CreateSession)
				{
					var newitem = DataEfHelpers.AddDefaultGridElement();
					var resourcesCountBefore = checkdb.Resources.Count();

					newitem.Resources.ToList()[0].Value = "xxx";
					newitem.Resources.ToList()[0].Key = "new cs";

					db.GridElement.Update(newitem.ToDto());

					var updated = db.GridElement.Get(newitem.Id);

					Assert.AreEqual(1, updated.ResourcesLoc.Count);
					Assert.AreEqual(resourcesCountBefore, checkdb.Resources.Count());
					Assert.IsNotNull(updated.ResourcesLoc.SingleOrDefault(x => x.Value.Value == "xxx"));
					Assert.AreNotEqual("new cs", updated.ResourcesLoc.Single(x => x.Value.Value == "xxx").Key);
				}
			}
		}

		[Test]
		public void UpdateGridElement_AddNewResources()
		{

			GridElement newitem;
			using (var db = new SessionManager().CreateSession)
			{
				var a = new GridElement { Content = "oldcontent", Width = 12, Position = 0, Skin = "xxx" };
				newitem = db.GridElement.AddToGrid(a, DataEfHelpers.guid);
				Assert.True(!newitem.Id.IsEmpty());
			}

			using (var db = new SessionManager().CreateSession)
			{

				var resources = new List<Resource>
					                {
						                new Resource {Culture = "cs", Value = "new cesky", Key = "text1"},
						                new Resource {Culture = "en", Value = "new englicky", Key = "text1"},
					                };

				newitem.Resources = resources;

				db.GridElement.Update(newitem.ToDto());
				var updated = db.GridElement.Get(newitem.Id);
				Assert.AreEqual(1, updated.ResourcesLoc.Count);
			}
		}

		[Test]
		public void addNewResources_withUniqueKey()
		{
			using (var checkdb = new EfContext())
			{
				var resourcesCountBefore = checkdb.Resources.Count();

				//Resources + 2
				GridElement newitem = DataEfHelpers.AddDefaultGridElement();

				//Resources + 1
				newitem.Resources.Add(new Resource { Culture = "cs", Value = "new cesky", Key = "text1xx" });

				using (var db = new SessionManager().CreateSession)
				{
					db.GridElement.Update(newitem.ToDto());
					var updated = db.GridElement.Get(newitem.Id);
					//text1, text1xx
					Assert.AreEqual(2, updated.ResourcesLoc.Count);

					Assert.AreEqual(resourcesCountBefore + 3, checkdb.Resources.Count());
				}
			}
		}

		[Test]
		public void UpdateGridElement_addNewResources_keyExist()
		{
			using (var checkdb = new EfContext())
			{
				var resourcesCountBefore = checkdb.Resources.Count();

				GridElement newitem = DataEfHelpers.AddDefaultGridElement();

				newitem.Resources.Add(new Resource { Culture = "cs", Value = "new cesky", Key = "text1" });
				newitem.Resources.Add(new Resource { Culture = "en", Value = "new cesky", Key = "text1" });

				using (var db = new SessionManager().CreateSession)
				{
					Assert.Throws<ArgumentException>(() => db.GridElement.Update(newitem.ToDto()));
					Assert.AreEqual(resourcesCountBefore + 2, checkdb.Resources.Count());
				}
			}
		}

		[Test]
		public void UpdateAttachedResource()
		{
			using (var checkdb = new EfContext())
			{
				var g1 = DataEfHelpers.AddDefaultGridElement();
				var resourcesCountBefore = checkdb.Resources.Count();
				var res1 = g1.Resources.First();

				Assert.AreEqual("cs", res1.Culture);
				Assert.AreEqual("text1", res1.Key);

				res1.Value = "prd";
				res1.Culture = "cs";
				res1.Key = "kkk";
				using (var db = new SessionManager().CreateSession)
				{

					db.GridElement.Update(g1.ToDto());
					var updated = db.GridElement.Get(g1.Id);
					var res1Updated = updated.ResourcesLoc.First();

					Assert.AreEqual(1, updated.ResourcesLoc.Count);
					Assert.AreEqual("prd", res1Updated.Value.Value);

					//u resourcu se da zmenit jenom value
					Assert.AreNotEqual("kkk", res1Updated.Key);
					Assert.AreEqual(resourcesCountBefore, checkdb.Resources.Count());
				}
			}
		}

		[Test]
		public void UpdateGridElement_AddNewResource()
		{
			using (var checkdb = new EfContext())
			{
				var g1 = DataEfHelpers.AddDefaultGridElement();
				var resourcesCountBefore = checkdb.Resources.Count();
				var a = new Resource
							{
								Value = "xxx",
								Key = "key1"
							};
				g1.Resources.Add(a);

				using (var db = new SessionManager().CreateSession)
				{
					db.GridElement.Update(g1.ToDto());
					var updated = db.GridElement.Get(g1.Id);

					Assert.AreEqual(2, updated.ResourcesLoc.Count);
					Assert.AreEqual(resourcesCountBefore + 1, checkdb.Resources.Count());
				}
			}
		}

		[Test]
		//INFO: Resources - pokud je neexistujici Id tak prida novy zaznam 
		public void UpdateGridElement_TryAttachNonExistingResources()
		{
			using (var checkdb = new EfContext())
			{
				var gridelementdb = DataEfHelpers.AddDefaultGridElement();
				var resourcesCountBefore = checkdb.Resources.Count();
				gridelementdb.Resources.Add(new Resource { Id = 111, Culture = "cs", Value = "non existing", Key = "text1AA" });

				using (var db = new SessionManager().CreateSession)
				{
					db.GridElement.Update(gridelementdb.ToDto());
				}
				Assert.AreEqual(resourcesCountBefore + 1, checkdb.Resources.Count());
			}
		}

		[Test]
		public void UpdateGridElement_AttachExistingResources_sadsdXXXXXXXXXXXXXXXXXX()
		{
			using (var checkdb = new EfContext())
			{
				var gridelementdb1 = DataEfHelpers.AddDefaultGridElement();
				var gridelementdb2 = DataEfHelpers.AddDefaultGridElement();

				var resourcesCountBefore = checkdb.Resources.Count();
				var res2 = gridelementdb2.Resources.First();

				gridelementdb1.Resources.Add(new Resource { Id = res2.Id, Culture = "cs", Value = "reference na resource z 1", Key = "text1AA" });
				using (var db = new SessionManager().CreateSession)
				{
					db.GridElement.Update(gridelementdb1.ToDto());
					var updated = db.GridElement.Get(gridelementdb1.Id);
					Assert.AreEqual(2, updated.ResourcesLoc.Count);
				}
				Assert.AreEqual(resourcesCountBefore, checkdb.Resources.Count());
			}
		}

		[Test]
		public void TryAttachExistingResourceWithNoKey_ShouldBeIgnored()
		{
			using (var checkdb = new EfContext())
			{
				var resourcesCountBefore = checkdb.Resources.Count();
				GridElement gridelementdb;

				using (var db = new SessionManager().CreateSession)
				{
					var a = new GridElement { Content = "oldcontent", Width = 12, Position = 0, Skin = "xxx" };
					gridelementdb = db.GridElement.AddToGrid(a, DataEfHelpers.guid);
				}

				using (var db = new SessionManager().CreateSession)
				{
					var existingsResBefore = checkdb.Resources.First();
					Assert.IsNull(existingsResBefore.Key);

					var resources = new List<Resource>
						                {
							                existingsResBefore,
						                };
					gridelementdb.Resources = resources;

					db.GridElement.Update(gridelementdb.ToDto());
					var updated = db.GridElement.Get(gridelementdb.Id);
					Assert.AreEqual(0, updated.ResourcesLoc.Count);
					Assert.AreEqual(resourcesCountBefore, checkdb.Resources.Count());
				}
			}
		}



		[TestFixture]
		public class JsonDataEfHelpers_Test_NoDB
		{
			const string CultureCs = "cs";
			const string CultureEn = "en";
			private static IList<Resource> _allResources;
			private IRepository _repository;
			
			private Dictionary<string, ResourceDtoLoc> DefaultResourcesDto()
			{
				return new Dictionary<string, ResourceDtoLoc>
					                 {
						                 {"link", new ResourceDtoLoc {Id = 1, Value = "linkxxxcs"}},
						                 {"name", new ResourceDtoLoc {Id = 2, Value = "name"}},
						                 {"newname", new ResourceDtoLoc {Id = 0, Value = "newnamename2"}}
					                 };

			}

			private Dictionary<string, ResourceDtoLoc> EmptyResourcesDto()
			{
				return new Dictionary<string, ResourceDtoLoc>();
			}

			private Grid DefaultGrid()
			{
				return new Grid()
				{
					Name = "aaa",
					Id = Guid.NewGuid(),
					Category = "page",
				};
			}

			private IRepository GetRepositoryMock()
			{
				var repo = new Mock<IRepository>();
				repo.Setup(x => x.Resources).Returns(_allResources.AsQueryable);
				return repo.Object;
			}

			[SetUp]
			public void SetUp()
			{
				_allResources = new List<Resource>();
				_repository = GetRepositoryMock();

				SharedLayer.Init();
			}

			[Test]
			public void UpdateResourceTest_gridHasNone_addNewResources()
			{
				var g = DefaultGrid();

				var gResources = EmptyResourcesDto()
					.WithResource("link", "linkvaluedto", 0)
					.WithResource("name", "linkvaluedto", 0);

				Assert.AreEqual(0, g.Resources.Count);

				g.UpdateResourceList(gResources, CultureCs, _repository);
				Console.WriteLine(g.Resources.Count);
				Assert.AreEqual(2, g.Resources.Count);
			}

			[Test]
			public void UpdateResourceTest_gridHas2_addNewResources()
			{
				var g = DefaultGrid()
					.WithResource("linkXXX", "dbvalueLink")
					.WithResource("nameXXX", "dbvalueName");

				var gResources = EmptyResourcesDto()
					.WithResource("link", "linkvaluedto", 0)
					.WithResource("name", "linkvaluedto", 0);

				Assert.AreEqual(2, g.Resources.Count);
				g.UpdateResourceList(gResources, CultureCs, _repository);
				Assert.AreEqual(4, g.Resources.Count);
			}

			[Test]
			public void UpdateResourceTest_gridHas2_UpdateResources_isOwner()
			{
				var g = DefaultGrid()
					.WithResource("link", "dbvalueLink")
					.WithResource("name", "dbvalueName");

				var gResources = EmptyResourcesDto()
					.WithResource("link", "linkvaluedto", 0)
					.WithResource("name", "namevaluedto", 0);

				g.UpdateResourceList(gResources, CultureCs, _repository);
			
				Assert.AreEqual(2, g.Resources.Count);
				g.CheckResource("link").ValueIs("linkvaluedto");
				g.CheckResource("name").ValueIs("namevaluedto");
			}

			[Test]
			public void UpdateResourceTest_checkOwnerShip()
			{
				var g = DefaultGrid()
					.WithResource("link", "dbvalueLink", CultureCs, 11)
					.WithResource("name", "dbvalueName", CultureCs, 12);

				Assert.AreEqual(g.Id, g.Resources.GetByKey("name", CultureCs).Owner);
			}

			[Test]
			public void UpdateResourceTest_UpdateResources_isOwnwer_searchById()
			{
				var g = DefaultGrid()
					.WithResource(_allResources, "link", "dbvalueLink", CultureCs, 1)
					.WithResource(_allResources, "name", "dbvalueName", CultureCs, 2);

				var gResources = EmptyResourcesDto()
					.WithResource("link", "linkvaluedto", 1)
					.WithResource("name", "namevaluedto", 2);

				g.UpdateResourceList(gResources, CultureCs, _repository);
				Assert.AreEqual(2, g.Resources.Count);
				g.CheckResource("link").ValueIs("linkvaluedto");
				g.CheckResource("name").ValueIs("namevaluedto");
			}

			[Test]
			public void UpdateResourceTest_UpdateResources_isNotOwnwer_ShouldReplaceByReference()
			{
				var g1 = DefaultGrid()
					.WithResource(_allResources, "link", "dbvalueLink", CultureCs, 1);

				var g2 = DefaultGrid()
					.WithResource(_allResources, "link", "xxx", CultureCs, 12);

				g1.UpdateResourceList(g2.Resources.ToDtos(), CultureCs, _repository);

				g1.CheckResource("link")
					.ValueIs("xxx")
					.OwnerIs(g2.Id);

				Assert.AreEqual(1, g1.Resources.Count);
			}

			[Test]
			public void UpdateResourceTest_AttachResourcReference()
			{
				var g1 = DefaultGrid()
					.WithResource(_allResources, "link", "dbvalueLink", CultureCs, 1);

				var g2 = DefaultGrid();

				g2.UpdateResourceList(g1.Resources.ToDtos(), CultureCs, _repository);

				g2.CheckResource("link")
					.ValueIs("dbvalueLink")
					.OwnerIs(g1.Id);

				Assert.AreEqual(1, g2.Resources.Count);
			}

			[Test]
			public void UpdateResourceTest_AttachResourcReference_ResourceExist_ShouldReplaceReference()
			{
				var grid1 = DefaultGrid().WithResource(_allResources, "link", "a111", CultureCs, 1);
				var grid2 = DefaultGrid().WithResource(_allResources, "link", "b222", CultureCs, 2);

				grid1.UpdateResourceList(grid2.Resources.ToDtos(), CultureCs, _repository);

				grid1.CheckResource("link")
					  .ValueIs("b222")
					  .OwnerIs(grid2.Id);

				Assert.AreEqual(1, grid2.Resources.Count);
			}

	
			[Test]
			public void UpdateResourceTest_LANGUAGE()
			{
				var grid1 = DefaultGrid().WithResource(_allResources, "link", "a111", CultureCs, 1);
				var grid2 = DefaultGrid().WithResource(_allResources, "link", "b222", CultureCs, 2);
				var grid3 = DefaultGrid().WithResource(_allResources, "link", "c333", CultureEn, 3);

				grid1.UpdateResourceList(grid2.Resources.ToDtos(), CultureCs, _repository);

				grid1.CheckResource("link", CultureCs)
					  .ValueIs("b222")
					  .OwnerIs(grid2.Id);

				grid1.UpdateResourceList(grid3.Resources.ToDtos(CultureEn), CultureEn, _repository);

				Assert.AreEqual(2, grid1.Resources.Count);

				grid1.CheckResource("link");
				grid1.CheckResource("link", CultureEn)
					  .ValueIs("c333")
					  .OwnerIs(grid3.Id);
			}

		}

		[TestFixture]
		public class JsonDataEfHelpersTest
		{
			const string CultureCs = "cs";
			const string CultureEn = "en";
			
			public JsonDataEfHelpersTest()
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
			public void ReplaceReference_Test()
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
					Assert.AreEqual(before, db.Resources.Count());
				
				}
			}
		}
	}

	public static class DbExtension
	{
		public static Grid AddToDb(this Grid item)
		{
			using (var db = new EfContext())
			{
				var x = db.Grids.Add(item);
				db.SaveChanges();
				return x;
			}
		}

	}

	public static class MyClass
	{
		private static readonly string CurrentCulture = SharedLayer.Culture;

		public static Resource CheckResource(this Grid item, string key, string culture)
		{
			var getbyLink = item.Resources.GetByKey(key, culture);
			Assert.IsNotNull(getbyLink);
			return getbyLink;
		}

		public static Resource CheckResource(this Grid item, string key)
		{
			return item.CheckResource(key, CurrentCulture);
		}

		public static Resource ValueIs(this Resource item, string value)
		{
			Assert.AreEqual(value, item.Value);
			return item;
		}

		public static Resource OwnerIs(this Resource item, Guid expectedId)
		{
			Assert.AreEqual(expectedId, item.Owner);
			return item;
		}

		public static Grid WithResource(this Grid item, string key, string value, string culture = "cs", int id = 0)
		{
			item.Resources.Add(GetResource(key, value, item.Id, culture, id));
			return item;
		}

		public static Grid WithResource(this Grid item, IList<Resource> allResources, string key, string value, string culture = "cs", int id = 0)
		{
			var newitem = GetResource(key, value, item.Id, culture, id);
			item.Resources.Add(newitem);
			allResources.Add(newitem);
			return item;
		}

		public static Dictionary<string, ResourceDtoLoc> WithResource(this Dictionary<string, ResourceDtoLoc> item, string key, string value, int id)
		{
			item.Add(key, new ResourceDtoLoc() { Id = id, Value = value });
			return item;
		}

		public static Resource GetResource(string key, string value, Guid parentId, string culture, int id)
		{
			return new Resource { Id = id, Value = value, Key = key, Culture = culture, Owner = parentId };
		}


	}

}