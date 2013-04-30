using System.Data.Entity;
using System.Linq;
using NUnit.Framework;
using WebMatrix.WebData;
using cms.data.EF;
using cms.data.EF.Initializers;
using cms.data.Shared.Models;

namespace cms.data.tests.EF
{
	[TestFixture]
	public class ApplicationsMethodTest
	{
		private SessionProvider SessionProvider { get; set; }

		[SetUp]
		public void SetInit()
		{
			Xxx.DeleteDatabaseDataGenereateSampleData();
			Database.SetInitializer(new DropAndCreateAlwaysForce());
			SessionProvider = SessionProviderFactoryTest.GetSessionProvider();
		}

		[Test]
		public void Application_add_test()
		{
			var a = new ApplicationSetting { Name = "xxx" };
			using (var db = SessionProvider.CreateSession())
			{
				Assert.AreEqual(1, db.Session.Applications().Count());

				var newitem = db.Session.Add(a, WebSecurity.GetUserId("admin"));
				var list = db.Session.Applications();

				Assert.AreEqual(2, list.Count());
				Assert.IsTrue(list.Any(x => x.Id == newitem.Id));
			}
		}
	}
}