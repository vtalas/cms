using System;
using NUnit.Framework;
using cms.data.Files;
using System.Linq;

namespace cms.data.tests
{
	[TestFixture]
	public class JsonDataFilesTest
	{
		private JsonDataFiles db { get; set; }
		[TestFixtureSetUp]
		public void Setup()
		{
			db = new JsonDataFiles("aaa");
		}

		[Test]
		public void Applications()
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
		public void Application()
		{

			var list = db.GetApplication(1);
			Assert.IsNotNull(list);
		}
	}
}