using System;
using System.Data.Entity;
using NUnit.Framework;
using WebMatrix.WebData;
using cms.data.EF;
using cms.data.EF.RepositoryImplementation;
using cms.data.EF.Initializers;
using cms.data.Extensions;
using cms.data.tests.EF;
using cms.data.tests._Common;
using System.Linq;
using cms.shared;

namespace cms.data.tests.PageAbstractTests
{
	[TestFixture]
	public class PageAbstract_Test_EntityFrameworkDB :Base_Test
	{
		private SessionProvider SessionProvider { get; set; }

		[SetUp]
		public void SetUp()
		{
			Database.SetInitializer(new DropAndCreateAlwaysForce());
			Xxx.DeleteDatabaseData();
			SecurityProvider.EnsureInitialized(true);
			SharedLayer.Init();

			SessionProvider = SessionProviderFactoryTest.GetSessionProvider();

		}

		public void RunTestDelegate(Action<PageAbstract_Test> test)
		{
			var testsInstance = new PageAbstract_Test(() => new EfRepository());
			test(testsInstance);
		}

		[Test]
		public void Get_ByLink_test()
		{
			RunTestDelegate(instance => instance.Get_ByLink_test());
		}

		[Test]
		public void Get_ById_LanguageSet_test()
		{
			RunTestDelegate(instance => instance.Get_ById_test());
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
		public void Add_TryAddExistingLink_test()
		{
			RunTestDelegate(instance => instance.Add_Change_lancguage_test());
		}

		[Test]
		public void Add_Change_language_test()
		{
			RunTestDelegate(instance => instance.Add_Change_lancguage_test());
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

		[Test]
		public void Delete_IsNotOwner_TryToDelete_test()
		{
			RunTestDelegate(instance => instance.Delete_IsNotOwner_TryToDelete_test());
		}

		[Test]
		public void Get_ByLink_test_SESSIONMANAGER()
		{

			using (var db = new EfContext())
			{
				var useriId = CreateUser("admin");
				AApplication("prd", SessionProviderFactoryTest.DefaultAppId)
					.WithUser(db.UserProfile.Single(x => x.Id == useriId))
					.WithGrid(AGrid()
					 .WithResource(SpecialResourceEnum.Link, "xxx"))
					 .AddTo(new EfRepository(db));
			}

			using (var db = SessionProvider.CreateSession())
			{
				var a = db.Session.Page.Get("xxx");
			}
		}



	}
}