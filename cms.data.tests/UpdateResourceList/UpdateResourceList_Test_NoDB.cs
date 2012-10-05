using System;
using NUnit.Framework;
using cms.data.Extensions;
using cms.data.Repository;
using cms.data.tests._Common;
using cms.shared;
using System.Linq;

namespace cms.data.tests.UpdateResourceList
{
	[TestFixture]
	public class UpdateResourceList_Test_NoDB : InjectableBase_Test
	{
		private int _totalResourcesCountBefore;

		public UpdateResourceList_Test_NoDB() 
			: this(new Base_MockDb().GetRepositoryMock) 
		{
			SharedLayer.Init();
		}
		public UpdateResourceList_Test_NoDB(Func<IRepository> repositoryCreator) 
			: base(repositoryCreator){}

		[SetUp]
		public void Setup()
		{
			using (var repository = RepositoryCreator())
			{
				_totalResourcesCountBefore = repository.Resources.Count();
			}
		}

		[Test]
		public void Values_AddNew_test()
		{
			using (var repository = RepositoryCreator())
			{
				var g = AGrid().AddTo(repository);
				var gResources = ResourcesAssertsHelper.EmptyResourcesDto()
					.WithResource("link", "linkvaluedto", 0)
					.WithResource("name", "linkvaluedto", 0);

				Assert.AreEqual(0, g.Resources.Count);

				g.UpdateResourceList(gResources, CultureCs, repository);
				Console.WriteLine(g.Resources.Count);
				Assert.AreEqual(2, g.Resources.Count);
			}
		}

		[Test]
		public void Values_AddNew_NewKeys_test()
		{
			var g = AGrid()
				.WithResource("linkXXX", "dbvalueLink")
				.WithResource("nameXXX", "dbvalueName");

			var gResources = ResourcesAssertsHelper.EmptyResourcesDto()
											.WithResource("link", "linkvaluedto", 0)
											.WithResource("name", "linkvaluedto", 0);

			using (var repository = RepositoryCreator())
			{
				Assert.AreEqual(2, g.Resources.Count);
				g.UpdateResourceList(gResources, CultureCs, repository);
				Assert.AreEqual(4, g.Resources.Count);
			}
		}

		[Test]
		public void Values_Replace_ByNewResources_test()
		{
			var g = AGrid()
				.WithResource("link", "dbvalueLink")
				.WithResource("name", "dbvalueName");

			var gResources = ResourcesAssertsHelper.EmptyResourcesDto()
											.WithResource("link", "linkvaluedto", 0)
											.WithResource("name", "namevaluedto", 0);
			using (var repository = RepositoryCreator())
			{

				g.UpdateResourceList(gResources, CultureCs, repository);

				Assert.AreEqual(2, g.Resources.Count);
				g.CheckResource("link").ValueIs("linkvaluedto");
				g.CheckResource("name").ValueIs("namevaluedto");
			}
		}

		[Test]
		public void UpdateResourceTest_checkOwnerShip()
		{
			var g = AGrid()
				.WithResource("link", "dbvalueLink", CultureCs, 11)
				.WithResource("name", "dbvalueName", CultureCs, 12);

			Assert.AreEqual(g.Id, g.Resources.GetByKey("name", CultureCs).Owner);
		}

		[Test]
		public void Values_Replace_ByNewResources_SearchById_test()
		{
			using (var repository = RepositoryCreator())
			{

				var g = AGrid()
					.WithResource("link", "dbvalueLink", CultureCs, 1)
					.WithResource("name", "dbvalueName", CultureCs, 2).AddTo(repository);

				var gResources = ResourcesAssertsHelper.EmptyResourcesDto()
					.WithResource("link", "linkvaluedto", 0)
					.WithResource("name", "namevaluedto", 0);

				g.UpdateResourceList(gResources, CultureCs, repository);
				Assert.AreEqual(2, g.Resources.Count);
				g.CheckResource("link").ValueIs("linkvaluedto");
				g.CheckResource("name").ValueIs("namevaluedto");
			}
		}

		[Test]
		public void Reference_AddNew_test()
		{
			using (var repository = RepositoryCreator())
			{
				var g1 = AGrid()
					.WithResource("link", "dbvalueLink", CultureCs, 1).AddTo(repository);

				var g2 = AGrid().AddTo(repository);

				g2.UpdateResourceList(g1.Resources.ToDtos(), CultureCs, repository);

				g2.CheckResource("link")
					.ValueIs("dbvalueLink")
					.OwnerIs(g1.Id);

				Assert.AreEqual(1, g2.Resources.Count);

			}
		}

