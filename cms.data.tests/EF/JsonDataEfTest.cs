using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using NUnit.Framework;
using cms.data.Dtos;
using cms.data.EF;
using cms.data.EF.DataProvider;
using cms.data.EF.Initializers;
using cms.data.Shared.Models;
using cms.shared;

namespace cms.data.tests.EF
{

	public static class Helpers{

		public static Resource getresource(this IEnumerable<Resource> source, string culture, string key )
		{
			return source.Single(x => x.Culture == culture && x.Key == key);
		}
	
		public static Guid guid = new Guid("c78ee05e-1115-480b-9ab7-a3ab3c0f6643");
	
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

		public static string CurrentCulture{get { return shared.SharedLayer.Culture; }
		}
		public static string _defaultlink = "linkTestPage";

	}

	[TestFixture]
	public class JsonDataEfTest
	{
		private JsonDataEf repo { get; set; }
		EfContext _context;

		[SetUp]
		public void Setup()
		{
			_context = new EfContext();
			Database.SetInitializer(new DropAndCreate());
			repo = new JsonDataEf(Helpers.guid, _context);
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
						ResourceDto = new ResourceDtoLoc { Value = link },
						Category = CategoryEnum.Page
					};

					var n = db.Page.Add(a);
					Assert.AreEqual(name, n.Name);
					Assert.AreEqual(link, n.ResourceDto.Value);
					return n;
				}
				
			}

			GridPageDto GetGridpage(Guid id )
			{
				using (var db = SessionManager.CreateSession)
				{
					var n = db.Page.Get(id);
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
		public class GetGridElement_MethodTest
		{
			[SetUp]
			public void Init()
			{
				shared.SharedLayer.Init();
			}

			[Test]
			public void CultureBasic()
			{
				Assert.AreEqual("cs",shared.SharedLayer.Culture);	

				var xxx = Helpers.AddDefaultGridElement();
				using (var db = SessionManager.CreateSession)
				{
					var a = db.GetGridElement(xxx.Id);

					Assert.IsNotNull(a);
					Assert.AreEqual(1, a.Resources.Count);
					Assert.AreEqual("cesky", a.Resources.First().Value);
				}
			}

			[Test]
			public void CultureSwitchCulture()
			{
				Assert.AreEqual("cs", shared.SharedLayer.Culture);
				shared.SharedLayer.Culture = "en";
				Assert.AreEqual("en",shared.SharedLayer.Culture);	

				var xxx = Helpers.AddDefaultGridElement();
				using (var db = SessionManager.CreateSession)
				{
					var a = db.GetGridElement(xxx.Id);

					Assert.IsNotNull(a);
					Assert.AreEqual(1, a.Resources.Count);
					Assert.AreEqual("englicky", a.Resources.First().Value);
				}
			}
		}

		[TestFixture]
		public class GetGridPage_MethodTest
		{
			[SetUp]
			public void init()
			{
				shared.SharedLayer.Init();
				Assert.AreEqual("cs",shared.SharedLayer.Culture);
			}

			[Test]
			public void Basic()
			{
				using (var db = SessionManager.CreateSession)
				{
					var a = db.Page.Get(Helpers._defaultlink);
					Assert.IsNotNull(a);
					Assert.AreEqual(Helpers._defaultlink, a.ResourceDto.Value);
				}
			}

			[Test]
			public void No_link()
			{
				using (var db = SessionManager.CreateSession)
				{
					Assert.Throws<ObjectNotFoundException>(() => db.Page.Get("linkTestPageXXX"));
				}
			}


		}

		[TestFixture]
		public class Update_GridElement_MethodTest 
		{
			EfContext _context;

			[SetUp]
			public void Setup()
			{
				shared.SharedLayer.Init();
				Database.SetInitializer(new DropAndCreate());
				_context = new EfContext();
			}

			[Test]
			public void Basics_test()
			{
				using (var db = SessionManager.CreateSession)
				{
					var newitem = Helpers.AddDefaultGridElement();

					newitem.Line = 1;
					newitem.Content = "newcontent";
					newitem.Width = 0;
					newitem.Position = 111;
					newitem.Skin = "aaa";

					db.Update(newitem.ToDto());

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

					db.Update(newitem.ToDto());

					var updated = db.GetGridElement(newitem.Id);

					Assert.AreEqual(1, updated.Resources.Count);
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
					var a = new GridElement { Content = "oldcontent", Width = 12, Line = 0, Skin = "xxx" };
					newitem = db.AddGridElementToGrid(a, Helpers.guid);
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

					db.Update(newitem.ToDto());
					var updated = db.GetGridElement(newitem.Id);
					Assert.AreEqual(1, updated.Resources.Count);
				}
			}

			[Test]
			public void addNewResources_withUniqueKey()
			{
				var resourcesCountBefore = _context.Resources.Count();

				//Resources + 2
				GridElement newitem = Helpers.AddDefaultGridElement();

				//Resources + 1
				newitem.Resources.Add(new Resource { Culture = "cs", Value = "new cesky", Key = "text1xx" });

				using (var db = SessionManager.CreateSession)
				{
					db.Update(newitem.ToDto());
					var updated = db.GetGridElement(newitem.Id);
					//text1, text1xx
					Assert.AreEqual(2, updated.Resources.Count);

					Assert.AreEqual(resourcesCountBefore + 3, _context.Resources.Count());
				}
			}

			[Test]
			public void UpdateGridElement_addNewResources_keyExist()
			{
				var resourcesCountBefore = _context.Resources.Count();

				GridElement newitem = Helpers.AddDefaultGridElement();

				newitem.Resources.Add(new Resource { Culture = "cs", Value = "new cesky", Key = "text1" });
				newitem.Resources.Add(new Resource { Culture = "en", Value = "new cesky", Key = "text1" });

				using (var db = SessionManager.CreateSession)
				{
					Assert.Throws<ArgumentException>(() => db.Update(newitem.ToDto()));
					Assert.AreEqual(resourcesCountBefore + 2, _context.Resources.Count());
				}
			}
			[Test]
			public void UpdateAttachedResource()
			{
				var g1 = Helpers.AddDefaultGridElement();
				var resourcesCountBefore = _context.Resources.Count();
				var res1 = g1.Resources.First();

				Assert.AreEqual("cs", res1.Culture);
				Assert.AreEqual("text1", res1.Key);

				res1.Value = "prd";
				res1.Culture = "cs";
				res1.Key = "kkk";
				using (var db = SessionManager.CreateSession)
				{

					db.Update(g1.ToDto());
					var updated = db.GetGridElement(g1.Id);
					var res1Updated = updated.Resources.First();

					Assert.AreEqual(1, updated.Resources.Count);
					Assert.AreEqual("prd", res1Updated.Value);

					//u resourcu se da zmenit jenom value
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
					db.Update(g1.ToDto());
					var updated = db.GetGridElement(g1.Id);

					Assert.AreEqual(2, updated.Resources.Count);
					Assert.AreEqual(resourcesCountBefore + 1, _context.Resources.Count());
				}
			}

			[Test]
			//INFO: Resources - pokud je neexistujici Id tak prida novy zaznam 
			public void UpdateGridElement_TryAttachNonExistingResources()
			{
				var gridelementdb = Helpers.AddDefaultGridElement();
				var resourcesCountBefore = _context.Resources.Count();
				gridelementdb.Resources.Add(new Resource { Id = 111, Culture = "cs", Value = "non existing",Key = "text1AA"});

				using (var db = SessionManager.CreateSession)
				{
					db.Update(gridelementdb.ToDto());
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

				gridelementdb1.Resources.Add(new Resource { Id = res2.Id, Culture = "cs", Value = "reference na resource z 1", Key = "text1AA" });
				using (var db = SessionManager.CreateSession)
				{

					db.Update(gridelementdb1.ToDto());

					var updated = db.GetGridElement(gridelementdb1.Id);

					Assert.AreEqual(2, updated.Resources.Count);
				}
				Assert.AreEqual(resourcesCountBefore, _context.Resources.Count());
			}

			[Test]
			public void TryAttachExistingResourceWithNoKey_ShouldBeIgnored()
			{
				var resourcesCountBefore = _context.Resources.Count();
				GridElement gridelementdb;

				using (var db = SessionManager.CreateSession)
				{
					var a = new GridElement { Content = "oldcontent", Width = 12, Line = 0, Skin = "xxx" };
					gridelementdb = db.AddGridElementToGrid(a, Helpers.guid);
				}

				using (var db = SessionManager.CreateSession)
				{
					var existingsResBefore = _context.Resources.First();
					Assert.IsNull(existingsResBefore.Key);

					var resources = new List<Resource>
			                            {
			                                existingsResBefore,
			                            };

					gridelementdb.Resources = resources;
					
					db.Update(gridelementdb.ToDto());
					var updated = db.GetGridElement(gridelementdb.Id);
					Assert.AreEqual(0, updated.Resources.Count);
					Assert.AreEqual(resourcesCountBefore, _context.Resources.Count());
				}
			}

		}


		[TestFixture]
		public class Applications_MethodTest
		{
			[Test]
			public void Applications_test()
			{

				using (var db = SessionManager.CreateSession)
				{
					var list = db.Applications();

					foreach (var item in list)
					{
						Console.WriteLine(item.Name);
					}
					Assert.IsNotNull(list);
					Assert.IsTrue(list.Any());
				}
			}

			[Test]
			public void Application_add_test()
			{
				var a = new ApplicationSetting
				{
					Name = "xxx",
				};

				using (var db = SessionManager.CreateSession)
				{
					var newitem = db.Add(a);
					Console.WriteLine(newitem.Name);
					Assert.IsNotNull(db.GetApplication(newitem.Id));
				}

			}

		}

		[TestFixture]
		public class Add_GridElement_MethodTest
		{
			EfContext _context;

			[SetUp]
			public void Setup()
			{
				Database.SetInitializer(new DropAndCreate());
				_context = new EfContext();
			}

			[Test]
			public void AddGridElement()
			{
				using (var db = SessionManager.CreateSession)
				{

					var gridpage = db.Page.Add(new GridPageDto {Name = "addgridElement test Gridpage", Category = CategoryEnum.Page});
					var gridDb = db.GetGrid(gridpage.Id);
					Assert.AreEqual(0, db.GetGrid(gridpage.Id).GridElements.Count);


					var gridelem = new GridElement()
					               	{
					               		Content = "XXX",
					               		Line = 0,
					               		Position = 0,
					               		Width = 12,
					               		Type = "text",
					               	};

					var newgridelem = db.AddGridElementToGrid(gridelem, gridDb.Id);

					Assert.AreEqual(1, newgridelem.Grid.Count);
					Assert.AreEqual(1, db.GetGrid(gridpage.Id).GridElements.Count);
				}
			}

			[Test]
			public void AddGridElement_AddNewResources_test_same_keys()
			{
				using (var db = SessionManager.CreateSession)
				{

					var gridpage = db.Page.Get(Helpers._defaultlink);
					var grid = db.GetGrid(gridpage.Id);

					var gridelem = new GridElement
					               	{
					               		Content = "XXX",
					               		Line = 0,
					               		Position = 0,
					               		Width = 12,
					               		Type = "text",
					               		Resources = new List<Resource>
					               		            	{
					               		            		new Resource {Culture = "cs", Value = "cesky", Key = "text1"},
					               		            		new Resource {Culture = "en", Value = "englicky", Key = "text1"},
					               		            	}
					               	};


					var newgridelem = db.AddGridElementToGrid(gridelem, grid.Id);

					Assert.IsTrue(newgridelem.Resources.Any());
					Assert.AreEqual(2, newgridelem.Resources.Count(x => x.Key == "text1"));
				}
			}

			[Test]
			public void AddGridElement_AddNew_and_ExistingResources_test_same_keys()
			{
				using (var db = SessionManager.CreateSession)
				{
					var gridpage = db.Page.Get(Helpers._defaultlink);
					var grid = db.GetGrid(gridpage.Id);
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
					               		            		new Resource {Culture = "cs", Value = "cesky", Key = "text1"},
					               		            		new Resource {Culture = "en", Value = "englicky", Key = "text1"},
					               		            		existingsRes
					               		            	}
					               	};

					var newgridelem = db.AddGridElementToGrid(gridelem, grid.Id);

					Assert.IsTrue(newgridelem.Resources.Any());
					Assert.AreEqual(2, newgridelem.Resources.Count(x => x.Key == "text1"));

					var resourcesCountAfter = _context.Resources.Count();
					Assert.AreEqual(resourcesCountBefore + 2, resourcesCountAfter);
				}
			}
		}



		[Test]
		public void UpdateGridPage_test()
		{
			var gridpage = repo.Page.Add(new GridPageDto
			{
				Name = "addgridElement test Gridpage",
				ResourceDto = new ResourceDtoLoc() { Value = "newlink" },
				Category = CategoryEnum.Page
			});
			var resourcesCountBefore = _context.Resources.Count();
			var gridpageResBefore = gridpage.ResourceDto;

			gridpage.ResourceDto.Value = "XXXX";
			repo.Update(gridpage);

			var updated = repo.Page.Get(gridpage.Id);
			var resourcesCountAfter = _context.Resources.Count();
			var gridpageResAfter = updated.ResourceDto;

			Assert.AreEqual("XXXX", updated.ResourceDto.Value);
			//check esli se nepridalo neco novyho 
			Assert.AreEqual(resourcesCountBefore, resourcesCountAfter);
			Assert.AreEqual(gridpageResBefore.Id, gridpageResAfter.Id);
		}


	}
}