using System;
using System.Collections.Generic;
using System.Data;
using NUnit.Framework;
using cms.data.DataProvider;
using cms.data.Extensions;
using cms.data.Repository;
using cms.data.Shared.Models;
using cms.data.tests._Common;
using cms.shared;
using System.Linq;

namespace cms.data.tests.PageAbstractTests
{
	[TestFixture]
	public class DataProvider_GridElement_Test : InjectableBase_Test
	{
		private DataProviderBase _dataProvider;

		public DataProvider_GridElement_Test(Func<IRepository> repositoryCreator) : base(repositoryCreator)
		{
		}

		public DataProvider_GridElement_Test()
			: this(new Base_MockDb().GetRepositoryMock)
		{
		}

		[SetUp]
		public void Setup()
		{
			Repository = RepositoryCreator();
			var subGridElement = AGridElement("text", 3).WithContent("subelement 1");
			var subGridElement2 = AGridElement("text", 4).WithContent("subelement 2");
			
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
						.WithGridElement(AGridElement("text", 0).WithParent(subGridElement2))
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
			var gridelement = AGridElement("text",  1).WithContent("prdel");
			_dataProvider.AddToGrid(gridelement, _gridId);
			var grid = Repository.Grids.Get(_gridId);

			foreach (var element in grid.GridElements.Where(x => x.Parent == null).OrderBy(x => x.Position))
			{
				Console.WriteLine(element.Position + " " + element.Content + " parent: " + element.Parent);
			}

			grid.Check()
				.HasGridElementsCountAndValid(9)
				.HasGridElementIndex(null, 1);
		}

		[Test]
		public void AddGridElement_AddExistingGridElement_test()
		{
			var gridElementsBefore = Repository.GridElements.Count();
			var gridelement = AGridElement("text", 1);
			Repository.Add(gridelement);
			Repository.SaveChanges();

			Repository.TotalGridElementsCount_Check(gridElementsBefore + 1);

			_dataProvider.AddToGrid(gridelement, _gridId);
			var x = Repository.Grids.Get(_gridId);
			x.Check()
				.HasGridElementsCountAndValid(9)
				.HasGridElementIndex(null, 1);

			Repository.TotalGridElementsCount_Check(gridElementsBefore + 1);
		}

		[Test]
		public void TryAddToNonExistingGrid_test()
		{
			var gridelement = AGridElement("text",  1);
			Assert.Throws<ObjectNotFoundException>(() => _dataProvider.AddToGrid(gridelement, Guid.NewGuid()));
		}


		public void UpdatePosition_UpdateBy(Guid gridElementId, GridElement update)
		{
			var newPosition = update.Position;
 
			_dataProvider.Update(update);

			var gridAfter = Repository.Grids.Get(update.Grid.Id);

			gridAfter.GridElements
				.Where(x => x.Parent == null)
				.OrderBy(x => x.Position)
				.PrintGridElements(x => x.Position + " " + x.Content);

			gridAfter.Check()
				.HasGridElementsCountAndValid(8)
				.HasGridElementIndex(null, newPosition);

			var updateAfter = gridAfter.GridElement(gridElementId);
			Assert.AreEqual("new position", updateAfter.Content);
			Assert.AreEqual(newPosition, updateAfter.Position);

			Console.WriteLine("");
			
		}
		public void UpdatePosition_withSameParents_testHelper(int itemFromPosition, int newPosition)
		{
			var grid = Repository.Grids.Get(_gridId);
			var gridElement = grid.GridElements.Single(x => x.Position == itemFromPosition && x.Parent == null);

			Console.WriteLine("puvodni " + gridElement.Position + "  nova :" + newPosition);

			var update = AGridElement("text", gridElement.Id, newPosition)
								.WithContent("new position")
								.IsPropertyOf(grid);

			UpdatePosition_UpdateBy(gridElement.Id, update);
		}

		[Test]
		public void UpdatePosition_withSameParents_test()
		{
			UpdatePosition_withSameParents_testHelper(1, 0);
			Setup();

			UpdatePosition_withSameParents_testHelper(4, 0);
			Setup();

			UpdatePosition_withSameParents_testHelper(4, 2);
			Setup();

			UpdatePosition_withSameParents_testHelper(2, 4);
			Setup();

			UpdatePosition_withSameParents_testHelper(0, 1);
			Setup();
		}

		[Test]
		public void TryUpadteNonExisting_test()
		{
			var update = AGridElement("text", Guid.NewGuid(), 6);
			update.Content = "xxx";
			_dataProvider.Update(update);
		}

		[Test]
		public void TryUpdate_otherOwner_test()
		{
			var gridEelemOtherApp = AGridElement("text", 2);
			AApplication("prd App")
				.WithGrid(
					AGrid()
						.WithGridElement(gridEelemOtherApp)
				).AddTo(Repository);

			
			_dataProvider.Update(gridEelemOtherApp);

		}

	}
	public static class PrintHelpers
	{
		public static void PrintGridElements(this IEnumerable<GridElement> gridElements, Func<GridElement,string> print )
		{
			foreach (var element in gridElements)
			{
				Console.WriteLine(print(element));
			}

		}
	}

}