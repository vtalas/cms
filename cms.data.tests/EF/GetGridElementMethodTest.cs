using System.Linq;
using NUnit.Framework;
using cms.shared;

namespace cms.data.tests.EF
{
	[TestFixture]
	public class GetGridElementMethodTest
	{
		[SetUp]
		public void Init()
		{
			Xxx.DeleteDatabaseDataGenereateSampleData();
			SharedLayer.Init();

		}

		[Test]
		public void CultureBasic()
		{
			Assert.AreEqual("cs", SharedLayer.Culture);

			var xxx = DataEfHelpers.AddDefaultGridElement();
			using (var db = new SessionManager().CreateSession)
			{
				var a = db.GridElement.Get(xxx.Id);

				Assert.IsNotNull(a);
				Assert.AreEqual(1, a.ResourcesLoc.Count);
				Assert.AreEqual("cesky", a.ResourcesLoc.First().Value.Value);
			}
		}

		[Test]
		public void CultureSwitchCulture()
		{
			Assert.AreEqual("cs", shared.SharedLayer.Culture);
			SharedLayer.Culture = "en";
			Assert.AreEqual("en", shared.SharedLayer.Culture);

			var xxx = DataEfHelpers.AddDefaultGridElement();
			using (var db = new SessionManager().CreateSession)
			{
				var a = db.GridElement.Get(xxx.Id);

				Assert.IsNotNull(a);
				Assert.AreEqual(1, a.ResourcesLoc.Count);
				Assert.AreEqual("englicky", a.ResourcesLoc.First().Value.Value);
			}
		}
	}
}