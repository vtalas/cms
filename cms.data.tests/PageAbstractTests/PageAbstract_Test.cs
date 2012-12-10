using System;
using System.Collections.Generic;
using System.Data;
using NUnit.Framework;
using cms.data.DataProvider;
using cms.data.Dtos;
using cms.data.EF.DataProviderImplementation;
using cms.data.EF.RepositoryImplementation;
using cms.data.Extensions;
using cms.data.Repository;
using cms.data.tests._Common;
using cms.shared;
using System.Linq;
using cms.data.EF.Initializers;

namespace cms.data.tests.PageAbstractTests
{
	[TestFixture]
	public class GridElementAbstract_Test : InjectableBase_Test
	{
		private GridElementAbstract GridElementImpl { get; set; }
		private readonly Guid _gridIdFromApp2 = new Guid("1111111e-1115-480b-9ab7-a3ab3c0f6643");
		private int _gridsCountBefore;
		private int _resourcesCountBefore;

		public GridElementAbstract_Test()
			: this(new Base_MockDb().GetRepositoryMock)
		{
		}

		public GridElementAbstract_Test(Func<IRepository> repositoryCreator)
			: base(repositoryCreator)
		{
			SharedLayer.Init();
		}

		[SetUp]
		public void Setup()
		{
			Repository = RepositoryCreator();

			var app1 = CreateDefaultApplication("prd App")
				.WithGrid(
					CreateDefaultGrid()
						.WithResource(SpecialResourceEnum.Link, "aaa")
						.WithResource("name", "NAME AAAA", CultureCs, 111)
				)
				.WithGrid(
					CreateDefaultGrid(_gridId)
						.WithResource(SpecialResourceEnum.Link, "bbb")
						.WithResource("name", "NAME BBB", CultureCs, 221)
						.WithResource("name", "NAME BBB EN", CultureEn, 222)
				).AddTo(Repository);
			GridElementImpl = new GridElementAbstractImpl(app1, Repository);
		}
	}

	[TestFixture]
	public class MenuAbstract_Test : InjectableBase_Test
	{
		private MenuAbstractImpl _implemtation;

		public MenuAbstract_Test()
			: this(new Base_MockDb().GetRepositoryMock)
		{
		}

		public MenuAbstract_Test(Func<IRepository> repositoryCreator) : base(repositoryCreator)
		{
			SharedLayer.Init();
		}

		[SetUp]
		public void Setup()
		{
			Repository = RepositoryCreator();

			var app1 = CreateDefaultApplication("prd App")
				.WithGrid(
					CreateDefaultGrid().WithCategory(CategoryEnum.Menu)
						.WithResource(SpecialResourceEnum.Link, "aaa")
						.WithResource("name", "NAME AAAA", CultureCs, 111)
				)
				.WithGrid(
					CreateDefaultGrid(_gridId)
						.WithResource(SpecialResourceEnum.Link, "bbb")
						.WithResource("name", "NAME BBB", CultureCs, 221)
						.WithResource("name", "NAME BBB EN", CultureEn, 222)
				).AddTo(Repository);
			_implemtation = new MenuAbstractImpl(app1, Repository);
		}


		//public MenuDto Get(Guid guid)
		//public MenuDto Add(MenuDto newitem);
		//public MenuItemDto UpdateMenuItem(MenuItemDto menuItem);
		//public MenuItemDto AddMenuitem(MenuItemDto menuItem);
		//public Guid AddMenuItem(MenuItemDto newitem, Guid gridId);
		//public IEnumerable<MenuDto> List();
		//public void Delete(Guid guid);

	}

	[TestFixture]
	public class PageAbstract_Test : InjectableBase_Test
	{
		private PageAbstractImpl Page { get; set; }
		private readonly Guid _gridIdFromApp2 = new Guid("1111111e-1115-480b-9ab7-a3ab3c0f6643");
		private int _gridsCountBefore;
		private int _resourcesCountBefore;

		public PageAbstract_Test() : this(new Base_MockDb().GetRepositoryMock) { }
		public PageAbstract_Test(Func<IRepository> repositoryCreator)
			: base(repositoryCreator)
		{
			SharedLayer.Init();
		}

