using System;
using System.Data.Entity;
using NUnit.Framework;
using WebMatrix.WebData;
using cms.data.EF;
using cms.data.EF.Repository;
using cms.data.EF.Initializers;
using cms.data.tests.EF;
using cms.data.tests._Common;
using System.Linq;
using cms.shared;

namespace cms.data.tests.PageAbstractTests
{
	[TestFixture]
	public class PageAbstract_Test_EntityFrameworkDB :Base_Test
	{
	
		[SetUp]
		public void SetUp()
		{
			Database.SetInitializer(new DropAndCreateAlwaysForce());
			Xxx.DeleteDatabaseData();
			SecurityProvider.EnsureInitialized(true);
			SharedLayer.Init();
		}

		public void RunTestDelegate(Action<PageAbstract_Test> test)
		{
			using (var db = new EfContext())
			{
				var testsInstance = new PageAbstract_Test(() => new EfRepository(db));
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
		public void Get_ByLink_test_SESSIONMANAGER()
		{

			using (var db = new EfContext())
			{
				var useriId = CreateUser("admin");
				CreateDefaultApplication("prd", SessionManager.DefaultAppId)
					.WithUser(db.UserProfile.Single(x => x.Id == useriId))
					.WithGrid(CreateDefaultGrid()
					 .WithResource("link", "xxx"))
					 .AddTo(new EfRepository(db));
			}

			using (var session = new SessionManager().CreateSession)
			{
				var a = session.Page.Get("xxx");
			}
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