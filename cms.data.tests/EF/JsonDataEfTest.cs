using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using NUnit.Framework;
using cms.data.EF;
using cms.data.Models;

namespace cms.data.tests.EF
{
	[TestFixture]
	public class JsonDataEfTest
	{
		private JsonDataEf db { get; set; }


		private IList<ApplicationSetting> AplicationSettings()
		{
			return db.Applications().ToList();
		}

		private IList<Grid> Grids()
		{
			return db.Grids().ToList();
		}
			
		[SetUp]
		public void Setup()
		{
			var context = new EfContext();
			Database.SetInitializer(new DropAndCreate());
			db = new JsonDataEf("aaa", context);
		}

		[Test]
		public void Applications_test()
		{
			var list = db.Applications();

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
			var newitem = db.Add(a);
			Console.WriteLine(newitem.Name);

			Assert.IsNotNull(db.GetApplication(newitem.Id));

		}

	}
}