using System.Data.Entity;
using System.Linq;
using NUnit.Framework;
using WebMatrix.WebData;
using cms.data.EF.Initializers;
using cms.data.Shared.Models;

namespace cms.data.tests.EF
{
	[TestFixture]
	public class ApplicationsMethodTest
	{
		[SetUp]
		public void SetInit()
		{
			Xxx.DeleteDatabaseDataGenereateSampleData();
			Database.SetInitializer(new DropAndCreate());
		}

		[Test]
		public void Application_add_test()
		{
			var a = new ApplicationSetting { Name = "xxx" };
			using (var db = new SessionManager().CreateSession)
			{
				Assert.AreEqual(1, db.Applications().Count());

				var newitem = db.Add(a, WebSecurity.GetUserId("admin"));
				var list = db.Applications();

				Assert.AreEqual(2, list.Count());
				Assert.IsTrue(list.Any(x => x.Id == newitem.Id));
			}
		}
	}
}