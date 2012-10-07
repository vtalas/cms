using System;
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
	public class UpdateGridElementMethodTest
	{

		[SetUp]
		public void Setup()
		{
			Database.SetInitializer(new DropAndCreate());
			Xxx.DeleteDatabaseData();
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

	}
}