using System;
using System.Data.Entity;
using System.Linq;
using NUnit.Framework;
using cms.data.EF;
using cms.data.DataProvider;
using cms.data.EF.DataProviderImplementation;
using cms.data.EF.Initializers;
using cms.data.tests.EF;

namespace cms.data.tests.SampleDataTest
{
	[TestFixture]
	public class CreateSampleDataTest
	{
		private DataEfAuthorized Repo { get; set; }

		[SetUp]
		[Ignore]
		public void Setup()
		{
			var context = new EfContext("EfContextSampleData");
			Database.SetInitializer(new DropAndCreateAlwaysForce());
			Repo = new DataEfAuthorized("aaa",  context, 1);
		}

		[Test]
		[Ignore]
		public void Create()
		{
			using (var db = new SessionManager().CreateSession)
			{
				var a = db.Applications();

				//zamerne fail
				Assert.AreEqual(-1, a.Count());
				Console.WriteLine(a.Count());
			}
		}
	}
}