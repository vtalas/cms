using System;
using NUnit.Framework;
using cms.data.EF.DataProvider;
using cms.data.tests._Common;
using cms.shared;
using System.Linq;

namespace cms.data.tests.PageAbstractTests
{
	[TestFixture]
	public class PageAbstract_Test : InjectableBase_Test
	{
		private PageAbstractImpl Page { get; set; }
		private readonly Guid _gridId = new Guid("0000005e-1115-480b-9ab7-a3ab3c0f6643");

		public PageAbstract_Test() : this(new Base_MockDb().GetRepositoryMock){}
		public PageAbstract_Test(Func<IRepository> repositoryCreator) : base(repositoryCreator)
		{
			SharedLayer.Init();
		}

		[SetUp]
		public void Setup()
		{
			Repository = RepositoryCreator();

			var app = CreateDefaultApplication("prd App")
				.WithGrid(
					CreateDefaultGrid()
						.WithResource("link", "aaa")
						.WithResource("name", "NAME AAAA", CultureCs, 111)
				)	
				.WithGrid(
					CreateDefaultGrid(_gridId)
						.WithResource("link", "bbb")
						.WithResource("name", "NAME BBB", CultureCs, 222)
				).AddTo(Repository);

			Page = new PageAbstractImpl(app, Repository);
			Assert.That(Repository.ApplicationSettings.Any());
			Assert.That(Repository.Grids.Any());
			Assert.That(Repository.Resources.Any());
			Console.WriteLine("resources: {0}", Repository.Resources.Count());
			Console.WriteLine("grids: {0}", Repository.Grids.Count());

		}

		[Test]
		public void Get_ByLink_test()
		{
			var gridpage = Page.Get("bbb");
			Assert.IsNotNull(gridpage);
			Assert.AreEqual(gridpage.Id, _gridId);
			Assert.AreEqual(4, Repository.Resources.Count());
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
			var xx = new PageAbstractImpl(CreateDefaultApplication("applikace with no grids"), Repository);
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