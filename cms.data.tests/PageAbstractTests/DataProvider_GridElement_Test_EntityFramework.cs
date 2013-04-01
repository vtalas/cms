using System;
using System.Data.Entity;
using NUnit.Framework;
using cms.data.DataProvider;
using cms.data.EF;
using cms.data.EF.Initializers;
using cms.data.EF.RepositoryImplementation;
using cms.data.Repository;
using cms.shared;
using System.Linq;

namespace cms.data.tests.PageAbstractTests
{
	[TestFixture]
	public class DataProvider_GridElement_Test_EntityFramework : DataProvider_GridElement_Test_base
	{
		public DataProvider_GridElement_Test_EntityFramework(Func<IRepository> repositoryCreator)
			: base(repositoryCreator)
		{
		}

		public DataProvider_GridElement_Test_EntityFramework()
			: base(() => new EfRepository())
		{
		}

		[SetUp]
		public void SetUp()
		{
			Database.SetInitializer(new DropAndCreateAlwaysForce());
			Xxx.DeleteDatabaseData();
			SecurityProvider.EnsureInitialized(true);
			SharedLayer.Init();

		}

		public void RunTestDelegate(Action<DataProvider_GridElement_Test> test)
		{
			var testsInstance = new DataProvider_GridElement_Test(() => new EfRepository());
			test(testsInstance);
		}

		[Test]
		public void AddGridElement_AddExistingGridElement_test()
		{
			RunTestDelegate(instance => instance.AddGridElement_AddExistingGridElement_test());
		}

		[Test]
		public void AddGridElement_CheckCountAndCorrectPosition_test()
		{
			RunTestDelegate(instance => instance.AddGridElement_CheckCountAndCorrectPosition_test());
		}

		[Test]
		public void TryAddToNonExistingGrid_test()
		{
			RunTestDelegate(instance => instance.TryAddToNonExistingGrid_test());
		}
		[Test]
		public void TryUpadteNonExisting_test()
		{
			RunTestDelegate(instance => instance.TryUpadteNonExisting_test());
		}
		[Test]
		public void TryUpdate_otherOwner_test()
		{
			RunTestDelegate(instance => instance.TryUpdate_otherOwner_test());
		}
		[Test]
		public void UpdatePosition_withDifferentParents_moveDownPosition_test()
		{
			RunTestDelegate(instance => instance.UpdatePosition_withDifferentParents_moveDownPosition_test());
		}
		[Test]
		public void UpdatePosition_withDifferentParents_samePosition_test()
		{
			RunTestDelegate(instance => instance.UpdatePosition_withDifferentParents_samePosition_test());
		}
		[Test]
		public void UpdatePosition_withSameParents_down_1_0_test()
		{
			RunTestDelegate(instance => instance.UpdatePosition_withSameParents_down_1_0_test());
		}

		[Test]
		public void UpdatePosition_withSameParents_down_4_0_test()
		{
			RunTestDelegate(instance => instance.UpdatePosition_withSameParents_down_4_0_test());
		}

		[Test]
		public void UpdatePosition_withSameParents_up_0_1_test()
		{
			RunTestDelegate(instance => instance.UpdatePosition_withSameParents_up_0_1_test());
		}

		[Test]
		public void UpdatePosition_withSameParents_up_2_4_test()
		{
			RunTestDelegate(instance => instance.UpdatePosition_withSameParents_up_2_4_test());
		}

		[Test]
		public void UpdatePosition_withSameParents_up_test()
		{
			RunTestDelegate(instance => instance.UpdatePosition_withSameParents_up_test());
		}
		
		[Test]
		public void Delete_test_delegate()
		{
			RunTestDelegate(instance => instance.Delete_test());
		}

		[Test]
		public void Delete_test()
		{
			var repository = RepositorySeed();
			using (var db = new DataProviderBase(Application, repository))
			{
				db.DeleteGridElement(GridElementId);
				Assert.AreEqual(GridElementsBefore - 1 , repository.GridElements.Count());
				Assert.AreEqual(ResourcesBefore - 1 , repository.Resources.Count());
			}
		
		}


	}
}