using System;
using System.Data.Entity;
using NUnit.Framework;
using cms.data.EF;
using cms.data.EF.DataProvider;
using cms.data.EF.Initializers;
using cms.data.tests.EF;
using cms.shared;

namespace cms.data.tests.PageAbstractTests
{
	[TestFixture]
	public class PageAbstract_Test_EntityFrameworkDB
	{
	
		[SetUp]
		public void SetUp()
		{
			Database.SetInitializer(new DropAndCreateAlwaysForce());
			Xxx.DeleteDatabaseDataGenereateSampleData();
		}

		public void RunTestDelegate(Action<PageAbstract_Test_MockDB> test)
		{
			using (var db = new EfContext())
			{
				var testsInstance = new PageAbstract_Test_MockDB(() => new EfRepository(db));
				testsInstance.Setup();
				test(testsInstance);
			}
		}

		[Test]
		public void Get_ByLink_test()
		{
			RunTestDelegate(instance => instance.Get_ByLink_test());
		}

		[Test]
		public void Get_ById_test()
		{
			RunTestDelegate(instance => instance.Get_ById_test());
		}

		[Test]
		public void List_test()
		{
			RunTestDelegate(instance => instance.List_test());
		}

		[Test]
		public void List_NoItems_test()
		{
			RunTestDelegate(instance => instance.List_NoItems_test());
		}

		[Test]
		public void Add_test()
		{
			RunTestDelegate(instance => instance.Add_test());
		}

		[Test]
		public void Update_test()
		{
			RunTestDelegate(instance => instance.Update_test());
		}

		[Test]
		public void Delete_test()
		{
			RunTestDelegate(instance => instance.Delete_test());
		}


	}
}