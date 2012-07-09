using System;
using System.Configuration;
using System.IO;
using NUnit.Framework;
using cms.Code.Bootstraper;
using cms.data.Files;
using System.Linq;
using cms.data.Migrations;

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
		public void SettingsTest()
		{
			var x = ConfigurationManager.AppSettings.Get("DefaultJsonBootstrap");
			Console.WriteLine(Path.GetFullPath(x));
			Assert.IsTrue(File.Exists(x));
		}

		[Test]
		public void Guidtest()
		{
			var a = "749acbca-8917-4ddf-9b43-0dbb6078d74c";
			var x = "749acbca89174ddf9b430dbb6078d74c";
			Console.WriteLine(new Guid(a));
			Console.WriteLine(new Guid(x).ToString("N"));
		}
	}
}