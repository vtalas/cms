using System.Linq;
using NUnit.Framework;
using cms.data.Dtos;
using cms.data.EF.DataProvider;
using cms.shared;

namespace cms.data.tests.EF
{
	[TestFixture]
	public class PageAbstractTest
	{
		[SetUp]
		public void Setup()
		{
			Xxx.DeleteDatabaseData();
			//Database.SetInitializer(new DropAndCreate());
		}

		[Test]
		public void List_test()
		{
			using (var db = new SessionManager().CreateSession)
			{
				Assert.IsNotNull(db.CurrentApplication);
				var page = new PageAbstractImpl(db.CurrentApplication, db.db);

				var list = page.List();
				Assert.IsTrue(list.Any());
				Assert.IsFalse(list.Any(x => x.Category == CategoryEnum.Menu));
			}
		}

		[Test]
		public void Add_test()
		{
			var a = new GridPageDto { Name = "xxxxx" };

			using (var db = new SessionManager().CreateSession)
			{
				var page = new PageAbstractImpl(db.CurrentApplication, db.db);
				var countBefore = page.List().Count();
				var newitem = page.Add(a);
				Assert.IsNotNull(newitem.Id);
				Assert.AreEqual(page.List().Count(), countBefore + 1);
				page.Delete(newitem.Id);
			}
		
		
		}

	}
}