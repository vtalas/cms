using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using cms.data.Dtos;
using cms.data.EF.DataProvider;
using cms.data.Shared.Models;
using cms.data.tests.Helpers;
using cms.shared;

namespace cms.data.tests.UpdateResourceList
{
	[TestFixture]
	public class UpdateResourceList_Test_NoDB
	{
		const string CultureCs = "cs";
		const string CultureEn = "en";

		private static IList<Resource> _allResources;
		private IRepository _repository;

		private Grid DefaultGrid()
		{
			return new Grid()
				{
					Id = Guid.NewGuid(),
					Category = "page",
				};
		}

		private IRepository GetRepositoryMock()
		{
			var repo = new Mock<IRepository>();
			repo.Setup(x => x.Resources).Returns(_allResources.AsQueryable);
			repo.Setup(x => x.Add(It.IsAny<Resource>())).Returns((Resource input) =>
																	 {
																		 input.Id = _allResources.Count + 1;
																		 return input;
																	 });
			return repo.Object;
		}

		[SetUp]
		public void SetUp()
		{
			_allResources = new List<Resource>();
			_repository = GetRepositoryMock();

			SharedLayer.Init();
		}

		[Test]
		public void Values_AddNew_test()
		{
			var g = DefaultGrid();

			var gResources = ResourcesHelper.EmptyResourcesDto()
											.WithResource("link", "linkvaluedto", 0)
											.WithResource("name", "linkvaluedto", 0);

			Assert.AreEqual(0, g.Resources.Count);

			g.UpdateResourceList(gResources, CultureCs, _repository);
			Console.WriteLine(g.Resources.Count);
			Assert.AreEqual(2, g.Resources.Count);
		}

		[Test]
		public void Values_AddNew_NewKeys_test()
		{
			var g = DefaultGrid()
				.WithResource("linkXXX", "dbvalueLink")
				.WithResource("nameXXX", "dbvalueName");

			var gResources = ResourcesHelper.EmptyResourcesDto()
											.WithResource("link", "linkvaluedto", 0)
											.WithResource("name", "linkvaluedto", 0);

			Assert.AreEqual(2, g.Resources.Count);
			g.UpdateResourceList(gResources, CultureCs, _repository);
			Assert.AreEqual(4, g.Resources.Count);
		}

		[Test]
		public void Values_Replace_ByNewResources_test()
		{
			var g = DefaultGrid()
				.WithResource("link", "dbvalueLink")
				.WithResource("name", "dbvalueName");

			var gResources = ResourcesHelper.EmptyResourcesDto()
											.WithResource("link", "linkvaluedto", 0)
											.WithResource("name", "namevaluedto", 0);

			g.UpdateResourceList(gResources, CultureCs, _repository);

			Assert.AreEqual(2, g.Resources.Count);
			g.CheckResource("link").ValueIs("linkvaluedto");
			g.CheckResource("name").ValueIs("namevaluedto");
		}

		[Test]
		public void UpdateResourceTest_checkOwnerShip()
		{
			var g = DefaultGrid()
				.WithResource("link", "dbvalueLink", CultureCs, 11)
				.WithResource("name", "dbvalueName", CultureCs, 12);

			Assert.AreEqual(g.Id, g.Resources.GetByKey("name", CultureCs).Owner);
		}

		[Test]
		public void Values_Replace_ByNewResources_SearchById_test()
		{
			var g = DefaultGrid()
				.WithResource(_allResources, "link", "dbvalueLink", CultureCs, 1)
				.WithResource(_allResources, "name", "dbvalueName", CultureCs, 2);

			var gResources = ResourcesHelper.EmptyResourcesDto()
											.WithResource("link", "linkvaluedto", 12)
											.WithResource("name", "namevaluedto", 23);

			g.UpdateResourceList(gResources, CultureCs, _repository);
			Assert.AreEqual(2, g.Resources.Count);
			g.CheckResource("link").ValueIs("linkvaluedto");
			g.CheckResource("name").ValueIs("namevaluedto");
		}

		[Test]
		public void UpdateResourceTest_UpdateResources_isNotOwnwer_ShouldReplaceByReference()
		{
			var g1 = DefaultGrid()
				.WithResource(_allResources, "link", "dbvalueLink", CultureCs, 1);

			var g2 = DefaultGrid()
				.WithResource(_allResources, "link", "xxx", CultureCs, 12);

			g1.UpdateResourceList(g2.Resources.ToDtos(), CultureCs, _repository);

			g1.CheckResource("link")
			  .ValueIs("xxx")
			  .OwnerIs(g2.Id);

			Assert.AreEqual(1, g1.Resources.Count);
		}

		[Test]
		public void Reference_AddNew_test()
		{
			var g1 = DefaultGrid()
				.WithResource(_allResources, "link", "dbvalueLink", CultureCs, 1);

			var g2 = DefaultGrid();

			g2.UpdateResourceList(g1.Resources.ToDtos(), CultureCs, _repository);

			g2.CheckResource("link")
			  .ValueIs("dbvalueLink")
			  .OwnerIs(g1.Id);

			Assert.AreEqual(1, g2.Resources.Count);
		}

		[Test]
		public void Reference_Replace_ByAnotherReference_test()
		{
			var grid1 = DefaultGrid().WithResource(_allResources, "link", "a111", CultureCs, 1);
			var grid2 = DefaultGrid().WithResource(_allResources, "link", "b222", CultureCs, 2);

			grid1.UpdateResourceList(grid2.Resources.ToDtos(), CultureCs, _repository);

			grid1.CheckResource("link")
				 .ValueIs("b222")
				 .OwnerIs(grid2.Id);

			Assert.AreEqual(1, grid2.Resources.Count);
		}

		[Test]
		public void Reference_Replace_ByNewResource_Test()
		{
			var grid1 = DefaultGrid();
			var grid2 = DefaultGrid().WithResource(_allResources, "link", "b222", CultureCs, 2);
			var newResources = ResourcesHelper.EmptyResourcesDto()
											 .WithResource("link", "newlinkvalue", 0);


			grid1.UpdateResourceList(grid2.Resources.ToDtos(), CultureCs, _repository);
			grid1.CheckResource("link")
				  .ValueIs("b222")
				  .OwnerIs(grid2.Id);

			grid1.UpdateResourceList(newResources, CultureCs, _repository);
			grid1.CheckResource("link")
				.ValueIs("newlinkvalue")
				.OwnerIs(grid1.Id);


			Assert.AreEqual(1, grid2.Resources.Count);
		}

		[Test]
		public void UpdateResourceTest_LANGUAGE()
		{
			var grid1 = DefaultGrid().WithResource(_allResources, "link", "a111", CultureCs, 1);
			var grid2 = DefaultGrid().WithResource(_allResources, "link", "b222", CultureCs, 2);
			var grid3 = DefaultGrid().WithResource(_allResources, "link", "c333", CultureEn, 3);

			grid1.UpdateResourceList(grid2.Resources.ToDtos(), CultureCs, _repository);

			grid1.CheckResource("link", CultureCs)
				 .ValueIs("b222")
				 .OwnerIs(grid2.Id);

			grid1.UpdateResourceList(grid3.Resources.ToDtos(CultureEn), CultureEn, _repository);

			Assert.AreEqual(2, grid1.Resources.Count);

			grid1.CheckResource("link");
			grid1.CheckResource("link", CultureEn)
				 .ValueIs("c333")
				 .OwnerIs(grid3.Id);
		}

	}
}