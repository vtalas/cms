using System;
using System.Data.Entity;
using NUnit.Framework;
using cms.data.Dtos;
using cms.data.EF;
using cms.data.EF.Initializers;
using cms.shared;

namespace cms.data.tests.EF
{
	[TestFixture]
	public class AddGridPageMethodTest
	{
		private SessionProvider SessionProvider { get; set; }

		public AddGridPageMethodTest()
		{
			Database.SetInitializer(new DropAndCreateAlwaysForce());
		}

		GridPageDto AddGridpage(string name, string link)
		{
			using (var db = SessionProvider.CreateSession())
			{
				var a = new GridPageDto
					        {
						        Name = name,
						        Link = link,
						        Category = CategoryEnum.Page
					        };

				var n = db.Session.Page.Add(a);
				Assert.AreEqual(name, n.Name);
				//Assert.AreEqual(link, n.ResourceDto.Value);
				return n;
			}
		}

		GridPageDto GetGridpage(Guid id)
		{
			using (var db = SessionProvider.CreateSession())
			{
				var n = db.Session.Page.Get(id);
				return n;
			}
		}

		[SetUp]
		public void Setup()
		{
		
			Xxx.DeleteDatabaseDataGenereateSampleData();
			SessionProvider = SessionProviderFactoryTest.GetSessionProvider();

		}


		[Test]
		public void AddGridpage_Basic()
		{
			const string name = "xxx";
			const string link = "AddGridpage_Basic";
			var n = AddGridpage(name, link);
			var g = GetGridpage(n.Id);

			Assert.AreEqual(name, g.Name);
			//Assert.AreEqual(link, g.ResourceDto.Value);
		}

		[Test]
		public void AddGridpage_Basic_linkExists()
		{
			const string name = "xxx";
			const string link = "AddGridpage_Basic_linkExists";

			AddGridpage(name, link);
			Assert.Throws<Exception>(() => AddGridpage(name + name, link));

		}
	}
}