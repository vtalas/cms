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
			Assert.AreEqual(a.Culture, dto.Culture);
			Assert.AreEqual(a.Value, dto.Value);
			Assert.AreEqual(a.Id, dto.Id);
			Assert.AreEqual(a.Key, dto.Key);
		}

		[Test]
		public void ResourceDto_UpdateValues_test()
		{
			var dto = a.ToDto();
			dto.Value = "xxxxxxxxxxx";
			dto.Key = "xxxxxxxxxxx";
			dto.Culture = "xxxxxxxxxxx";

			var updated = a.UpdateValues(dto);

			Assert.AreEqual(updated.Culture, dto.Culture);
			Assert.AreEqual(updated.Value, dto.Value);
			Assert.AreEqual(updated.Key, dto.Key);
			Assert.AreEqual(updated.Id, dto.Id);
		
		}
	}
}