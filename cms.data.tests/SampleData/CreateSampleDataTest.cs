using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using NUnit.Framework;
using cms.data.EF;
using cms.data.EF.Initializers;
using cms.data.Models;

namespace cms.data.tests.SampleData
{
	[TestFixture]
	public class CreateSampleDataTest
	{
		private JsonDataEf repo { get; set; }

		[SetUp]
		public void Setup()
		{
			var context = new EfContext("EfContextSampleData");
			Database.SetInitializer(new DropAndCreate());
			repo = new JsonDataEf("aaa", context);
		}


		[Test]
		public void create()
		{
			var a = repo.Applications();

			Console.WriteLine(a.Count());

		}






	}
}