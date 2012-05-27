using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using NUnit.Framework;
using cms.data.Dtos;
using cms.data.EF;
using cms.data.EF.Initializers;
using cms.data.Models;

namespace cms.data.tests.EF
{
	[TestFixture]
	public class JsonDataEfTest
	{
		private JsonDataEf repo { get; set; }
		
		private IList<ApplicationSettingDto> AplicationSettings()
		{
			return JsonDataEf.Applications().ToList();
		}

		private IList<Grid> Grids()
		{
			return repo.Grids().ToList();
		}
		private Grid Grid()
		{
			return repo.Grids().Single(x => x.Id == 1);
		}
			
		[SetUp]
		public void Setup()
		{
			var context = new EfContext();
			Database.SetInitializer(new DropAndCreate());
			repo = new JsonDataEf("aaa", context);
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
			var a = new ApplicationSetting()
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
			var grid = repo.GetGrid(1);

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

			Console.WriteLine(newgridelem.Grid.Count);
			Console.WriteLine(repo.GetGrid(1).GridElements.Count);

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
			Console.WriteLine(updated.Content);







		}
		
		[Test]
		public void GetGridPage()
		{
			var a = repo.GetGridPage("linkTestPage");
		}

		[Test]
		public void GetGridPage_no_link()
		{
			Assert.Throws<ObjectNotFoundException>(() => repo.GetGridPage("linkTestPageXXX"));
		}






	}
}