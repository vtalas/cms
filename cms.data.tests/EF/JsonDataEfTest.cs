using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using NUnit.Framework;
using cms.data.Dtos;
using cms.data.EF;
using cms.data.EF.Initializers;
using cms.data.Shared.Models;

namespace cms.data.tests.EF
{


	public static class Helpers{

		public static Resource getresource(this IEnumerable<Resource> source, string culture, string key )
		{
			return source.Single(x => x.Culture == culture && x.Key == key);
		}
	
		static Guid guid = new Guid("c78ee05e-1115-480b-9ab7-a3ab3c0f6643");
	
		public static GridElement AddDefaultGridElement()
		{
			using (var db = SessionManager.CreateSession)
			{
				var a = new GridElement
				{
					Content = "oldcontent",
					Width = 12,
					Line = 0,
					Skin = "xxx",
					Resources = new List<Resource>
				            	{
				            		new Resource{ Culture = "cs", Value = "cesky", Key = "text1"},
									new Resource{ Culture = "en", Value = "englicky", Key = "text1"},
				            	}

				};
				var newitem = db.AddGridElementToGrid(a, guid);
				Assert.True(!newitem.Id.IsEmpty());
				return newitem;
			}

		}

}

	[TestFixture]
	public class JsonDataEfTest
	{
		private JsonDataEf repo { get; set; }
		private string _defaultlink = "linkTestPage";
		EfContext _context;
		private Guid guid = new Guid("c78ee05e-1115-480b-9ab7-a3ab3c0f6643");



		GridElement AddDefaultGridElement()
		{
			using (var db = SessionManager.CreateSession)
			{
				var a = new GridElement
				{
					Content = "oldcontent",
					Width = 12,
					Line = 0,
					Skin = "xxx",
					Resources = new List<Resource>
				            	{
				            		new Resource{ Culture = "cs", Value = "cesky", Key = "text1"},
									new Resource{ Culture = "en", Value = "englicky", Key = "text1"},
				            	}

				};
				var newitem = db.AddGridElementToGrid(a, guid);
				Assert.True(!newitem.Id.IsEmpty());
				return newitem;
			}

		}

		[SetUp]
		public void Setup()
		{
			_context = new EfContext();
			Database.SetInitializer(new DropAndCreate());
			repo = new JsonDataEf(guid, _context);
			Assert.IsNotNull(repo);
		}

		[TestFixture]
		public class AddGridPageMethodTest
		{
			[SetUp]
			public void Setup()
			{
				Database.SetInitializer(new DropAndCreate());
			}

			GridPageDto AddGridpage(string name, string link)
			{
				using (var db = SessionManager.CreateSession)
				{
					var a = new GridPageDto
					{
						Name = name,
						ResourceDto = new ResourceDto { Value = link }
					};

					var n = db.Add(a);
					Assert.AreEqual(name, n.Name);
					Assert.AreEqual(link, n.ResourceDto.Value);
					return n;
				}
				
			}

			GridPageDto GetGridpage(Guid id )
			{
				using (var db = SessionManager.CreateSession)
				{
					var n = db.GetGridPage(id);
					return n;
				}
				
			}
			
			[Test]
			public void AddGridpage_Basic()
			{
				var name = "xxx";
				var link = "AddGridpage_Basic";
				var n = AddGridpage(name, link);
				var g = GetGridpage(n.Id);
				
				Assert.AreEqual(name, g.Name);
				Assert.AreEqual(link, g.ResourceDto.Value);
			}

			[Test]
			public void AddGridpage_Basic_linkExists()
			{
				var name = "xxx";
				var link = "AddGridpage_Basic_linkExists";
				
				AddGridpage(name, link);
				Assert.Throws<ArgumentException>(() => AddGridpage(name + name, link));
			}
		}

		[TestFixture]
		public class Update_GridElement_MethodTest 
		{
			EfContext _context;
			private Guid guid = new Guid("c78ee05e-1115-480b-9ab7-a3ab3c0f6643");

			[SetUp]
			public void Setup()
			{
				_context = new EfContext();
				Database.SetInitializer(new DropAndCreate());
			}

