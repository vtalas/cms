using System;
using NUnit.Framework;
using cms.data.Dtos;
using cms.data.DtosExtensions;
using cms.data.EF.Repository;
using cms.data.EF.Initializers;
using cms.data.tests.PageAbstractTests;
using cms.data.tests._Common;
using cms.shared;

namespace cms.data.tests.UpdateResourceList
{
	[TestFixture]
	public class UpdateResourceList_Test_NoDB : InjectableBase_Test
	{
		
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
			Repository = RepositoryCreator();
		}

		[Test]
		public void Values_AddNew_test()
		{
			var g = CreateDefaultGrid().AddTo(Repository);

			var gResources = ResourcesHelper.EmptyResourcesDto()
													  .WithResource("link", "linkvaluedto", 0)
													  .WithResource("name", "linkvaluedto", 0);

			Assert.AreEqual(0, g.Resources.Count);

			g.UpdateResourceList(gResources, CultureCs, Repository);
			Console.WriteLine(g.Resources.Count);
			Assert.AreEqual(2, g.Resources.Count);
		}

		[Test]
		public void Values_AddNew_NewKeys_test()
		{
			var g = CreateDefaultGrid()
				.WithResource("linkXXX", "dbvalueLink")
				.WithResource("nameXXX", "dbvalueName");

			var gResources = ResourcesHelper.EmptyResourcesDto()
											.WithResource("link", "linkvaluedto", 0)
											.WithResource("name", "linkvaluedto", 0);

			Assert.AreEqual(2, g.Resources.Count);
			g.UpdateResourceList(gResources, CultureCs, Repository);
			Assert.AreEqual(4, g.Resources.Count);
		}

		[Test]
		public void Values_Replace_ByNewResources_test()
		{
			var g = CreateDefaultGrid()
				.WithResource("link", "dbvalueLink")
				.WithResource("name", "dbvalueName");

			var gResources = ResourcesHelper.EmptyResourcesDto()
											.WithResource("link", "linkvaluedto", 0)
											.WithResource("name", "namevaluedto", 0);

			g.UpdateResourceList(gResources, CultureCs, Repository);

			Assert.AreEqual(2, g.Resources.Count);
			g.CheckResource("link").ValueIs("linkvaluedto");
			g.CheckResource("name").ValueIs("namevaluedto");
		}

		[Test]
		public void UpdateResourceTest_checkOwnerShip()
		{
			var g = CreateDefaultGrid()
				.WithResource("link", "dbvalueLink", CultureCs, 11)
				.WithResource("name", "dbvalueName", CultureCs, 12);

			Assert.AreEqual(g.Id, g.Resources.GetByKey("name", CultureCs).Owner);
		}

		[Test]
		public void Values_Replace_ByNewResources_SearchById_test()
		{
			var g = CreateDefaultGrid()
				.WithResource("link", "dbvalueLink", CultureCs, 1)
				.WithResource("name", "dbvalueName", CultureCs, 2).AddTo(Repository);

			var gResources = ResourcesHelper.EmptyResourcesDto()
											.WithResource("link", "linkvaluedto", 12)
											.WithResource("name", "namevaluedto", 23);

			g.UpdateResourceList(gResources, CultureCs, Repository);
			Assert.AreEqual(2, g.Resources.Count);
			g.CheckResource("link").ValueIs("linkvaluedto");
			g.CheckResource("name").ValueIs("namevaluedto");
		}

		[Test]
		public void UpdateResourceTest_UpdateResources_isNotOwnwer_ShouldReplaceByReference()
		{
			var g1 = CreateDefaultGrid()
				.WithResource("link", "dbvalueLink", CultureCs, 1).AddTo(Repository);

			var g2 = CreateDefaultGrid()
				.WithResource("link", "xxx", CultureCs, 12).AddTo(Repository);

			g1.UpdateResourceList(g2.Resources.ToDtos(), CultureCs, Repository);

			g1.CheckResource("link")
			  .ValueIs("xxx")
			  .OwnerIs(g2.Id);

			Assert.AreEqual(1, g1.Resources.Count);
		}

		[Test]
		public void Reference_AddNew_test()
		{
			var g1 = CreateDefaultGrid()
				.WithResource("link", "dbvalueLink", CultureCs, 1).AddTo(Repository);

			var g2 = CreateDefaultGrid().AddTo(Repository);

			g2.UpdateResourceList(g1.Resources.ToDtos(), CultureCs, Repository);

			g2.CheckResource("link")
			  .ValueIs("dbvalueLink")
			  .OwnerIs(g1.Id);

			Assert.AreEqual(1, g2.Resources.Count);
		}

		[Test]
		public void Reference_Replace_ByAnotherReference_test()
		{
			var grid1 = CreateDefaultGrid().WithResource("link", "a111", CultureCs, 1).AddTo(Repository);
			var grid2 = CreateDefaultGrid().WithResource("link", "b222", CultureCs, 2).AddTo(Repository);

			grid1.UpdateResourceList(grid2.Resources.ToDtos(), CultureCs, Repository);

			grid1.CheckResource("link")
				 .ValueIs("b222")
				 .OwnerIs(grid2.Id);

			Assert.AreEqual(1, grid2.Resources.Count);
		}

		[Test]
		public void Reference_Replace_ByNewResource_Test()
		{
			var grid1 = CreateDefaultGrid().AddTo(Repository);
			var grid2 = CreateDefaultGrid().WithResource("link", "b222", CultureCs, 2).AddTo(Repository);
			var newResources = ResourcesHelper.EmptyResourcesDto()
											 .WithResource("link", "newlinkvalue", 0);


			grid1.UpdateResourceList(grid2.Resources.ToDtos(), CultureCs, Repository);
			grid1.CheckResource("link")
				  .ValueIs("b222")
				  .OwnerIs(grid2.Id);

			grid1.UpdateResourceList(newResources, CultureCs, Repository);
			grid1.CheckResource("link")
				.ValueIs("newlinkvalue")
				.OwnerIs(grid1.Id);


			Assert.AreEqual(1, grid2.Resources.Count);
		}

		[Test]
		public void UpdateResourceTest_LANGUAGE()
		{
			var grid1 = CreateDefaultGrid().WithResource("link", "a111", CultureCs, 1).AddTo(Repository);
			var grid2 = CreateDefaultGrid().WithResource("link", "b222", CultureCs, 2).AddTo(Repository);
			var grid3 = CreateDefaultGrid().WithResource("link", "c333", CultureEn, 3).AddTo(Repository);

			grid1.UpdateResourceList(grid2.Resources.ToDtos(), CultureCs, Repository);

			grid1.CheckResource("link", CultureCs)
				 .ValueIs("b222")
				 .OwnerIs(grid2.Id);

			grid1.UpdateResourceList(grid3.Resources.ToDtos(CultureEn), CultureEn, Repository);

			Assert.AreEqual(2, grid1.Resources.Count);

			grid1.CheckResource("link");
			grid1.CheckResource("link", CultureEn)
				 .ValueIs("c333")
				 .OwnerIs(grid3.Id);
		}

	}
}