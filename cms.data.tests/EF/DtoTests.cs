using System;
using NUnit.Framework;
using cms.data.Shared.Models;
using cms.data.Dtos;

namespace cms.data.tests.EF
{
	[TestFixture]
	public class DtoTests
	{
		Resource a = new Resource
		{
			Culture = "cs",
			Value = "Value",
			Id = 11,
			Key = "key"
		};
		
		[Test]
		public void guidTest()
		{
			var zeros = new Guid("00000000-0000-0000-0000-000000000000");
			var to = Guid.NewGuid();



			Console.WriteLine(to.CompareTo(to));
			Console.WriteLine(to.CompareTo(zeros));
			Console.WriteLine(to.Equals(to));
			Console.WriteLine(to.Equals(zeros));
		}

		[Test]
		public void ResourceDto_toDto_test()
		{
			var dto = a.ToDto();
			Assert.AreEqual(a.Value, dto.Value);
			Assert.AreEqual(a.Id, dto.Id);
		}

	}
}