using System;
using System.Data.Entity;
using NUnit.Framework;
using cms.data.EF;
using cms.data.EF.DataProvider;
using cms.data.EF.Initializers;
using cms.data.tests.EF;
using cms.data.tests._Common;
using cms.shared;
using System.Linq;

namespace cms.data.tests.PageAbstractTests
{
	[TestFixture]
	public class PageAbstract_Test_DBXXX
	{
		[SetUp]
		public void SetUp()
		{
			Database.SetInitializer(new DropAndCreateAlwaysForce());
			Xxx.DeleteDatabaseDataGenereateSampleData();
			SharedLayer.Init();
		}
	
		[Test]
		public void xxxx()
		{
			using (var x = new EfContext())
			{
				var a = new PageAbstract_Test_DBMock(() => new EfRepository(x));
				a.Setup();
				a.Get_ById_test();
			}
		}
	}

	[TestFixture]
	public class PageAbstract_Test_DBMock : NoDbBase_Test
	{
		private PageAbstractImpl Page { get; set; }
		private readonly Guid _gridId = new Guid("0000005e-1115-480b-9ab7-a3ab3c0f6643");
		private Func<IRepository> RepositoryCreator { get; set; }
		
		public PageAbstract_Test_DBMock(Func<IRepository> repositoryCreator)
		{
			RepositoryCreator = repositoryCreator;
		}

		public PageAbstract_Test_DBMock()
		{
			RepositoryCreator = GetRepositoryMock;
		}

		[SetUp]
		public void Setup()
		{
			_repository = RepositoryCreator();
			SharedLayer.Init();

			var app = CreateDefaultApplication("prd App")
				.WithGrid(
					GridHelper.WithResource(CreateDefaultGrid()
							              .WithResource("link", "aaa"), "name", "NAME AAAA", CultureCs, 111)
				)
				.WithGrid(
					GridHelper.WithResource(CreateDefaultGrid(_gridId)
							              .WithResource("link", "bbb"), "name", "NAME BBB", CultureCs, 222)
				).AddTo(_repository);

			Page = new PageAbstractImpl(app, _repository);
			Assert.That(_repository.ApplicationSettings.Any());
			Assert.That(_repository.Grids.Any());
			Assert.That(_repository.Resources.Any());
		}

		[Test]
		public void Get_ByLink_test()
		{
			var gridpage = Page.Get("bbb");
			Assert.IsNotNull(gridpage);
			Assert.AreEqual(gridpage.Id, _gridId);
		}

		[Test]
		public void Get_ById_test()
		{
			var gridpage = Page.Get(_gridId);
			Assert.IsNotNull(gridpage);
			Assert.AreEqual(gridpage.Id, _gridId);
		}

		[Test]
		public void List_test()
		{
			var gridpage = Page.List();
			Assert.IsTrue(gridpage.Any());
		}

		[Test]
		public void List_NoItems_test()
		{
			var xx = new PageAbstractImpl(CreateDefaultApplication("applikace with no grids"), _repository);
			Assert.IsFalse(xx.List().Any());
		}

		[Test]
		public void Add_test()
		{
			Assert.IsTrue(false);
		}

		[Test]
		public void Update_test()
		{
			Assert.IsTrue(false);
		}
	
		[Test]
		public void Delete_test()
		{
			Assert.IsTrue(false);
		}


	}
}