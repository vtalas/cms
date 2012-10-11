using System;
using System.Data.Entity;
using System.Linq;
using NUnit.Framework;
using cms.data.EF.DataProvider;
using cms.data.EF.Initializers;

namespace cms.data.tests.EF
{
	[TestFixture]
	public class DataEfTestNEW
	{
		[SetUp]
		public void Setup()
		{
			Xxx.DeleteDatabaseDataGenereateSampleData();
			Database.SetInitializer(new DropAndCreate());
		}

		[Test]
		public void ValidUserInstance_test()
		{
			using (var context = new SessionManager().CreateSession)
			{
				Assert.IsNotNull(context);
				Assert.AreEqual(1, context.Applications().Count());
				Assert.IsNotNull(context.CurrentApplication);
			}
		}

		[Test]
		public void InvalidUserNonExistingInstance_test_AppId()
		{
			using (var context = new SessionManager().CreateSessionWithInvalidUser)
			{
				Assert.IsNotNull(context);
				Assert.Throws<Exception>(delegate { var xxx = context.CurrentApplication; });
				Assert.AreEqual(0, context.Applications().Count());
			}
		}

		[Test]
		public void InvalidUserNonExistingInstance_test_AppName()
		{
			using (var context = new DataEfAuthorized("test1", 0))
			{
				Assert.IsNotNull(context);
				Assert.Throws<Exception>(delegate { var xxx = context.CurrentApplication; });
				Assert.AreEqual(0, context.Applications().Count());
			}
		}
	}
}