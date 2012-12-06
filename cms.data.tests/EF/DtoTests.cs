using System;
using System.Collections.Generic;
using System.ComponentModel;
using NUnit.Framework;
using cms.data.Extensions;
using cms.data.Shared.Models;
using System.Linq;

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

		[Test]
		public void MenuDto_invalid_data()
		{
			Assert.Throws<InvalidEnumArgumentException>(() => new Grid().ToMenuDto());
		}

		[Test]
		public void MenuDto_test()
		{
			var root = new GridElement()
				           {
					           Parent = null,
					           Content = "",
					           Id = Guid.NewGuid(),
				           };
			var rootchild = new GridElement()
				           {
					           Parent = root,
					           Content = "",
					           Id = Guid.NewGuid(),
				           };
			var rootchild2 = new GridElement()
				           {
					           Parent = root,
					           Content = "",
					           Id = Guid.NewGuid(),
				           };
			

			var menu = new List<GridElement>
				           {
					           root,rootchild,rootchild2
				           };


			var dto = menu.ToChildren().ToList();
			
			Assert.AreEqual(1,dto.Count);
			Assert.AreEqual(2,dto[0].Children.Count());

		}

	}
}