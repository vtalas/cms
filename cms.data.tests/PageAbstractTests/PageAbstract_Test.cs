using System;
using System.Data;
using NUnit.Framework;
using cms.data.Dtos;
using cms.data.EF.DataProviderImplementation;
using cms.data.Extensions;
using cms.data.Repository;
using cms.data.Shared.Models;
using cms.data.tests._Common;
using cms.shared;
using System.Linq;

namespace cms.data.tests.PageAbstractTests
{
	[TestFixture]
	public class PageAbstract_Test : InjectableBase_Test
	{
		private readonly Guid _gridIdFromApp2 = new Guid("1111111e-1115-480b-9ab7-a3ab3c0f6643");
		private int _gridsCountBefore;
		private int _resourcesCountBefore;
		private ApplicationSetting _application;

		public PageAbstract_Test() : this(new Base_MockDb().GetRepositoryMock) { }
		public PageAbstract_Test(Func<IRepository> repositoryCreator)
			: base(repositoryCreator)
		{
			SharedLayer.Init();
		}

		public IRepository RepositorySeed()
		{
			var repository = RepositoryCreator();
			_application = AApplication("prd App")
				.WithGrid(
					AGrid()
						.WithResource(SpecialResourceEnum.Link, "aaa")
						.WithResource("name", "NAME AAAA", CultureCs, 111)
				)
				.WithGrid(
					AGrid(GridId)
						.WithResource(SpecialResourceEnum.Link, "bbb")
						.WithResource("name", "NAME BBB", CultureCs, 221)
						.WithResource("name", "NAME BBB EN", CultureEn, 222)
				).AddTo(repository);

			AApplication("almost the same as app1")
				.WithGrid(
					AGrid(_gridIdFromApp2)
						.WithResource(SpecialResourceEnum.Link, "bbb")
						.WithResource("name", "NAME AAAA", CultureCs, 111)
				).AddTo(repository);

			Assert.That(repository.ApplicationSettings.Any());
			Assert.That(repository.Grids.Any());
			Assert.That(repository.Resources.Any());
			_gridsCountBefore = repository.Grids.Count();
			_resourcesCountBefore = repository.Resources.Count();
			return repository;
		}

		[Test]
		public void Get_ByLink_test()
		{
			var repo = RepositorySeed();
			using (var page = new PageAbstractImpl(_application, repo))
			{
				var gridpage = page.Get("bbb");
				Assert.IsNotNull(gridpage);
				Assert.AreEqual(gridpage.Id, GridId);

				SetCulture(CultureEn);

				gridpage = page.Get("bbb");
				Assert.IsNotNull(gridpage);
				Assert.AreEqual(gridpage.Id, GridId);
				Assert.AreEqual(gridpage.Name, "NAME BBB EN");
			}
		}

		[Test]
		public void Get_ById_test()
		{
			var repo = RepositorySeed();
			using (var page = new PageAbstractImpl(_application, repo))
			{
				var gridpage = page.Get(GridId);
				Assert.IsNotNull(gridpage);
				Assert.AreEqual(gridpage.Id, GridId);
			}
		}

		[Test]
		public void Get_ById_LanguageSet_test()
		{
			var repo = RepositorySeed();
			using (var page = new PageAbstractImpl(_application, repo))
			{
				var gridpage = page.Get(GridId);
				Assert.IsNotNull(gridpage);
				Assert.AreEqual(gridpage.Id, GridId);
				Assert.AreEqual(gridpage.Name, "NAME BBB");

				SharedLayer.SetCulture(CultureEn);
				gridpage = page.Get(GridId);

				Assert.IsNotNull(gridpage);
				Assert.AreEqual(gridpage.Id, GridId);
				Assert.AreEqual(gridpage.Name, "NAME BBB EN");
			}
		}

		[Test]
		public void List_test()
		{
			var repo = RepositorySeed();
			using (var page = new PageAbstractImpl(_application, repo))
			{
				var gridpage = page.List();
				Assert.IsTrue(gridpage.Any());
			}
		}

		[Test]
		public void List_NoItems_test()
		{
			using (var repository = RepositoryCreator())
			{
				var xx = new PageAbstractImpl(AApplication("applikace with no grids"), repository);
				Assert.IsFalse(xx.List().Any());
			}
		}

		[Test]
		public void Add_test()
		{
			var repo = RepositorySeed();
			using (var page = new PageAbstractImpl(_application, repo))
			{
				var gridpage = AGridpageDto("gridpagename", "xxxlink");
				var saved = page.Add(gridpage);

				Assert.AreEqual("xxxlink", saved.Link);
				Assert.AreEqual("gridpagename", saved.Name);
				Assert.AreEqual(_gridsCountBefore + 1, repo.Grids.Count());

			}
		}

		[Test]
		public void Add_TryAddExistingLink_test()
		{
			var repo = RepositorySeed();
			using (var page = new PageAbstractImpl(_application, repo))
			{
				Assert.AreEqual(_gridsCountBefore, repo.Grids.Count());
				Assert.AreEqual(2, repo.Resources.Count(x => x.Key == SpecialResourceEnum.Link && x.Value == "bbb"));
				var gridpage = AGridpageDto("gridpagename", "bbb");
				Assert.Throws<Exception>(() => page.Add(gridpage));
				Assert.AreEqual(_gridsCountBefore, repo.Grids.Count());
			}
		}

		[Test]
		public void Add_Change_lancguage_test()
		{
			var repo = RepositorySeed();
			using (var page = new PageAbstractImpl(_application, repo))
			{
				var gridpage = AGridpageDto("gridpagename", "xxxlink");
				var saved = page.Add(gridpage);
				Assert.AreEqual("xxxlink", saved.Link);
				Assert.AreEqual("gridpagename", saved.Name);
			}
		}

		[Test]
		public void Update_test()
		{
			var repo = RepositorySeed();
			using (var page = new PageAbstractImpl(_application, repo))
			{

				var a = new GridPageDto
							{
								Category = CategoryEnum.Page,
								Home = true,
								Id = GridId,
								Link = "new link",
								Name = "new name"
							};
				var gridpage = page.Update(a);

				Assert.IsNotNull(gridpage);
				Assert.AreEqual(GridId, gridpage.Id);
				Assert.AreEqual("new name", gridpage.Name);
				Assert.AreEqual("new link", gridpage.Link);
				Assert.IsTrue(gridpage.Home);
			}
		}

		[Test]
		public void Delete_test()
		{
			var repo = RepositorySeed();
			using (var page = new PageAbstractImpl(_application, repo))
			{
				page.Delete(GridId);
				Assert.AreEqual(_gridsCountBefore - 1, repo.Grids.Count());
				Assert.AreEqual(_resourcesCountBefore, repo.Resources.Count());
			}
		}

		[Test]
		public void Delete_IsNotOwner_TryToDelete_test()
		{
			var repo = RepositorySeed();
			using (var page = new PageAbstractImpl(_application, repo))
			{
				Assert.Throws<ObjectNotFoundException>(() => page.Delete(_gridIdFromApp2));
			}
		}


	}
}