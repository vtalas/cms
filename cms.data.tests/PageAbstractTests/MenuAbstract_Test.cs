using System;
using NUnit.Framework;
using cms.data.EF.DataProviderImplementation;
using cms.data.Extensions;
using cms.data.Repository;
using cms.data.Shared.Models;
using cms.data.tests._Common;
using cms.shared;

namespace cms.data.tests.PageAbstractTests
{
	[TestFixture]
	public class MenuAbstract_Test : InjectableBase_Test
	{
		private ApplicationSetting _application;

		public MenuAbstract_Test()
			: this(new Base_MockDb().GetRepositoryMock)
		{
		}

		public MenuAbstract_Test(Func<IRepository> repositoryCreator)
			: base(repositoryCreator)
		{
			SharedLayer.Init();
		}

		public IRepository RepositorySeed()
		{
			SetCulture(CultureCs);
			var repository = RepositoryCreator();
			_application = AApplication("prd App")
				.WithGrid(
					AGrid().WithCategory(CategoryEnum.Menu)
						.WithResource(SpecialResourceEnum.Link, "aaa")
				)
				.WithGrid(
					AGrid(GridId)
						.WithCategory(CategoryEnum.Menu)
						.WithResource(SpecialResourceEnum.Link, "bbb")
						.WithResource("name", "xxx")
						.WithResource("name", "xxxEn", CultureEn)
				).AddTo(repository);
			return repository;
		}

		[Test]
		public void Get_ById_test()
		{
			var repository = RepositorySeed();
			using (var db = new MenuAbstractImpl(_application, repository))
			{
				var menu = db.Get(GridId);
				Assert.AreEqual(CategoryEnum.Menu, menu.Category);
				Assert.AreEqual("xxx", menu.Name);
				Assert.AreEqual("bbb", menu.Link);
				SetCulture(CultureEn);

				menu = db.Get(GridId);
				Assert.AreEqual("bbb", menu.Link);
				Assert.AreEqual("xxxEn", menu.Name);
			}
		}

		[Test]
		public void AddMenuItem_XXX_test()
		{
			var repository = RepositorySeed();
			using (var db = new MenuAbstractImpl(_application, repository))
			{
				var menu = db.Get(GridId);
				Assert.AreEqual(CategoryEnum.Menu, menu.Category);
				Assert.AreEqual("xxx", menu.Name);
				Assert.AreEqual("bbb", menu.Link);
				SetCulture(CultureEn);

				menu = db.Get(GridId);
				Assert.AreEqual("bbb", menu.Link);
				Assert.AreEqual("xxxEn", menu.Name);
			}
		}

		//public MenuDto Get(Guid guid)
		//public MenuDto Add(MenuDto newitem);
		//public MenuItemDto UpdateMenuItem(MenuItemDto menuItem);
		//public MenuItemDto AddMenuitem(MenuItemDto menuItem);
		//public Guid AddMenuItem(MenuItemDto newitem, Guid gridId);
		//public IEnumerable<MenuDto> List();
		//public void Delete(Guid guid);

	}
}