using System;
using System.Data;
using NUnit.Framework;
using cms.data.DataProvider;
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
	public class GridElementDataProvider_Test : InjectableBase_Test
	{
		private DataProviderBase _dataProvider;

		public GridElementDataProvider_Test(Func<IRepository> repositoryCreator) : base(repositoryCreator)
		{
		}

		public GridElementDataProvider_Test()
			: this(new Base_MockDb().GetRepositoryMock)
		{
		}

		[SetUp]
		public void Setup()
		{
			Repository = RepositoryCreator();
			var subGridElement = AGridElement("text",3);
			var subGridElement2 = AGridElement("text",4);
			
			var application = AApplication("prd App")
				.WithGrid(
					AGrid(_gridId)
						.WithResource(SpecialResourceEnum.Link, "aaa")
						.WithResource("name", "NAME AAAA", CultureCs, 111)
						.WithGridElement(AGridElement("text", 0))
						.WithGridElement(AGridElement("text", 1))
						.WithGridElement(AGridElement("text", 2))
						.WithGridElement(subGridElement)
						.WithGridElement(AGridElement("text", 0).WithParent(subGridElement))
						.WithGridElement(AGridElement("text", 1).WithParent(subGridElement))
						.WithGridElement(subGridElement2)
						.WithGridElement(AGridElement("text", 1).WithParent(subGridElement2))


				).AddTo(Repository);

			_dataProvider = new DataProviderBase(application, Repository);
			Assert.IsNotNull(_dataProvider);
			
			Repository.TotalGridElementsCount_Check(8);
			Assert.AreEqual(1, Repository.ApplicationSettings.Count());
			Assert.AreEqual(1, Repository.Grids.Count());
		}

		[Test]
		public void AddGridElement_CheckCountAndCorrectPosition_test()
		{
			var gridelement = AGridElement("text",  1);
			_dataProvider.AddToGrid(gridelement, _gridId);
			var x = Repository.Grids.Get(_gridId);
			x.Check()
				.HasGridElementsCountAndValid(9)
				.HasGridElementIndexIn(null, 1);
		}

		[Test]
		public void AddGridElement_CheckCountAndCorrectPosition_testXX()
		{
			var gridelement = AGridElement("text",  1);
			_dataProvider.AddToGrid(gridelement, _gridId);
			var x = Repository.Grids.Get(_gridId);
			x.Check()
				.HasGridElementsCountAndValid(9)
				.HasGridElementIndexIn(null, 1);
		}

		[Test]
		public void TryAddToNonExistingGrid_test()
		{
			var gridelement = AGridElement("text",  1);
			Assert.Throws<ObjectNotFoundException>(() => _dataProvider.AddToGrid(gridelement, Guid.NewGuid()));
		}
	}

	[TestFixture]
	public class GridElementAbstract_Test : InjectableBase_Test
	{
		private GridElementAbstract _implemtation;
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

			var app1 = AApplication("prd App")
				.WithGrid(
					AGrid()
						.WithResource(SpecialResourceEnum.Link, "aaa")
						.WithResource("name", "NAME AAAA", CultureCs, 111)
				)
				.WithGrid(
					AGrid(_gridId)
						.WithResource(SpecialResourceEnum.Link, "bbb")
						.WithResource("name", "NAME BBB", CultureCs, 221)
						.WithResource("name", "NAME BBB EN", CultureEn, 222)
						.WithGridElement(AGridElement("text", 0 ))
						.WithGridElement(AGridElement("text", 1 ))
						.WithGridElement(
								AGridElement("text", 2 )
									.WithResource("aaa","aaavalue")
								)
				).AddTo(Repository);
			_implemtation = new GridElementAbstractImpl(app1, Repository);
		}


		[Test]
		public void aaa()
		{
			var el = _implemtation.AddToGrid(AGridElement("text", 1), _gridId);
			Assert.AreEqual(1, el.Position);
			var xx = new MenuItemDto();
			
		}

		GridElement AddToGgridData()
		{
			return AGridElement("text", Guid.NewGuid())
				.WithResource("xxx", "xxxvalue")
				.WithResource("xxx", "xxxvalueEn", CultureEn);
		}

		[Test]
		public void AddToGrid_addNewGRdItem_test()
		{
			var gridelement = AddToGgridData();

			var el = _implemtation.AddToGrid(gridelement, Guid.NewGuid());

			el.CheckResource("xxx", CultureCs).ValueIs("xxxvalue");
			el.CheckResource("xxx", CultureEn).ValueIs("xxxvalueEn");
			el.HasResoucesCount(2);
		}

		[Test]
		public void AddToGrid_addNewGRdiitem_test_XXX()
		{
			var gridelementsBefore = Repository.GridElements.Count();
			var gridelement = AddToGgridData();
			
			var el = _implemtation.AddToGrid(gridelement, Guid.NewGuid());

			el.CheckResource("xxx", CultureCs).ValueIs("xxxvalue");
			el.HasResoucesCount(2);
			el.CheckResource("xxx", CultureEn).ValueIs("xxxvalueEn");
			Repository.TotalGridElementsCount_Check(gridelementsBefore + 1);
		}

	}
}