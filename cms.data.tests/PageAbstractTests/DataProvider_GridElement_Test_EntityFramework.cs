using System;
using System.Data.Entity;
using NUnit.Framework;
using cms.data.EF;
using cms.data.EF.Initializers;
using cms.data.EF.RepositoryImplementation;
using cms.data.tests._Common;
using cms.shared;

namespace cms.data.tests.PageAbstractTests
{
	[TestFixture]
	public class DataProvider_GridElement_Test_EntityFramework : Base_Test
	{
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
			using (var db = new EfContext())
			{
				var testsInstance = new DataProvider_GridElement_Test(() => new EfRepository(db));
				testsInstance.Setup();
				test(testsInstance);
			}
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


	}
}