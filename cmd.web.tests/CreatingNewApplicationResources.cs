using System;
using System.IO;
using BundleTransformer.Core.FileSystem;
using BundleTransformer.Core.Web;
using Moq;
using NUnit.Framework;
using cms.Code.UserResources;
using System.Linq;
using cms.web.tests.Code;

namespace cms.web.tests
{
	[TestFixture]
	class CreatingNewApplicationResources : UserResourceManagerTestBase
	{
		protected string RootDir { get; set; }

		public CreatingNewApplicationResources()
		{
			RootDir = Path.GetFullPath(".");
		}

		IHttpApplicationInfo HttpApplicationInfoObject()
		{
			return new HttpApplicationInfo(RootDir, RootDir);
		}


		IFileSystemWrapper FileSystemWrapperMock()
		{
			var mock = new Mock<IFileSystemWrapper>();
			mock.Setup(x => x.FileExists(It.IsAny<string>())).Returns(true);

			mock.Setup(x => x.GetFileTextContent(Path.Combine(RootDir, Id.ToString(), "js1.js"))).Returns("alert('js1client')");
			mock.Setup(x => x.GetFileTextContent(Path.Combine(RootDir, Id.ToString(), "coffee.coffee"))).Returns("test -> console.log('coffee')");
			mock.Setup(x => x.GetFileTextContent(Path.Combine(RootDir, Id.ToString(), "default/js1.js"))).Returns("alert('js1defaltclient')");
			mock.Setup(x => x.GetFileTextContent(Path.Combine(RootDir, Id.ToString(), "js2.js"))).Returns("alert('js2client')");
			mock.Setup(x => x.GetFileTextContent(Path.Combine(RootDir, Id.ToString(), "js2_admin.js"))).Returns("alert('js2admin')");
			mock.Setup(x => x.GetFileTextContent(Path.Combine(RootDir, Id.ToString(), "t1.thtml"))).Returns("<span>template1client</span>");
			mock.Setup(x => x.GetFileTextContent(Path.Combine(RootDir, Id.ToString(), "css1.css"))).Returns("body{color:red;}");
			
			return mock.Object;
		}

		public IResourceManager CreateDefaultResourceManagerFilesMocked()
		{
			var app = CreateDefaultApp();
			return UserResourceManager.Create(app.Id, HttpApplicationInfoObject(), FileSystemWrapperMock());
		}
		
		[Test]
		public void NoAssetsOnBeginning_test()
		{
			var resources = CreateDefaultResourceManagerFilesMocked();
			Assert.IsTrue(resources.Assets.Count == 0);
		}

		[Test]
		public void IncludeFile_script_test()
		{
			var resources = CreateDefaultResourceManagerFilesMocked();
			resources.Include("js1.js");
			Assert.IsTrue(resources.Assets.Count == 1);
			Assert.IsTrue(resources.Assets["js1.js"].Content.Length > 0);
			Console.WriteLine(resources.Assets["js1.js"].Content);
		}


		[Test]
		public void IsAdmin_test()
		{
			var resources = CreateDefaultResourceManagerFilesMocked();
			resources.Include("js2.js");
			resources.Include("js2_admin.js");

			Assert.IsTrue(resources.Assets["js2_admin.js"].IsAdmin);
			Assert.IsFalse(resources.Assets["js2.js"].IsAdmin);
		}

		[Test]
		public void IncludeCoffee_test()
		{
			var resources = CreateDefaultResourceManagerFilesMocked();
			resources.Include("coffee.coffee");

			// coffee se prelozil
			Assert.IsTrue(resources.Assets["coffee.coffee"].Content.Contains("return console.log('coffee')"));
			Assert.IsTrue(resources.RenderScripts().Contains("return console.log('coffee')"));

		}

		[Test]
		public void Render_test()
		{
			var resources = CreateDefaultResourceManagerFilesMocked();
			resources.Include("js1.js");
			resources.Include("js2.js");
			resources.Include("t1.thtml");
			resources.Include("css1.css");

			Assert.AreEqual(resources.Assets.Where(x => x.Value.IsScript).Sum(x => x.Value.Content.Length), resources.RenderScripts().Length);
			Assert.IsTrue(resources.RenderHtmlTemplates().Contains("span") );
			Assert.AreEqual(resources.Assets.Where(x => x.Value.IsStylesheet).Sum(x => x.Value.Content.Length), resources.RenderStyleSheets().Length);
		}

		[Test]
		public void Render_Withoverwriting_test()
		{
			var resources = CreateDefaultResourceManagerFilesMocked();
			resources.Include("js1.js");
			resources.Include("default/js1.js");

			var render = resources.RenderScripts();
			var asset1Default = resources.Assets["js1.js"]; 
			Assert.AreEqual(asset1Default.Content, render);
			Assert.AreEqual(1, resources.Assets.Count);
		}

		[Test]
		public void CheckAssetType_Script_test()
		{
			var resources = CreateDefaultResourceManagerFilesMocked();
			resources.Include("js1.js");
			resources.Include("coffee.coffee");
			resources.Include("ts1.ts");

			Assert.IsTrue(resources.Assets["js1.js"].IsScript);
			Assert.IsTrue(resources.Assets["js1.js"].AssetTypeExtended == AssetTypeExtened.JavaScript);

			Assert.IsTrue(resources.Assets["coffee.coffee"].IsScript);
			Assert.IsTrue(resources.Assets["coffee.coffee"].AssetTypeExtended == AssetTypeExtened.CoffeeScript);
	
			Assert.IsTrue(resources.Assets["ts1.ts"].IsScript);
			Assert.IsTrue(resources.Assets["ts1.ts"].AssetTypeExtended == AssetTypeExtened.TypeScript);
		}

		[Test]
		public void CheckAssetType_HtmlTemplate_test()
		{
			var resources = CreateDefaultResourceManagerFilesMocked();
			resources.Include("template1.thtml");
			resources.Include("templatexxx");

			Assert.IsTrue(resources.Assets["template1.thtml"].IsHtmlTemplate);
			Assert.IsTrue(resources.Assets["template1.thtml"].AssetTypeExtended == AssetTypeExtened.HtmlTemplate);

			Assert.IsTrue(resources.Assets["templatexxx"].AssetTypeExtended == AssetTypeExtened.Unknown);
		}

	}
}
