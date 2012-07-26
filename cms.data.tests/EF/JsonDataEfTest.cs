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

		[SetUp]
		public void Setup()
		{
			var context = new EfContext();
			Database.SetInitializer(new DropAndCreate());
			repo = new JsonDataEf(new Guid("c78ee05e-1115-480b-9ab7-a3ab3c0f6643"), context);
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
			var gridpage = repo.GetGridPage(_defaultlink);
			var grid = repo.GetGrid(gridpage.Id);

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
			Assert.AreEqual(2, repo.GetGrid(gridpage.Id).GridElements.Count);

		}
	
		[Test]
		public void AddGridElement_resources_test_same_keys()
		{
			var gridpage = repo.GetGridPage(_defaultlink);
			var grid = repo.GetGrid(gridpage.Id);

			var gridelem = new GridElement
			{
				Content = "XXX",Line = 0,Position = 0,Width = 12,Type = "text",
				Resources = new List<Resource>
				            	{
				            		new Resource{ Culture = "cs", Value = "cesky", Key = "text1"},
									new Resource{ Culture = "en", Value = "englicky", Key = "text1"},
				            	}
			};

			gridelem.Grid.Add(grid);
			
			var newgridelem = repo.Add(gridelem);

			Assert.IsTrue(newgridelem.Resources.Any());
			Assert.AreEqual(2,newgridelem.Resources.Count(x => x.Key == "text1"));

		}
	
		[Test]
		public void UpdateGridElement()
		{
			var a = new GridElement
			        	{
			        		Content = "oldcontent",
			        		Width = 12,
			        		Line = 0,
			        		Skin = "xxx"
			        	};
			var newitem = repo.Add(a);

			Assert.True(newitem.Id !=0);

			newitem.Line = 1;
			newitem.Content = "newcontent";
			newitem.Width  = 0;

			repo.Update(newitem);

			var updated = repo.GetGridElement(newitem.Id);

			Console.WriteLine(updated.Id);
			Console.WriteLine(updated.Line);
			Console.WriteLine(updated.Width);
			Assert.AreEqual( 1,updated.Line);
			Assert.AreEqual( 0,updated.Width);
			Assert.AreEqual( "newcontent",updated.Content);
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