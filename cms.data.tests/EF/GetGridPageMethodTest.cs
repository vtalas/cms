using System.Data;
using NUnit.Framework;
using cms.data.EF;
using cms.data.EF.Initializers;
using cms.data.Shared;
using cms.shared;

namespace cms.data.tests.EF
{
	[TestFixture]
	public class GetGridPageMethodTest
	{
		private SessionProvider SessionProvider { get; set; }

		[SetUp]
		public void init()
		{
			Xxx.DeleteDatabaseDataGenereateSampleData();
			SharedLayer.Init();
			Assert.AreEqual("cs", SharedLayer.Culture);

			SessionProvider = SessionProviderFactoryTest.GetSessionProvider();
		}

		[Test]
		public void Basic()
		{
			using (var x = new RepositoryFactory().Create)
			{
				
			}
			using (var db = SessionProvider.CreateSession())
			{
				var a = db.Session.Page.Get(DataEfHelpers._defaultlink);
				Assert.IsNotNull(a);
				//Assert.AreEqual(DataEfHelpers._defaultlink, a.ResourceDto.Value);
			}
		}

		[Test]
		public void No_link()
		{
			using (var db = SessionProvider.CreateSession())
			{
				Assert.Throws<ObjectNotFoundException>(() => db.Session.Page.Get("linkTestPageXXX"));
			}
		}
	}
}