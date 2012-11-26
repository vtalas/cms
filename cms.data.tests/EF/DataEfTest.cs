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
	[TestFixture]
	public class PageAbstractTest
	{
		[SetUp]
		public void Setup()
		{
			Database.SetInitializer(new DropAndCreate());
		}

		[Test]
		public void List_test()
		{
			using (var db = SessionManager.CreateSessionWithSampleData)
			{
				Assert.IsNotNull(db.CurrentApplication);
				var page = new PageAbstractImpl(db.CurrentApplication, db.db);

				var list = page.List();
				Assert.IsTrue(list.Any());
				Assert.IsFalse(list.Any(x=>x.Category == CategoryEnum.Menu));
			}
		}

		[Test]
		public void Add_test()
		{
			var a = new GridPageDto();
				        {
							
				        };
			using (var db = SessionManager.CreateSessionWithSampleData)
			{
				var page = new PageAbstractImpl(db.CurrentApplication, db.db);
				var countBefore = page.List().Count();
				var newitem = page.Add(a);
				Assert.IsNotNull(newitem.Id);
				Assert.AreEqual(page.List().Count(), countBefore + 1 );
			}
		}
	}

	[TestFixture]
	public class DataEfTestNEW
	{
		[SetUp]
		public void Setup()
		{
			Database.SetInitializer(new DropAndCreate());
		}

		[Test]
		public void ValidUserInstance_test()
		{
			using (var context = new DataEfAuthorized(new Guid("c78ee05e-1115-480b-9ab7-a3ab3c0f6643"), 1))
			{
				Assert.IsNotNull(context);
				Assert.AreEqual(1, context.Applications().Count());
				Assert.IsNotNull(context.CurrentApplication);
			}
		}

		[Test]
		public void InvalidUserNonExistingInstance_test_AppId()
		{
			using (var context = new DataEfAuthorized(new Guid("c78ee05e-1115-480b-9ab7-a3ab3c0f6643"), 0))
			{
				Assert.IsNotNull(context);
				Assert.Throws<Exception>(delegate {var xxx = context.CurrentApplication; });
				Assert.AreEqual(0, context.Applications().Count());
			}
		}

		[Test]
		public void InvalidUserNonExistingInstance_test_AppName()
		{
			using (var context = new DataEfAuthorized("test1", 0))
			{
				Assert.IsNotNull(context);
				Assert.Throws<Exception>(delegate {var xxx = context.CurrentApplication; });
				Assert.AreEqual(0, context.Applications().Count());
			}
		}


	}


	[TestFixture]
	public class DataEfTest
	{
		private DataEfAuthorized repo { get; set; }
		EfContext _context;

		[SetUp]
		public void Setup()
		{
			_context = new EfContext();
			Database.SetInitializer(new DropAndCreate());
			repo = new DataEfAuthorized(DataEfHelpers.guid, _context, 1);
			Assert.IsNotNull(repo);
		}

		[TestFixture]
		public class AddGridPageMethodTest
		{
			public AddGridPageMethodTest()
			{
				Database.SetInitializer(new DropAndCreate());
			}

			GridPageDto AddGridpage(string name, string link)
			{
				using (var db = SessionManager.CreateSessionWithSampleData)
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

			GridPageDto GetGridpage(Guid id)
			{
				using (var db = SessionManager.CreateSessionWithSampleData)
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
				Assert.AreEqual("cs", SharedLayer.Culture);

				var xxx = DataEfHelpers.AddDefaultGridElement();
				using (var db = SessionManager.CreateSessionWithSampleData)
				{
					var a = db.GridElement.Get(xxx.Id);

					Assert.IsNotNull(a);
					Assert.AreEqual(1, a.ResourcesLoc.Count);
					Assert.AreEqual("cesky", a.ResourcesLoc.First().Value);
				}
			}

			[Test]
			public void CultureSwitchCulture()
			{
				Assert.AreEqual("cs", shared.SharedLayer.Culture);
				shared.SharedLayer.Culture = "en";
				Assert.AreEqual("en", shared.SharedLayer.Culture);

				var xxx = DataEfHelpers.AddDefaultGridElement();
				using (var db = SessionManager.CreateSessionWithSampleData)
				{
					var a = db.GridElement.Get(xxx.Id);

					Assert.IsNotNull(a);
					Assert.AreEqual(1, a.ResourcesLoc.Count);
					Assert.AreEqual("englicky", a.ResourcesLoc.First().Value);
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
				Assert.AreEqual("cs", shared.SharedLayer.Culture);
			}

			[Test]
			public void Basic()
			{
				using (var db = SessionManager.CreateSessionWithSampleData)
				{
					var a = db.Page.Get(DataEfHelpers._defaultlink);
					Assert.IsNotNull(a);
					Assert.AreEqual(DataEfHelpers._defaultlink, a.ResourceDto.Value);
				}
			}

			[Test]
			public void No_link()
			{
				using (var db = SessionManager.CreateSessionWithSampleData)
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
				using (var db = SessionManager.CreateSessionWithSampleData)
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
				using (var db = SessionManager.CreateSessionWithSampleData)
				{
					var newitem = DataEfHelpers.AddDefaultGridElement();
					var resourcesCountBefore = _context.Resources.Count();

					newitem.Resources.ToList()[0].Value = "xxx";
					newitem.Resources.ToList()[0].Key = "new cs";

					db.GridElement.Update(newitem.ToDto());

					var updated = db.GridElement.Get(newitem.Id);

					Assert.AreEqual(1, updated.ResourcesLoc.Count);
					Assert.AreEqual(resourcesCountBefore, _context.Resources.Count());
					Assert.IsNotNull(updated.ResourcesLoc.SingleOrDefault(x => x.Value.Value == "xxx"));


					Assert.AreNotEqual("new cs", updated.ResourcesLoc.Single(x => x.Value.Value == "xxx").Key);
				}
			}

			[Test]
			public void UpdateGridElement_AddNewResources()
			{

				GridElement newitem;
				using (var db = SessionManager.CreateSessionWithSampleData)
				{
					var a = new GridElement { Content = "oldcontent", Width = 12, Position = 0, Skin = "xxx" };
					newitem = db.GridElement.AddToGrid(a, DataEfHelpers.guid);
					Assert.True(!newitem.Id.IsEmpty());
				}

				using (var db = SessionManager.CreateSessionWithSampleData)
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
				var resourcesCountBefore = _context.Resources.Count();

				//Resources + 2
				GridElement newitem = DataEfHelpers.AddDefaultGridElement();

				//Resources + 1
				newitem.Resources.Add(new Resource { Culture = "cs", Value = "new cesky", Key = "text1xx" });

				using (var db = SessionManager.CreateSessionWithSampleData)
				{
					db.GridElement.Update(newitem.ToDto());
					var updated = db.GridElement.Get(newitem.Id);
					//text1, text1xx
					Assert.AreEqual(2, updated.ResourcesLoc.Count);

					Assert.AreEqual(resourcesCountBefore + 3, _context.Resources.Count());
				}
			}

			[Test]
			public void UpdateGridElement_addNewResources_keyExist()
			{
				var resourcesCountBefore = _context.Resources.Count();

				GridElement newitem = DataEfHelpers.AddDefaultGridElement();

				newitem.Resources.Add(new Resource { Culture = "cs", Value = "new cesky", Key = "text1" });
				newitem.Resources.Add(new Resource { Culture = "en", Value = "new cesky", Key = "text1" });

				using (var db = SessionManager.CreateSessionWithSampleData)
				{
					Assert.Throws<ArgumentException>(() => db.GridElement.Update(newitem.ToDto()));
					Assert.AreEqual(resourcesCountBefore + 2, _context.Resources.Count());
				}
			}
			[Test]
			public void UpdateAttachedResource()
			{
				var g1 = DataEfHelpers.AddDefaultGridElement();
				var resourcesCountBefore = _context.Resources.Count();
				var res1 = g1.Resources.First();

				Assert.AreEqual("cs", res1.Culture);
				Assert.AreEqual("text1", res1.Key);

				res1.Value = "prd";
				res1.Culture = "cs";
				res1.Key = "kkk";
				using (var db = SessionManager.CreateSessionWithSampleData)
				{

					db.GridElement.Update(g1.ToDto());
					var updated = db.GridElement.Get(g1.Id);
					var res1Updated = updated.ResourcesLoc.First();

					Assert.AreEqual(1, updated.ResourcesLoc.Count);
					Assert.AreEqual("prd", res1Updated.Value.Value);

					//u resourcu se da zmenit jenom value
					Assert.AreNotEqual("kkk", res1Updated.Key);
					Assert.AreEqual(resourcesCountBefore, _context.Resources.Count());
				}
			}

			[Test]
			public void UpdateGridElement_AddNewResource()
			{
				var g1 = DataEfHelpers.AddDefaultGridElement();
				var resourcesCountBefore = _context.Resources.Count();
				var a = new Resource
				{
					Value = "xxx",
					Key = "key1"
				};
				g1.Resources.Add(a);

				using (var db = SessionManager.CreateSessionWithSampleData)
				{
					db.GridElement.Update(g1.ToDto());
					var updated = db.GridElement.Get(g1.Id);

					Assert.AreEqual(2, updated.ResourcesLoc.Count);
					Assert.AreEqual(resourcesCountBefore + 1, _context.Resources.Count());
			
				}
			}

			[Test]
			//INFO: Resources - pokud je neexistujici Id tak prida novy zaznam 
			public void UpdateGridElement_TryAttachNonExistingResources()
			{
				var gridelementdb = DataEfHelpers.AddDefaultGridElement();
				var resourcesCountBefore = _context.Resources.Count();
				gridelementdb.Resources.Add(new Resource { Id = 111, Culture = "cs", Value = "non existing", Key = "text1AA" });

				using (var db = SessionManager.CreateSessionWithSampleData)
				{
					db.GridElement.Update(gridelementdb.ToDto());
				}
				Assert.AreEqual(resourcesCountBefore + 1, _context.Resources.Count());
			}

			[Test]
			public void UpdateGridElement_AttachExistingResources_sadsdXXXXXXXXXXXXXXXXXX()
			{
				var gridelementdb1 = DataEfHelpers.AddDefaultGridElement();
				var gridelementdb2 = DataEfHelpers.AddDefaultGridElement();

				var resourcesCountBefore = _context.Resources.Count();
				var res2 = gridelementdb2.Resources.First();

				gridelementdb1.Resources.Add(new Resource { Id = res2.Id, Culture = "cs", Value = "reference na resource z 1", Key = "text1AA" });
				using (var db = SessionManager.CreateSessionWithSampleData)
				{

					db.GridElement.Update(gridelementdb1.ToDto());

					var updated = db.GridElement.Get(gridelementdb1.Id);

					Assert.AreEqual(2, updated.ResourcesLoc.Count);
				}
				Assert.AreEqual(resourcesCountBefore, _context.Resources.Count());
			}

			[Test]
			public void TryAttachExistingResourceWithNoKey_ShouldBeIgnored()
			{
				var resourcesCountBefore = _context.Resources.Count();
				GridElement gridelementdb;

				using (var db = SessionManager.CreateSessionWithSampleData)
				{
					var a = new GridElement { Content = "oldcontent", Width = 12, Position = 0, Skin = "xxx" };
					gridelementdb = db.GridElement.AddToGrid(a, DataEfHelpers.guid);
				}

				using (var db = SessionManager.CreateSessionWithSampleData)
				{
					var existingsResBefore = _context.Resources.First();
					Assert.IsNull(existingsResBefore.Key);

					var resources = new List<Resource>
			                            {
			                                existingsResBefore,
			                            };

					gridelementdb.Resources = resources;

					db.GridElement.Update(gridelementdb.ToDto());
					var updated = db.GridElement.Get(gridelementdb.Id);
					Assert.AreEqual(0, updated.ResourcesLoc.Count);
					Assert.AreEqual(resourcesCountBefore, _context.Resources.Count());
				}
			}

		}


		[TestFixture]
		public class Applications_MethodTest
		{
			[Test]
			public void Application_add_test()
			{
				var a = new ApplicationSetting
				{
					Name = "xxx",
				};

				using (var db = SessionManager.CreateSessionWithSampleData)
				{
					Assert.AreEqual(1, db.Applications().Count());
					
					var newitem = db.Add(a, 1);
					var list = db.Applications();
					
					Assert.AreEqual(2,list.Count());
					Assert.IsTrue(list.Any(x=>x.Id == newitem.Id));
					
				}

			}

		}

		[TestFixture]
		public class Add_GridElement_MethodTest
		{
			EfContext _context;

			public Add_GridElement_MethodTest()
			{
				Database.SetInitializer(new DropAndCreate());
				_context = new EfContext();
			}

			[Test]
			public void AddGridElement()
			{
				using (var db = SessionManager.CreateSessionWithSampleData)
				{

					var gridpage = db.Page.Add(new GridPageDto { Name = "addgridElement test Gridpage", Category = CategoryEnum.Page });
					var gridDb = db.Page.Get(gridpage.Id);
					//Assert.AreEqual(0, db.Page.Get(gridpage.Id).GridElements.Count);
					Assert.AreEqual(0, -11);


					var gridelem = new GridElement()
									{
										Content = "XXX",
										Position = 0,
										Width = 12,
										Type = "text",
									};

					var newgridelem = db.GridElement.AddToGrid(gridelem, gridDb.Id);

					Assert.AreEqual(1, newgridelem.Grid.Count);
					//Assert.AreEqual(1, db.Page.Get(gridpage.Id).GridElements.Count);
					Assert.AreEqual(1, -111);
				}
			}

			[Test]
			public void AddGridElement_AddNewResources_test_same_keys()
			{
				using (var db = SessionManager.CreateSessionWithSampleData)
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


					var newgridelem = db.GridElement.AddToGrid(gridelem, grid.Id);

					Assert.IsTrue(newgridelem.Resources.Any());
					Assert.AreEqual(2, newgridelem.Resources.Count(x => x.Key == "text1"));
				}
			}

			[Test]
			public void AddGridElement_AddNew_and_ExistingResources_test_same_keys()
			{
				using (var db = SessionManager.CreateSessionWithSampleData)
				{
					var gridpage = db.Page.Get(DataEfHelpers._defaultlink);
					var grid = db.Page.Get(gridpage.Id);
					var resourcesCountBefore = _context.Resources.Count();
					var existingsRes = _context.Resources.First();

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
					               		            		existingsRes
					               		            	}
									};

					var newgridelem = db.GridElement.AddToGrid(gridelem, grid.Id);

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
			repo.Page.Update(gridpage);

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