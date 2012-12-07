using System;
using System.Data.Entity;
using NUnit.Framework;
using cms.data.EF;
using cms.data.EF.RepositoryImplementation;
using cms.data.EF.Initializers;
using cms.data.tests.EF;
using cms.data.tests._Common;

namespace cms.data.tests.UpdateResourceList
{
	[TestFixture]
	public class UpdateResourceList_Test_WithDB : Base_Test
	{

		public UpdateResourceList_Test_WithDB()
		{
			Database.SetInitializer(new DropAndCreateAlwaysForce());
		}

		public void RunTestDelegate(Action<UpdateResourceList_Test_NoDB> test)
		{
			using (var db = new EfContext())
			{
				var testsInstance = new UpdateResourceList_Test_NoDB(() => new EfRepository(db));
				testsInstance.Setup();
				test(testsInstance);
			}
		}

		[SetUp]
		public void SetUp()
		{
			Xxx.DeleteDatabaseDataGenereateSampleData();
		}

		[Test]
		public void Values_AddNew_test()
		{
			RunTestDelegate(x => x.Values_AddNew_test());
		}

		[Test]
		public void Values_AddNew_NewKeys_test()
		{
			RunTestDelegate(x => x.Values_AddNew_NewKeys_test());
		}

		[Test]
		public void Values_Replace_ByNewResources_test()
		{
			RunTestDelegate(x => x.Values_Replace_ByNewResources_test());
		}

		[Test]
		public void UpdateResourceTest_checkOwnerShip()
		{
			RunTestDelegate(x => x.UpdateResourceTest_checkOwnerShip());
		}

		[Test]
		public void Values_Replace_ByNewResources_SearchById_test()
		{
			RunTestDelegate(x => x.Values_Replace_ByNewResources_SearchById_test());
		}

		[Test]
		public void Reference_AddNew_test()
		{
			RunTestDelegate(x => x.Reference_AddNew_test());
		}

		[Test]
		public void Reference_Replace_ByAnotherReference_test()
		{
			RunTestDelegate(x => x.Reference_Replace_ByAnotherReference_test());
		}

		[Test]
		public void Reference_Replace_ByNewResource_Test()
		{
			RunTestDelegate(x => x.Reference_Replace_ByNewResource_Test());
		}

		[Test]
		public void UpdateResourceTest_LANGUAGE()
		{
			RunTestDelegate(x => x.UpdateResourceTest_LANGUAGE());
		}
	}
}