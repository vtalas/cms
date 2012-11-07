
using System;
using System.ComponentModel;
using System.IO;
using BundleTransformer.Core.Assets;
using BundleTransformer.Core.FileSystem;
using BundleTransformer.Core.Web;
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
		private static readonly Guid Id = new Guid("c78ee05e-1115-480b-9ab7-a3ab3c0f6643");
		private string RootDir { get; set; }
		private string IntegrationRootDir { get; set; }

		public CreatingNewApplicationResources()
		{
			RootDir = Path.GetFullPath("../../Content");
			IntegrationRootDir = Path.GetFullPath("../../Integration");
		}

		readonly ApplicationSetting _defaultApp = new ApplicationSetting
		{
			Name = "prdel",
			DefaultLanguage = "cs",
			Id = Id
		};

		public ApplicationSetting CreateDefaultApp()
		{
		
			var mock = new Mock<JsonDataProvider>("00000000-0000-0000-0000-000000000000");
			mock.Setup(x => x.Add(It.IsAny<ApplicationSetting>()))
				.Returns(_defaultApp);
			var db = mock.Object;

			return db.Add(new ApplicationSetting());
		}

		IHttpApplicationInfo HttpApplicationInfoObject()
		{
			return new HttpApplicationInfo(RootDir, RootDir);
		}
		IHttpApplicationInfo HttpApplicationInfoIntegratonObject()
		{
			return new HttpApplicationInfo(IntegrationRootDir, IntegrationRootDir);
		}

		IFileSystemWrapper FileSystemWrapperObject()
		{
			return new FileSystemWrapper();
		}

		IFileSystemWrapper FileSystemWrapperMock()
		{
			var mock = new Mock<IFileSystemWrapper>();
			mock.Setup(x => x.FileExists(It.IsAny<string>())).Returns(true);

			mock.Setup(x => x.GetFileTextContent(Path.Combine(RootDir, Id.ToString(), "js1.js"))).Returns("alert('kabdkjsa js1 1111111')");
			mock.Setup(x => x.GetFileTextContent(Path.Combine(RootDir, Id.ToString(), "js2.js"))).Returns("alert('kabdkjsa js2 222222')");
			mock.Setup(x => x.GetFileTextContent(Path.Combine(RootDir, Id.ToString(), "t1.thtml"))).Returns("<span>template1</span>");
			mock.Setup(x => x.GetFileTextContent(Path.Combine(RootDir, Id.ToString(), "css1.css"))).Returns("body{color:red;}");
			
			//mock.Setup(x => x.DirectoryExists(Path.Combine(RootDir, Id.ToString()))).Returns(true);
			return mock.Object;
		}

		public IResourceManager CreateDefaultResourceManagerFilesMocked()
		{
			var app = CreateDefaultApp();
			return UserResourceManager.Create(app.Id, HttpApplicationInfoObject(), FileSystemWrapperMock());
		}
		
		[TestFixtureSetUp]
		public void Setup()
		{
			var apppath = Path.Combine(RootDir, Id.ToString());
			if (Directory.Exists(apppath))
			{
				Directory.Delete(apppath, true);
			}
		}

		public void Cleanup()
		{
			var apppath = Path.Combine(RootDir, Id.ToString());
			if (Directory.Exists(apppath))
			{
				Directory.Delete(apppath, true);
			}
		}

		[Test]
		public void CreateAndCheckIfExists_test()
		{
			var app = CreateDefaultApp();
			var res = UserResourceManager.Create(app.Id, HttpApplicationInfoObject(), FileSystemWrapperObject());
			Assert.IsTrue(res.Exist());
			Cleanup();
		}


		[Test]
		public void TryCreateWithSameName_test()
		{
			var app = CreateDefaultApp();
			UserResourceManager.Create(app.Id, HttpApplicationInfoObject(), FileSystemWrapperObject());
			Assert.Throws<Exception>(() =>  UserResourceManager.Create(app.Id, HttpApplicationInfoObject(), FileSystemWrapperObject()));
			Cleanup();
		}


		[Test]
		public void GetApp_ApplicationExists_test()
		{
			var app = CreateDefaultApp();
			UserResourceManager.Create(app.Id, HttpApplicationInfoObject(), FileSystemWrapperObject());
			var res = UserResourceManager.Get(app.Id, HttpApplicationInfoObject(), FileSystemWrapperObject());
			Assert.IsTrue(res.Exist());
			Cleanup();
		}

		[Test]
		public void GetApp_ApplicationDoesntExists_test()
		{
			var app = CreateDefaultApp();
			Assert.Throws<DirectoryNotFoundException>(() => UserResourceManager.Get(app.Id, HttpApplicationInfoObject(), FileSystemWrapperObject()));
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
			Assert.IsTrue(resources.Assets[0].Content.Length > 0);
			Console.WriteLine(resources.Assets[0].Content );
		}


		[Test]
		public void IncludeDirectory_test()
		{
			var resources = UserResourceManager.Get(Id, HttpApplicationInfoIntegratonObject(), FileSystemWrapperObject());
			resources.IncludeDirectory("js");
			Assert.AreEqual(3, resources.Assets.Count);
		}

		[Test]
		public void Render_test()
		{
			var resources = CreateDefaultResourceManagerFilesMocked();
			resources.Include("js1.js");
			resources.Include("js2.js");
			resources.Include("t1.thtml");
			resources.Include("css1.css");
			
			Assert.AreEqual(resources.Assets.Where(x=>x.IsScript).Sum(x => x.Content.Length),  resources.RenderScripts().Length );
			Assert.IsTrue(resources.RenderHtmlTemplates().Contains("<span>") );
			Assert.AreEqual(resources.Assets.Where(x=>x.IsStylesheet).Sum(x => x.Content.Length),  resources.RenderStyleSheets().Length );
		
		}

		[Test]
		public void CheckAssetType_Script_test()
		{
			var resources = CreateDefaultResourceManagerFilesMocked();
			resources.Include("js1.js");
			resources.Include("coffee1.coffee");
			resources.Include("ts1.ts");

			Assert.IsTrue(resources.Assets.Single(x=>x.AssetTypeExtended == AssetTypeExtened.JavaScript).IsScript);
			Assert.IsTrue(resources.Assets.Single(x=>x.AssetTypeExtended == AssetTypeExtened.JavaScript).Path.Contains("js1.js") );

			Assert.IsTrue(resources.Assets.Single(x=>x.AssetTypeExtended == AssetTypeExtened.CoffeeScript).IsScript);
			Assert.IsTrue(resources.Assets.Single(x=>x.AssetTypeExtended == AssetTypeExtened.CoffeeScript).Path.Contains("coffee1.coffee") );

			Assert.IsTrue(resources.Assets.Single(x=>x.AssetTypeExtended == AssetTypeExtened.TypeScript).IsScript);
			Assert.IsTrue(resources.Assets.Single(x=>x.AssetTypeExtended == AssetTypeExtened.TypeScript).Path.Contains("ts1.ts") );
		}

		[Test]
		public void CheckAssetType_HtmlTemplate_test()
		{
			var resources = CreateDefaultResourceManagerFilesMocked();
			resources.Include("template1.thtml");
			resources.Include("templatexxx");

			Assert.IsTrue(resources.Assets.Single(x=>x.AssetTypeExtended == AssetTypeExtened.HtmlTemplate).IsHtmlTemplate);
			Assert.IsTrue(resources.Assets.Single(x => x.AssetTypeExtended == AssetTypeExtened.HtmlTemplate).Path.Contains("template1.thtml"));

			Assert.IsTrue(resources.Assets.Single(x => x.AssetTypeExtended == AssetTypeExtened.Unknown).Path.Contains("templatexxx"));
		}
	
	}
}
