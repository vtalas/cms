using System;
using System.Collections.Generic;
using NUnit.Framework;
using cms.data.EF.DataProvider;
using cms.data.Shared.Models;
using cms.data.tests._Common;
using cms.shared;

namespace cms.data.tests.PageAbstractTests
{
	[TestFixture]
	public class PageAbstract_Test_DBMock : NoDbBase_Test
	{
		private PageAbstractImpl Page { get; set; }
		private readonly Guid _gridId = new Guid("c78ee05e-1115-480b-9ab7-a3ab3c0f6643");

		[SetUp]
		public void Setup()
		{
			_allResources = new List<Resource>();
			_repository = GetRepositoryMock();
			SharedLayer.Init();

			var app = CreateDefaultApplication("prd App")
				.WithGrid(
					CreateDefaultGrid()
						.WithResource("link", "aaaaaaaa")
						.WithResource("name", "NAME AAAA", CultureCs, 111)
				)
				.WithGrid(
					CreateDefaultGrid(_gridId)
						.WithResource("link", "bbbbbbbb")
						.WithResource("name", "NAME BBB", CultureCs, 222)
				);

			Page = new PageAbstractImpl(app, _repository);

		}

		[Test]
		public void Get_ByLink_test()
		{
			

		}

		[Test]
		public void Get_ById_test()
		{
			foreach (var item in _allResources)
			{
				Console.WriteLine(item.Value);
			}
			var gridpage = Page.Get(_gridId);
			Assert.IsNotNull(gridpage);
			Assert.AreEqual(gridpage.Id, _gridId);
		}

		[Test]
		public void List_test()
		{
			Assert.IsTrue(false);
		}

		[Test]
		public void Add_test()
		{
			Assert.IsTrue(false);
		}

		[Test]
		public void Update_test()
		{
			Assert.IsTrue(false);
		}
	
		[Test]
		public void Delete_test()
		{
			Assert.IsTrue(false);
		}


	}
}