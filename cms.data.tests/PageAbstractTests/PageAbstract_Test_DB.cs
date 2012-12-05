using System.Linq;
using NUnit.Framework;
using cms.data.Dtos;
using cms.data.EF.DataProvider;
using cms.data.tests.EF;
using cms.shared;

namespace cms.data.tests.PageAbstractTests
{
	[TestFixture]
	public class PageAbstract_Test_DB
	{
		[SetUp]
		public void Setup()
		{
			Xxx.DeleteDatabaseDataGenereateSampleData();
			//Database.SetInitializer(new DropAndCreate());
		}

		[Test]
		public void List_test()
		{
			using (var db = new SessionManager().CreateSession)
			{

				Assert.IsNotNull(db.CurrentApplication);
				var page = new PageAbstractImpl(db.CurrentApplication, null);

				var list = page.List();
				Assert.IsTrue(list.Any());
				Assert.IsFalse(list.Any(x => x.Category == CategoryEnum.Menu));
			}
		}

		[Test]
		public void Add_test()
		{
			var a = new GridPageDto { Name = "xxxxx" };


			//TODO opravit 
			using (var db = new SessionManager().CreateSession)
			{
				var page = new PageAbstractImpl(db.CurrentApplication, null);
				var countBefore = page.List().Count();
				var newitem = page.Add(a);
				Assert.IsNotNull(newitem.Id);
				Assert.AreEqual(page.List().Count(), countBefore + 1);
				page.Delete(newitem.Id);
			}
		
		
		}

	}
}