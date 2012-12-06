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
			SharedLayer.Init();
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
		public void Get_ById_test()
		{
			RunTestDelegate(instance => instance.Get_ById_test());
		}

		[Test]
		public void Add_test()
		{
			RunTestDelegate(instance => instance.Add_test());
		}
	}
}