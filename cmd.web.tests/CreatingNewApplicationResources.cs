using System;
using System.IO;
using BundleTransformer.Core.FileSystem;
using BundleTransformer.Core.Web;
using Moq;
using NUnit.Framework;
using cms.Code.UserResources;
using System.Linq;

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

			Assert.IsTrue(resources.Assets.Single(x => x.Value.Content.Contains("js2admin")).Value.IsAdmin);
			Assert.IsFalse(resources.Assets.Single(x => x.Value.Content.Contains("js2client")).Value.IsAdmin);

			Console.WriteLine(resources.Assets.Single(x => x.Value.Content.Contains("js2admin")).Value.FileName);
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
			Assert.IsTrue(resources.RenderHtmlTemplates().Contains("<span>") );
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
			resources.Include("coffee1.coffee");
			resources.Include("ts1.ts");

			Assert.IsTrue(resources.Assets.Single(x => x.Value.AssetTypeExtended == AssetTypeExtened.JavaScript).Value.IsScript);
			Assert.IsTrue(resources.Assets.Single(x => x.Value.AssetTypeExtended == AssetTypeExtened.JavaScript).Value.Path.Contains("js1.js"));

			Assert.IsTrue(resources.Assets.Single(x => x.Value.AssetTypeExtended == AssetTypeExtened.CoffeeScript).Value.IsScript);
			Assert.IsTrue(resources.Assets.Single(x => x.Value.AssetTypeExtended == AssetTypeExtened.CoffeeScript).Value.Path.Contains("coffee1.coffee"));

			Assert.IsTrue(resources.Assets.Single(x => x.Value.AssetTypeExtended == AssetTypeExtened.TypeScript).Value.IsScript);
			Assert.IsTrue(resources.Assets.Single(x => x.Value.AssetTypeExtended == AssetTypeExtened.TypeScript).Value.Path.Contains("ts1.ts"));
		}

		[Test]
		public void CheckAssetType_HtmlTemplate_test()
		{
			var resources = CreateDefaultResourceManagerFilesMocked();
			resources.Include("template1.thtml");
			resources.Include("templatexxx");

			Assert.IsTrue(resources.Assets.Single(x => x.Value.AssetTypeExtended == AssetTypeExtened.HtmlTemplate).Value.IsHtmlTemplate);
			Assert.IsTrue(resources.Assets.Single(x => x.Value.AssetTypeExtended == AssetTypeExtened.HtmlTemplate).Value.Path.Contains("template1.thtml"));

			Assert.IsTrue(resources.Assets.Single(x => x.Value.AssetTypeExtended == AssetTypeExtened.Unknown).Value.Path.Contains("templatexxx"));
		}

	}
}
