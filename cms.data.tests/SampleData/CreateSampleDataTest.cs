using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using NUnit.Framework;
using cms.data.EF;
using cms.data.EF.Initializers;

namespace cms.data.tests.SampleData
{
	[TestFixture]
	public class CreateSampleDataTest
	{
		private JsonDataEf Repo { get; set; }

		[SetUp]
		public void Setup()
		{
			var context = new EfContext("EfContextSampleData");
			Database.SetInitializer(new DropAndCreate());
			Repo = new JsonDataEf("aaa", context);
		}


		[Test]
		public void Create()
		{
			var a = JsonDataEf.Applications();
			Console.WriteLine(a.Count());
		}






	}
}