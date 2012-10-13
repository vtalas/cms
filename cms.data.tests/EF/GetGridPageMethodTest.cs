using System.Data;
using NUnit.Framework;
using cms.shared;

namespace cms.data.tests.EF
{
	[TestFixture]
	public class GetGridPageMethodTest
	{
		[SetUp]
		public void init()
		{
			Xxx.DeleteDatabaseDataGenereateSampleData();
			SharedLayer.Init();
			Assert.AreEqual("cs", SharedLayer.Culture);
		}

		[Test]
		public void Basic()
		{
			using (var db = new SessionManager().CreateSession)
			{
				var a = db.Page.Get(DataEfHelpers._defaultlink);
				Assert.IsNotNull(a);
				//Assert.AreEqual(DataEfHelpers._defaultlink, a.ResourceDto.Value);
			}
		}

		[Test]
		public void No_link()
		{
			using (var db = new SessionManager().CreateSession)
			{
				Assert.Throws<ObjectNotFoundException>(() => db.Page.Get("linkTestPageXXX"));
			}
		}
	}
}