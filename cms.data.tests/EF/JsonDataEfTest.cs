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
	[TestFixture]
	public class JsonDataEfTest
	{
		private JsonDataEf repo { get; set; }
		private string _defaultlink = "linkTestPage";
		EfContext _context;

		[SetUp]
		public void Setup()
		{
			_context = new EfContext();
			Database.SetInitializer(new DropAndCreate());
			repo = new JsonDataEf(new Guid("c78ee05e-1115-480b-9ab7-a3ab3c0f6643"), _context);
			Assert.IsNotNull(repo);
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
			var grid = repo.GetGrid(gridpage.Id);
			Assert.AreEqual(0, repo.GetGrid(gridpage.Id).GridElements.Count);

			var gridelem = new GridElement()
			{
				Content = "XXX",
				Line = 0,
				Position = 0,
				Width = 12,
				Type = "text",
			};

			gridelem.Grid.Add(grid);

			var newgridelem = repo.Add(gridelem);

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

			gridelem.Grid.Add(grid);

			var newgridelem = repo.Add(gridelem);

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

			gridelem.Grid.Add(grid);

			var newgridelem = repo.Add(gridelem);

			Assert.IsTrue(newgridelem.Resources.Any());
			Assert.AreEqual(2, newgridelem.Resources.Count(x => x.Key == "text1"));

			var resourcesCountAfter = _context.Resources.Count();
			Assert.AreEqual(resourcesCountBefore + 2, resourcesCountAfter);

		}

		[Test]
		public void UpdateGridElement_test()
		{
			var a = new GridElement
						{
							Content = "oldcontent",
							Width = 12,
							Line = 0,
							Skin = "xxx"
						};
			var newitem = repo.Add(a);

			Assert.True(newitem.Id != 0);

			newitem.Line = 1;
			newitem.Content = "newcontent";
			newitem.Width = 0;

			repo.Update(newitem);

			var updated = repo.GetGridElement(newitem.Id);

			Assert.AreEqual(1, updated.Line);
			Assert.AreEqual(0, updated.Width);
			Assert.AreEqual("newcontent", updated.Content);
		}

		[Test]
		public void UpdateGridElement_addNewResources()
		{
			var a = new GridElement { Content = "oldcontent", Width = 12, Line = 0, Skin = "xxx" };
			var newitem = repo.Add(a);
			Assert.True(newitem.Id != 0);

			var resources = new List<Resource>
			                	{
			                		new Resource {Culture = "cs", Value = "new cesky", Key = "text1"},
			                		new Resource {Culture = "en", Value = "new englicky", Key = "text1"},
			                	};

			newitem.Resources = resources;

			repo.Update(newitem);
			var updated = repo.GetGridElement(newitem.Id);
			Assert.AreEqual(2, updated.Resources.Count);
		}

		[Test]
		public void UpdateGridElement_AttachExistingResources()
		{
			var a = new GridElement { Content = "oldcontent", Width = 12, Line = 0, Skin = "xxx" };
			var newitem = repo.Add(a);
			var resourcesCountBefore = _context.Resources.Count();
			var existingsResBefore = _context.Resources.First();

			Assert.True(newitem.Id != 0);

			var resources = new List<Resource>
			                	{
									existingsResBefore,
			                		new Resource {Culture = "cs", Value = "new cesky", Key = "text1"},
								};

			newitem.Resources = resources;

			repo.Update(newitem);

			var updated = repo.GetGridElement(newitem.Id);
			var resourcesCountAfter = _context.Resources.Count();
			var existingsResAfter = _context.Resources.First(x => x.Id == existingsResBefore.Id);

			Assert.AreEqual(2, updated.Resources.Count);
			Assert.AreEqual(resourcesCountBefore + 1, resourcesCountAfter);
			Assert.AreEqual(existingsResBefore.Value, existingsResAfter.Value);
		}


		[Test]
		public void UpdateGridPage_test()
		{
			var gridpage = repo.Add(new GridPageDto
			{
				Name = "addgridElement test Gridpage",
				Resource = new ResourceDto { Value = "newlink", Culture = "xx" }
			});
			var resourcesCountBefore = _context.Resources.Count();
			var gridpageResBefore = gridpage.Resource;

			gridpage.Resource.Value = "XXXX";
			repo.Update(gridpage);

			var updated = repo.GetGridPage(gridpage.Id);
			var resourcesCountAfter = _context.Resources.Count();
			var gridpageResAfter = updated.Resource;

			Assert.AreEqual("XXXX", updated.Resource.Value);
			//check esli se nepridalo neco novyho 
			Assert.AreEqual(resourcesCountBefore, resourcesCountAfter);
			Assert.AreEqual(gridpageResBefore.Id, gridpageResAfter.Id);
		}

		[Test]
		public void GetGridPage()
		{
			var a = repo.GetGridPage(_defaultlink);
			Assert.IsNotNull(a);
			Assert.AreEqual("linkTestPage", a.Resource.Value);
		}

		[Test]
		public void GetGridPage_no_link()
		{
			Assert.Throws<ObjectNotFoundException>(() => repo.GetGridPage("linkTestPageXXX"));
		}






	}
}