		[SetUp]
		public void Setup()
		{
			Repository = RepositoryCreator();

			var app1 = CreateDefaultApplication("prd App")
				.WithGrid(
					CreateDefaultGrid()
						.WithResource(SpecialResourceEnum.Link, "aaa")
						.WithResource("name", "NAME AAAA", CultureCs, 111)
				)
				.WithGrid(
					CreateDefaultGrid(_gridId)
						.WithResource(SpecialResourceEnum.Link, "bbb")
						.WithResource("name", "NAME BBB", CultureCs, 221)
						.WithResource("name", "NAME BBB EN", CultureEn, 222)
				).AddTo(Repository);

			CreateDefaultApplication("almost the same as app1")
				.WithGrid(
					CreateDefaultGrid(_gridIdFromApp2)
						.WithResource(SpecialResourceEnum.Link, "bbb")
						.WithResource("name", "NAME AAAA", CultureCs, 111)
				).AddTo(Repository);

			Page = new PageAbstractImpl(app1, Repository);
			Assert.That(Repository.ApplicationSettings.Any());
			Assert.That(Repository.Grids.Any());
			Assert.That(Repository.Resources.Any());
			_gridsCountBefore = Repository.Grids.Count();
			_resourcesCountBefore = Repository.Resources.Count();
		}

		[Test]
		public void Get_ByLink_test()
		{
			var gridpage = Page.Get("bbb");
			Assert.IsNotNull(gridpage);
			Assert.AreEqual(gridpage.Id, _gridId);

			SetCulture(CultureEn);

			gridpage = Page.Get("bbb");
			Assert.IsNotNull(gridpage);
			Assert.AreEqual(gridpage.Id, _gridId);
			Assert.AreEqual(gridpage.Name, "NAME BBB EN");
		}

		[Test]
		public void Get_ById_test()
		{
			var gridpage = Page.Get(_gridId);
			Assert.IsNotNull(gridpage);
			Assert.AreEqual(gridpage.Id, _gridId);
		}

		[Test]
		public void Get_ById_LanguageSet_test()
		{
			var gridpage = Page.Get(_gridId);
			Assert.IsNotNull(gridpage);
			Assert.AreEqual(gridpage.Id, _gridId);
			Assert.AreEqual(gridpage.Name, "NAME BBB");

			SharedLayer.SetCulture(CultureEn);
			gridpage = Page.Get(_gridId);

			Assert.IsNotNull(gridpage);
			Assert.AreEqual(gridpage.Id, _gridId);
			Assert.AreEqual(gridpage.Name, "NAME BBB EN");
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
			var gridpage = CreateDefaultGridpage("gridpagename", "xxxlink");
			var saved = Page.Add(gridpage);

			Assert.AreEqual("xxxlink", saved.Link);
			Assert.AreEqual("gridpagename", saved.Name);
			Assert.AreEqual(_gridsCountBefore + 1, Repository.Grids.Count());

		}

		[Test]
		public void Add_TryAddExistingLink_test()
		{
			Assert.AreEqual(_gridsCountBefore, Repository.Grids.Count());
			Assert.AreEqual(2, Repository.Resources.Count(x => x.Key == SpecialResourceEnum.Link && x.Value == "bbb"));
			var gridpage = CreateDefaultGridpage("gridpagename", "bbb");
			Assert.Throws<Exception>(() => Page.Add(gridpage));
			Assert.AreEqual(_gridsCountBefore, Repository.Grids.Count());
		}

		[Test]
		public void Add_Change_lancguage_test()
		{
			var gridpage = CreateDefaultGridpage("gridpagename", "xxxlink");
			var saved = Page.Add(gridpage);

			Assert.AreEqual("xxxlink", saved.Link);
			Assert.AreEqual("gridpagename", saved.Name);
		}

		[Test]
		public void Update_test()
		{
			var a = new GridPageDto
				{
					Category = CategoryEnum.Page,
					Home = true,
					Id = _gridId,
					Link = "new link",
					Name = "new name"
				};
			var gridpage = Page.Update(a);

			Assert.IsNotNull(gridpage);
			Assert.AreEqual(_gridId, gridpage.Id);
			Assert.AreEqual("new name", gridpage.Name);
			Assert.AreEqual("new link", gridpage.Link);
			Assert.IsTrue(gridpage.Home);
		}

		[Test]
		public void Delete_test()
		{
			Page.Delete(_gridId);
			Assert.AreEqual(_gridsCountBefore - 1, Repository.Grids.Count());
			Assert.AreEqual(_resourcesCountBefore, Repository.Resources.Count());
		}

		[Test]
		public void Delete_IsNotOwner_TryToDelete_test()
		{
			Assert.Throws<ObjectNotFoundException>(() => Page.Delete(_gridIdFromApp2));

		}


	}
}