			[Test]
			public void UpdateGridElement_Basics_test()
			{
				using (var db = SessionManager.CreateSession)
				{
					var newitem = Helpers.AddDefaultGridElement();

					newitem.Line = 1;
					newitem.Content = "newcontent";
					newitem.Width = 0;
					newitem.Position = 111;
					newitem.Skin = "aaa";

					db.Update(newitem);

					var updated = db.GetGridElement(newitem.Id);

					Assert.AreEqual(1, updated.Line);
					Assert.AreEqual(0, updated.Width);
					Assert.AreEqual(111, updated.Position);
					Assert.AreEqual("aaa", updated.Skin);
					Assert.AreEqual("newcontent", updated.Content);
				}
			}

			[Test]
			public void UpdateGridElement_Resources_UpdateResourceValue_and_OnlyValue()
			{
				using (var db = SessionManager.CreateSession)
				{
					var newitem = Helpers.AddDefaultGridElement();
					var resourcesCountBefore = _context.Resources.Count();

					newitem.Resources.ToList()[0].Value = "xxx";
					newitem.Resources.ToList()[0].Key = "new cs";

					db.Update(newitem);

					var updated = db.GetGridElement(newitem.Id);

					Assert.AreEqual(2, updated.Resources.Count);
					Assert.AreEqual(resourcesCountBefore, _context.Resources.Count());
					Assert.IsNotNull(updated.Resources.SingleOrDefault(x => x.Value == "xxx"));


					Assert.AreNotEqual("new cs", updated.Resources.Single(x => x.Value == "xxx").Key);
				}
			}

			[Test]
			public void UpdateGridElement_AddNewResources()
			{

				GridElement newitem;
				using (var db = SessionManager.CreateSession)
				{
					var a = new GridElement {Content = "oldcontent", Width = 12, Line = 0, Skin = "xxx"};
					newitem = db.AddGridElementToGrid(a, guid);
					Assert.True(!newitem.Id.IsEmpty());
				}

				using (var db = SessionManager.CreateSession)
				{

					var resources = new List<Resource>
					                	{
					                		new Resource {Culture = "cs", Value = "new cesky", Key = "text1"},
					                		new Resource {Culture = "en", Value = "new englicky", Key = "text1"},
					                	};

					newitem.Resources = resources;

					db.Update(newitem);
					var updated = db.GetGridElement(newitem.Id);
					Assert.AreEqual(2, updated.Resources.Count);
				}
			}

			[Test]
			public void UpdateGridElement_addNewResources_keydoesntExist()
			{
				var resourcesCountBefore = _context.Resources.Count();

				GridElement newitem = Helpers.AddDefaultGridElement();

				newitem.Resources.Add(new Resource {Culture = "cs", Value = "new cesky", Key = "text1xx"});
				newitem.Resources.Add(new Resource {Culture = "en", Value = "new cesky", Key = "text1xx"});

				using (var db = SessionManager.CreateSession)
				{
					db.Update(newitem);
					var updated = db.GetGridElement(newitem.Id);
					Assert.AreEqual(4, updated.Resources.Count);

					Assert.AreEqual(resourcesCountBefore + 4, _context.Resources.Count());
				}
			}

			[Test]
			public void UpdateGridElement_addNewResources_keyExist()
			{
				var resourcesCountBefore = _context.Resources.Count();

				GridElement newitem = Helpers.AddDefaultGridElement();

				newitem.Resources.Add(new Resource {Culture = "cs", Value = "new cesky", Key = "text1"});
				newitem.Resources.Add(new Resource {Culture = "en", Value = "new cesky", Key = "text1"});

				using (var db = SessionManager.CreateSession)
				{
					Assert.Throws<ArgumentException>(() => db.Update(newitem));
					Assert.AreEqual(resourcesCountBefore + 2, _context.Resources.Count());
				}
			}
			[Test]
			public void UpdateGridElement_UpdateAttachedResource()
			{
				var g1 = Helpers.AddDefaultGridElement();
				var resourcesCountBefore = _context.Resources.Count();
				var res1 = g1.Resources.First();

				Assert.AreEqual("cs", res1.Culture);
				Assert.AreEqual("text1", res1.Key);

				res1.Value = "prd";
				res1.Culture = "xx";
				res1.Key = "kkk";
				using (var db = SessionManager.CreateSession)
				{

					db.Update(g1);
					var updated = db.GetGridElement(g1.Id);
					var res1Updated = updated.Resources.First();

					Assert.AreEqual(2, updated.Resources.Count);
					Assert.AreEqual("prd", res1Updated.Value);

					//u resourcu se da zmenit jenom value
					Assert.AreNotEqual("xx", res1Updated.Culture);
					Assert.AreNotEqual("kkk", res1Updated.Key);
					Assert.AreEqual(resourcesCountBefore, _context.Resources.Count());
				}
			}

