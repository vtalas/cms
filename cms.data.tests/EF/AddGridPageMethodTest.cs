using System;
using System.Data.Entity;
using NUnit.Framework;
using cms.data.Dtos;
using cms.data.EF.Initializers;
using cms.shared;

namespace cms.data.tests.EF
{
	[TestFixture]
	public class AddGridPageMethodTest
	{
		public AddGridPageMethodTest()
		{
			Database.SetInitializer(new DropAndCreate());
		}

		GridPageDto AddGridpage(string name, string link)
		{
			using (var db = new SessionManager().CreateSession)
			{
				var a = new GridPageDto
					        {
						        Name = name,
						        ResourceDto = new ResourceDtoLoc { Value = link },
						        Category = CategoryEnum.Page
					        };

				var n = db.Page.Add(a);
				Assert.AreEqual(name, n.Name);
				Assert.AreEqual(link, n.ResourceDto.Value);
				return n;
			}
		}

		GridPageDto GetGridpage(Guid id)
		{
			using (var db = new SessionManager().CreateSession)
			{
				var n = db.Page.Get(id);
				return n;
			}
		}

		[SetUp]
		public void Setup()
		{
			Xxx.DeleteDatabaseData();
		}

		[Test]
		public void AddGridpage_Basic()
		{
			var name = "xxx";
			var link = "AddGridpage_Basic";
			var n = AddGridpage(name, link);
			var g = GetGridpage(n.Id);

			Assert.AreEqual(name, g.Name);
			Assert.AreEqual(link, g.ResourceDto.Value);
		}

		[Test]
		public void AddGridpage_Basic_linkExists()
		{
			const string name = "xxx";
			const string link = "AddGridpage_Basic_linkExists";

			AddGridpage(name, link);
			Assert.Throws<ArgumentException>(() => AddGridpage(name + name, link));

		}
	}
}