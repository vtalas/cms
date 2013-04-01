using System;
using System.Data;
using NUnit.Framework;
using cms.data.DataProvider;
using cms.data.Dtos;
using cms.data.Extensions;
using cms.data.Repository;
using cms.data.Shared.Models;
using cms.data.tests._Common;
using cms.shared;
using System.Linq;

namespace cms.data.tests.PageAbstractTests
{
	[TestFixture]
	public class DataProvider_GridElement_Test : DataProvider_GridElement_Test_base
	{

		public DataProvider_GridElement_Test(Func<IRepository> repositoryCreator)
			: base(repositoryCreator)
		{
		}

		public DataProvider_GridElement_Test()
			: base(new Base_MockDb().GetRepositoryMock)
		{
		}

		[Test]
		public void AddGridElement_CheckCountAndCorrectPosition_test()
		{
			var repository = RepositorySeed();
			using (var dataProvider = new DataProviderBase(Application, repository))
			{
				var gridelement = AGridElement("text", 1).WithContent("prdel");
				dataProvider.AddToGrid(gridelement.ToDto(), GridId);
				var grid = repository.Grids.Get(GridId);

				foreach (var element in grid.GridElements.Where(x => x.Parent == null).OrderBy(x => x.Position))
				{
					Console.WriteLine(element.Position + " " + element.Content + " parent: " + element.Parent);
				}

				grid.Check()
					.HasGridElementsCountAndValid(9)
					.HasGridElementIndex(null, 1);
			}
		}

		[Test]
		public void AddGridElement_AddExistingGridElement_test()
		{
			var repository = RepositorySeed();
			using (var db = new DataProviderBase(Application, repository))
			{
				var gridelement = AGridElement("text", 1);
				repository.Add(gridelement);
				repository.SaveChanges();

				repository.TotalGridElementsCount_Check(GridElementsBefore + 1);

				db.AddToGrid(gridelement.ToDto(), GridId);
				var x = repository.Grids.Get(GridId);
				x.Check()
					.HasGridElementsCountAndValid(9)
					.HasGridElementIndex(null, 1);

				repository.TotalGridElementsCount_Check(GridElementsBefore + 1);
			}
		}

		[Test]
		public void TryAddToNonExistingGrid_test()
		{
			var repository = RepositorySeed();
			using (var db = new DataProviderBase(Application, repository))
			{
				var gridelement = AGridElement("text", 1);
				Assert.Throws<ObjectNotFoundException>(() => db.AddToGrid(gridelement.ToDto(), Guid.NewGuid()));
			}
		}

		public void UpdatePosition_UpdateBy(GridElement update, Guid gridId, DataProviderBase db, IRepository repository)
		{
			var gridElementId = update.Id;
			var newPosition = update.Position;

			db.Update(update.ToDto());

			var gridAfter = repository.Grids.Get(gridId);

			gridAfter.GridElements
				.Where(x => x.Parent == update.Parent)
				.PrintGridElementsPositons(update.Id);

			gridAfter.Check()
				.HasGridElementsCountAndValid(8)
				.HasGridElementIndex(null, newPosition);

			var updateAfter = gridAfter.GridElement(gridElementId);
			Assert.AreEqual("new position", updateAfter.Content);
			Assert.AreEqual(newPosition, updateAfter.Position);
			Assert.AreEqual(GridElementsBefore, repository.GridElements.Count());

		}

		public void UpdatePosition_withSameParents_testHelper(int itemFromPosition, int newPosition)
		{
			var repository = RepositorySeed();
			using (var db = new DataProviderBase(Application, repository))
			{
				var grid = repository.Grids.Get(GridId);
				var gridElements = grid.GridElements.Where(x => x.Parent == null);

				Console.WriteLine("puvodni " + itemFromPosition + "  nova :" + newPosition);

				var gridElement = gridElements.Single(x => x.Position == itemFromPosition);
				gridElements.PrintGridElementsPositons(gridElement.Id);

				var update = AGridElement("text", gridElement.Id, newPosition)
					.WithContent("new position")
					.IsPropertyOf(grid);

				UpdatePosition_UpdateBy(update, GridId, db, repository);
			}
		}

		[Test]
		public void UpdatePosition_withDifferentParents_moveUpPosition_test()
		{
			var repository = RepositorySeed();
			using (var db = new DataProviderBase(Application, repository))
			{
				const int fromPosition = 0;
				const int toPosition = 1;

				var gridId = GridId;
				var grid = repository.Grids.Get(gridId);

				var gridElement = grid.GridElements.Single(x => x.Position == fromPosition && GridElement.EqualsById(x.Parent, null));
				var newParent = grid.GridElements.Single(x => x.Content == SubElement1Content);

				UpdatePosition_withDifferentParents(gridId, gridElement, toPosition, newParent, db, repository);
			}
		}

		[Test]
		public void UpdatePosition_withDifferentParents_samePosition_test()
		{
			var repository = RepositorySeed();
			using (var db = new DataProviderBase(Application, repository))
			{
				const int fromPosition = 1;
				const int toPosition = 1;

				var gridId = GridId;
				var grid = repository.Grids.Get(gridId);

				var gridElement = grid.GridElements.Single(x => x.Position == fromPosition && GridElement.EqualsById(x.Parent, null));
				var newParent = grid.GridElements.Single(x => x.Content == SubElement1Content);

				UpdatePosition_withDifferentParents(gridId, gridElement, toPosition, newParent, db, repository);
			}
		}