			[Test]
			public void UpdateGridElement_AddNewResource()
			{
				var g1 = Helpers.AddDefaultGridElement();
				var resourcesCountBefore = _context.Resources.Count();
				var a = new Resource
				{
					Value = "xxx",
					Key = "key1"
				};
				g1.Resources.Add(a);

				using (var db = SessionManager.CreateSession)
				{
					db.Update(g1);
					var updated = db.GetGridElement(g1.Id);

					Assert.AreEqual(3, updated.Resources.Count);
					Assert.AreEqual(resourcesCountBefore + 1, _context.Resources.Count());
				}
			}

			[Test]
			//INFO: Resources - pokud je neexistujici Id tak prida novy zaznam 
			public void UpdateGridElement_TryAttachNonExistingResources()
			{
				var gridelementdb = Helpers.AddDefaultGridElement();
				var resourcesCountBefore = _context.Resources.Count();
				gridelementdb.Resources.Add(new Resource { Id = 111, Value = "non existing" });

				using (var db = SessionManager.CreateSession)
				{
					db.Update(gridelementdb);
				}
				Assert.AreEqual(resourcesCountBefore + 1, _context.Resources.Count());
			}

			[Test]
			public void UpdateGridElement_AttachExistingResources_sadsdXXXXXXXXXXXXXXXXXX()
			{
				var gridelementdb1 = Helpers.AddDefaultGridElement();
				var gridelementdb2 = Helpers.AddDefaultGridElement();

				var resourcesCountBefore = _context.Resources.Count();
				var res2 = gridelementdb2.Resources.First();

				gridelementdb1.Resources.Add(new Resource { Id = res2.Id, Value = "reference na resource z 1" });
				using (var db = SessionManager.CreateSession)
				{

					db.Update(gridelementdb1);

					var updated = db.GetGridElement(gridelementdb1.Id);

					Assert.AreEqual(3, updated.Resources.Count);
				}
				Assert.AreEqual(resourcesCountBefore, _context.Resources.Count());
			}

			[Test]
			public void UpdateGridElement_AttachExistingResources()
			{
				var resourcesCountBefore = _context.Resources.Count();
				GridElement gridelementdb;
	
				using (var db = SessionManager.CreateSession)
				{
					var a = new GridElement {Content = "oldcontent", Width = 12, Line = 0, Skin = "xxx"};
					gridelementdb = db.AddGridElementToGrid(a, guid);
				}

				using (var db = SessionManager.CreateSession)
				{
					var existingsResBefore = _context.Resources.First();
					var resources = new List<Resource>
					                	{
					                		existingsResBefore,
					                		new Resource {Culture = "cs", Value = "new cesky", Key = "text1"},
					                	};

					gridelementdb.Resources = resources;

					db.Update(gridelementdb);
					var updated = db.GetGridElement(gridelementdb.Id);
					var existingsResAfter = _context.Resources.First(x => x.Id == existingsResBefore.Id);

					Assert.AreEqual(2, updated.Resources.Count);
					Assert.AreEqual(resourcesCountBefore + 1, _context.Resources.Count());
					Assert.AreEqual(existingsResBefore.Value, existingsResAfter.Value);

				}
			}
			[Test]
			public void UpdateGridElement_Update_Attached()
			{
				const string culture = "cs";
				const string key = "text1";

				var a = Helpers.AddDefaultGridElement();
				var c = Helpers.AddDefaultGridElement();
				var resourcesCountBefore = _context.Resources.Count();
				
				using (var db = SessionManager.CreateSession)
				{

					var resa = a.Resources.getresource(culture, key);
					var resb = a.Resources.First();

					c.Resources = new List<Resource> {resa};
					db.Update(c);

					var updated = db.GetGridElement(c.Id);
					Assert.AreEqual(2, updated.Resources.Count);
					Assert.AreEqual(resa.Id, updated.Resources.getresource(culture, key).Id);

					c.Resources.Add(resb);
					db.Update(c);
					updated = db.GetGridElement(c.Id);
					Assert.AreEqual(2, updated.Resources.Count);
					Assert.AreEqual(resb.Id, updated.Resources.getresource(culture, key).Id);
				}
			
				Assert.AreEqual(resourcesCountBefore, _context.Resources.Count());
			}
		}

		[Test]
		public void Applications_test()
		{
			var list = JsonDataEf.Applications();

			foreach (var item in list)
			{
				Console.WriteLine(item.Name);
			}

			Assert.IsNotNull(list);
			Assert.IsTrue(list.Any());
		}

