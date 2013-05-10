using System;
using System.Data.Entity;
using System.Linq;
using NUnit.Framework;
using cms.data.EF;
using cms.data.EF.DataProviderImplementation;
using cms.data.EF.Initializers;
using cms.data.Shared;

namespace cms.data.tests.EF
{
	[TestFixture]
	public class DataEfTestNEW
	{
		private SessionProvider SessionProvider { get; set; }

		[SetUp]
		public void Setup()
		{
			Xxx.DeleteDatabaseDataGenereateSampleData();
			Database.SetInitializer(new DropAndCreateAlwaysForce());

			SessionProvider = SessionProviderFactoryTest.GetSessionProvider();
		}

		[Test]
		public void ValidUserInstance_test()
		{
			using (var context = SessionProvider.CreateSession())
			{
				Assert.IsNotNull(context);
				Assert.AreEqual(1, context.Session.Applications().Count());
				Assert.IsNotNull(context.Session.CurrentApplication);
			}
		}

	
	}
}