		[Test]
		public void UpdatePosition_withDifferentParents_moveDownPosition_test()
		{
			var repository = RepositorySeed();
			using (var db = new DataProviderBase(Application, repository))
			{
				const int fromPosition = 4;
				const int toPosition = 1;

				var gridId = GridId;
				var grid = repository.Grids.Get(gridId);

				var gridElement = grid.GridElements.Single(x => x.Position == fromPosition && GridElement.EqualsById(x.Parent, null));
				var newParent = grid.GridElements.Single(x => x.Content == SubElement1Content);

				UpdatePosition_withDifferentParents(gridId, gridElement, toPosition, newParent, db, repository);
			}
		}

		public void UpdatePosition_withDifferentParents(Guid gridId, GridElement gridElement, int newPosition, GridElement newparent, DataProviderBase db, IRepository repository)
		{
			var gridBefore = repository.Grids.Get(gridId);

			Console.WriteLine("puvodni " + gridElement.Position + "  nova :" + newPosition);

			var update = AGridElement("text", gridElement.Id, newPosition)
				.WithContent("new position")
				.IsPropertyOf(gridElement.Grid)
				.WithParent(newparent);

			gridBefore.GridElements
				.Where(x => x.Parent == gridElement.Parent)
				.PrintGridElementsPositons(gridElement.Id);

			UpdatePosition_UpdateBy(update, gridId, db, repository);
		}

		[Test]
		public void UpdatePosition_withSameParents_down_1_0_test()
		{
			UpdatePosition_withSameParents_testHelper(1, 0);
		}

		[Test]
		public void UpdatePosition_withSameParents_down_4_0_test()
		{
			UpdatePosition_withSameParents_testHelper(4, 0);
		}

		[Test]
		public void UpdatePosition_withSameParents_up_test()
		{
			UpdatePosition_withSameParents_testHelper(4, 2);
		}

		[Test]
		public void UpdatePosition_withSameParents_up_2_4_test()
		{
			UpdatePosition_withSameParents_testHelper(2, 4);
		}

		[Test]
		public void UpdatePosition_withSameParents_up_0_1_test()
		{
			UpdatePosition_withSameParents_testHelper(0, 1);
		}

		[Test]
		public void TryUpadteNonExisting_test()
		{
			var repository = RepositorySeed();
			using (var db = new DataProviderBase(Application, repository))
			{
				var update = AGridElement("text", Guid.NewGuid(), 6);
				update.Content = "xxx";
				Assert.Throws<ObjectNotFoundException>(() => db.Update(update.ToDto()));
			}
		}

		[Test]
		public void TryUpdate_otherOwner_test()
		{
			var repository = RepositorySeed();
			using (var db = new DataProviderBase(Application, repository))
			{
				var gridEelemOtherApp = AGridElement("text", 2);
				AApplication("prd App")
					.WithGrid(
						AGrid()
							.WithGridElement(gridEelemOtherApp)
					).AddTo(repository);
				Assert.Throws<ObjectNotFoundException>(() => db.Update(gridEelemOtherApp.ToDto()));
			}
		}

		[Test]
		public void Delete_test()
		{
			var repository = RepositorySeed();
			using (var db = new DataProviderBase(Application, repository))
			{
				db.DeleteGridElement(GridElementId);
				Assert.AreEqual(GridElementsBefore - 1, repository.GridElements.Count());
				Assert.AreEqual(ResourcesBefore - 1, repository.Resources.Count());
			}
		}

	}

	public class DataProvider_GridElement_Test_base : InjectableBase_Test
	{
		protected const string SubElement1Content = "s1";
		protected const string SubElement2Content = "s2";
		protected int GridElementsBefore;
		protected int ResourcesBefore;
		protected ApplicationSetting Application { get; set; }

		public DataProvider_GridElement_Test_base(Func<IRepository> repositoryCreator)
			: base(repositoryCreator)
		{
		}

		protected IRepository RepositorySeed()
		{
			var repository = RepositoryCreator();
			var subGridElement = AGridElement("text", 3).WithContent(SubElement1Content);
			var subGridElement2 = AGridElement("text", 4).WithContent(SubElement2Content);

			Application = AApplication("prd App")
				.WithGrid(
					AGrid(GridId)
						.WithResource(SpecialResourceEnum.Link, "aaa")
						.WithResource("name", "NAME AAAA", CultureCs, 111)
						.WithGridElement(AGridElement("text",GridElementId, 0).WithResource("link", "xxx"))
						.WithGridElement(AGridElement("text", 1))
						.WithGridElement(AGridElement("text", 2))
						.WithGridElement(subGridElement)
						.WithGridElement(AGridElement("text", 0).WithParent(subGridElement))
						.WithGridElement(AGridElement("text", 1).WithContent("chujs").WithParent(subGridElement))
						.WithGridElement(subGridElement2)
						.WithGridElement(AGridElement("text", 0).WithParent(subGridElement2))
				).AddTo(repository);

			GridElementsBefore = repository.GridElements.Count();
			repository.TotalGridElementsCount_Check(GridElementsBefore);
			Assert.AreEqual(1, repository.ApplicationSettings.Count(), "There should be an application ");
			Assert.AreEqual(1, repository.Grids.Count());
			ResourcesBefore = repository.Resources.Count();
			return repository;
		}

	}
}