		[Test]
		public void Reference_Replace_ByAnotherReference_test()
		{
			using (var repository = RepositoryCreator())
			{
				var grid1 = AGrid().WithResource("link", "a111", CultureCs, 1).AddTo(repository);
				var grid2 = AGrid().WithResource("link", "b222", CultureCs, 2).AddTo(repository);

				grid1.UpdateResourceList(grid2.Resources.ToDtos(), CultureCs, repository);

				grid1.CheckResource("link")
					.ValueIs("b222")
					.OwnerIs(grid2.Id);

				Assert.AreEqual(1, grid2.Resources.Count);
			}
		}

		[Test]
		public void Reference_Replace_ByNewResource_Test()
		{
			using (var repository = RepositoryCreator())
			{
				var grid1 = AGrid().AddTo(repository);
				var grid2 = AGrid().WithResource("link", "b222", CultureCs, 2).AddTo(repository);
				var newResources = ResourcesAssertsHelper.EmptyResourcesDto()
					.WithResource("link", "newlinkvalue", 0);


				grid1.UpdateResourceList(grid2.Resources.ToDtos(), CultureCs, repository);
				grid1.CheckResource("link")
					.ValueIs("b222")
					.OwnerIs(grid2.Id);

				grid1.UpdateResourceList(newResources, CultureCs, repository);
				grid1.CheckResource("link")
					.ValueIs("newlinkvalue")
					.OwnerIs(grid1.Id);


				Assert.AreEqual(1, grid2.Resources.Count);
			}
		}

		[Test]
		public void UpdateResourceTest_LANGUAGE()
		{
			using (var repository = RepositoryCreator())
			{
				var grid1 = AGrid().WithResource("link", "a111", CultureCs, 1).AddTo(repository);
				var grid2 = AGrid().WithResource("link", "b222", CultureCs, 2).AddTo(repository);
				var grid3 = AGrid().WithResource("link", "c333", CultureEn, 3).AddTo(repository);

				grid1.UpdateResourceList(grid2.Resources.ToDtos(), CultureCs, repository);

				grid1.CheckResource("link", CultureCs)
					.ValueIs("b222")
					.OwnerIs(grid2.Id);

				grid1.UpdateResourceList(grid3.Resources.ToDtos(CultureEn), CultureEn, repository);

				Assert.AreEqual(2, grid1.Resources.Count);

				grid1.CheckResource("link");
				grid1.CheckResource("link", CultureEn)
					.ValueIs("c333")
					.OwnerIs(grid3.Id);
			}
		}

		[Test]
		public void UpdateResourceValue_existingResource_test()
		{
			using (var repository = RepositoryCreator())
			{
				var grid1 = AGrid().WithResource("link", "a111", CultureCs, 1).AddTo(repository);
				var dtos = grid1.Resources.ToDtos();
				dtos["link"].Value = "xxx";
				grid1.UpdateResourceList(dtos, CultureCs, repository);

				grid1.CheckResource("link", CultureCs)
					.ValueIs("xxx")
					.OwnerIs(grid1.Id);
			}
		}

		[Test]
		public void Values_HandlingSpecianResources_linkIsCultureInvariant()
		{
			using (var Repository = RepositoryCreator())
			{
				var g = AGrid()
							.WithResource(SpecialResourceEnum.Link, "dbvalueLink").AddTo(Repository);

				var gResources = ResourcesAssertsHelper.EmptyResourcesDto()
														  .WithResource(SpecialResourceEnum.Link, "linkvaluedtoXXX", 0);

				g.UpdateResourceList(gResources, CultureCs, Repository);

				Assert.AreEqual(null, g.Resources.GetByKey(SpecialResourceEnum.Link, null).Culture);
				Assert.AreEqual(1, g.Resources.Count);
				Assert.AreEqual("linkvaluedtoXXX", g.Resources.GetValueByKey(SpecialResourceEnum.Link, CultureCs));
				Assert.AreEqual("linkvaluedtoXXX", g.Resources.GetValueByKey(SpecialResourceEnum.Link, CultureEn));
				Assert.AreEqual("linkvaluedtoXXX", g.Resources.GetValueByKey(SpecialResourceEnum.Link, ""));
				Assert.AreEqual("linkvaluedtoXXX", g.Resources.GetValueByKey(SpecialResourceEnum.Link, null));
			}
		}
	}
}