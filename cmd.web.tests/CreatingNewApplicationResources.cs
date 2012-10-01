
using System;
using System.ComponentModel;
using Moq;
using NUnit.Framework;
using cms.Code.UserResources;
using cms.data;
using cms.data.Shared.Models;
using System.Linq;

namespace cms.web.tests
{
	[TestFixture]
	class CreatingNewApplicationResources
	{
		readonly ApplicationSetting defaultApp = new ApplicationSetting
		{
			Name = "prdel",
			DefaultLanguage = "cs",
			Id = Guid.NewGuid()
		};

		public ApplicationSetting CreateDefaultApp()
		{
			var mock = new Mock<JsonDataProvider>("00000000-0000-0000-0000-000000000000");
			mock.Setup(x => x.Add(It.IsAny<ApplicationSetting>()))
				.Returns(defaultApp);
			var db = mock.Object;
			return db.Add(new ApplicationSetting());
		}

		public IResourceManager GetDefaultResourceManager()
		{
			var app = CreateDefaultApp();
			return ResourceManager.Create(app.Id);
		}

		[TestFixtureSetUp]
		public void Setup()
		{
		}


		[Test]
		public void CreateAndCheckIfExists_test()
		{
			var app = CreateDefaultApp();
			var res = ResourceManager.Create(app.Id);
			Assert.IsTrue(res.Exist(app.Id));
		}

		[Test]
		public void GetAllAssets_test()
		{
			var resources = GetDefaultResourceManager();
			var tmpl = resources.Assets();

			Assert.IsNotNull(tmpl);
			Assert.IsTrue(tmpl.Count > 0);
		}

		[Test]
		public void Combine_scripts_test()
		{
			var resources = GetDefaultResourceManager();
			var tmpl = resources.Javascripts();


		}



		[Test]
		public void HtmlTemplate_test()
		{
			var resources = GetDefaultResourceManager();
			var tmpl = resources.HtmlTemplate("text", ViewModeType.Admin);
			Assert.IsNotNull(tmpl);
			Assert.IsNotNullOrEmpty(tmpl.Path);
		}

		[Test]
		public void HtmlTemplateToHtml_test()
		{
			var resources = GetDefaultResourceManager();

			var tmpl = resources.HtmlTemplate("text", ViewModeType.Admin);
			Console.WriteLine(tmpl.ToHtml());
			Assert.IsNotNull(tmpl.ToHtml());
		}
	
	}
}