		[Test]
		public void Application_add_test()
		{
			var a = new ApplicationSetting
						{
							Name = "xxx",
						};
			var newitem = repo.Add(a);
			Console.WriteLine(newitem.Name);
			Assert.IsNotNull(repo.GetApplication(newitem.Id));

		}

		[Test]
		public void AddGridElement()
		{
			var gridpage = repo.Add(new GridPageDto { Name = "addgridElement test Gridpage" });
			var gridDb = repo.GetGrid(gridpage.Id);
			Assert.AreEqual(0, repo.GetGrid(gridpage.Id).GridElements.Count);


			var gridelem = new GridElement()
			{
				Content = "XXX",
				Line = 0,
				Position = 0,
				Width = 12,
				Type = "text",
			};

			var newgridelem = repo.AddGridElementToGrid(gridelem,gridDb.Id);

			Assert.AreEqual(1, newgridelem.Grid.Count);
			Assert.AreEqual(1, repo.GetGrid(gridpage.Id).GridElements.Count);

		}

		[Test]
		public void AddGridElement_AddNewResources_test_same_keys()
		{
			var gridpage = repo.GetGridPage(_defaultlink);
			var grid = repo.GetGrid(gridpage.Id);

			var gridelem = new GridElement
			{
				Content = "XXX",
				Line = 0,
				Position = 0,
				Width = 12,
				Type = "text",
				Resources = new List<Resource>
				            	{
				            		new Resource{ Culture = "cs", Value = "cesky", Key = "text1"},
									new Resource{ Culture = "en", Value = "englicky", Key = "text1"},
				            	}
			};


			var newgridelem = repo.AddGridElementToGrid(gridelem,grid.Id);

			Assert.IsTrue(newgridelem.Resources.Any());
			Assert.AreEqual(2, newgridelem.Resources.Count(x => x.Key == "text1"));

		}

		[Test]
		public void AddGridElement_AddNew_and_ExistingResources_test_same_keys()
		{
			var gridpage = repo.GetGridPage(_defaultlink);
			var grid = repo.GetGrid(gridpage.Id);
			var resourcesCountBefore = _context.Resources.Count();
			var existingsRes = _context.Resources.First();

			var gridelem = new GridElement
			{
				Content = "XXX",
				Line = 0,
				Position = 0,
				Width = 12,
				Type = "text",
				Resources = new List<Resource>
				            	{
				            		new Resource{ Culture = "cs", Value = "cesky", Key = "text1"},
									new Resource{ Culture = "en", Value = "englicky", Key = "text1"},
									existingsRes	
								}
			};

			var newgridelem = repo.AddGridElementToGrid(gridelem,grid.Id);

			Assert.IsTrue(newgridelem.Resources.Any());
			Assert.AreEqual(2, newgridelem.Resources.Count(x => x.Key == "text1"));

			var resourcesCountAfter = _context.Resources.Count();
			Assert.AreEqual(resourcesCountBefore + 2, resourcesCountAfter);

		}


		[Test]
		public void UpdateGridPage_test()
		{
			var gridpage = repo.Add(new GridPageDto
			{
				Name = "addgridElement test Gridpage",
				ResourceDto = new ResourceDto { Value = "newlink", Culture = "xx" }
			});
			var resourcesCountBefore = _context.Resources.Count();
			var gridpageResBefore = gridpage.ResourceDto;

			gridpage.ResourceDto.Value = "XXXX";
			repo.Update(gridpage);

			var updated = repo.GetGridPage(gridpage.Id);
			var resourcesCountAfter = _context.Resources.Count();
			var gridpageResAfter = updated.ResourceDto;

			Assert.AreEqual("XXXX", updated.ResourceDto.Value);
			//check esli se nepridalo neco novyho 
			Assert.AreEqual(resourcesCountBefore, resourcesCountAfter);
			Assert.AreEqual(gridpageResBefore.Id, gridpageResAfter.Id);
		}

		[Test]
		public void GetGridPage()
		{
			var a = repo.GetGridPage(_defaultlink);
			Assert.IsNotNull(a);
			Assert.AreEqual("linkTestPage", a.ResourceDto.Value);
		}

		[Test]
		public void GetGridPage_no_link()
		{
			Assert.Throws<ObjectNotFoundException>(() => repo.GetGridPage("linkTestPageXXX"));
		}

	}
}