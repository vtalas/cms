using System;
using NUnit.Framework;
using cms.data.EF.DataProviderImplementation;
using cms.data.Extensions;
using cms.data.Repository;
using cms.data.tests._Common;
using cms.shared;

namespace cms.data.tests.PageAbstractTests
{
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

			var app1 = AApplication("prd App")
				.WithGrid(
					AGrid().WithCategory(CategoryEnum.Menu)
					                   .WithResource(SpecialResourceEnum.Link, "aaa")
				)
				.WithGrid(
					AGrid(_gridId)
						.WithCategory(CategoryEnum.Menu)
						.WithResource(SpecialResourceEnum.Link, "bbb")
						.WithResource("name", "xxx")
						.WithResource("name", "xxxEn", CultureEn)
				).AddTo(Repository);
			_implemtation = new MenuAbstractImpl(app1, Repository);
		}

		[Test]
		public void Get_ById_test()
		{
			var menu = _implemtation.Get(_gridId);
			Assert.AreEqual(CategoryEnum.Menu, menu.Category );
			Assert.AreEqual("xxx", menu.Name);
			Assert.AreEqual("bbb", menu.Link);
			SetCulture(CultureEn);

			menu = _implemtation.Get(_gridId);
			Assert.AreEqual("bbb", menu.Link);
			Assert.AreEqual("xxxEn", menu.Name);
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