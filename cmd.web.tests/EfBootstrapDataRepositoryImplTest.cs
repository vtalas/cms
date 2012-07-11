using System;
using System.Data.Entity;
using NUnit.Framework;
using cms.Code.Bootstraper.DataRepository;
using cms.data.EF.Bootstrap;
using cms.data.EF.Initializers;

namespace cms.web.tests
{
	[TestFixture]
	public class EfBootstrapDataRepositoryImplTest
	{
		BootstrapDataRepository repo { get; set; }

		[SetUp]
		public void EfBootstrapGeneratorTest_setup()
		{
			repo = new EfBootstrapDataRepositoryImpl();
			Database.SetInitializer(new DropAndCreate());
		}

		[Test]
		public void DefaultBoostrapExistTest()
		{
			var x = repo.Default();
			Console.WriteLine(x);
			Assert.IsNotNull(x);
			Assert.AreEqual((byte)Boostrapperstatus.Default, (int)x.SelectToken("Status")   );
		}
	
	}
	
	[TestFixture]
	public class SipmleTests
	{

		[Test]
		public void DefaultBoostrapExistTestEE()
		{
			Console.WriteLine((byte)Boostrapperstatus.Default);
		}
		[Test]
		public void DefaultBoostrapExistTestEEXX()
		{
			Console.WriteLine((Boostrapperstatus)2);
		}


	}
}

