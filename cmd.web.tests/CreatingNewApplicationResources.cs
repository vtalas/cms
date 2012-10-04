
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

		public CreatingNewApplicationResources()
		{
			RootDir = Path.GetFullPath("../../Content");
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
			
			//mock.Setup(x => x.DirectoryExists(Path.Combine(RootDir, Id.ToString()))).Returns(true);
			return mock.Object;
		}

		public IResourceManager CreateDefaultResourceManager()
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
			var resources = CreateDefaultResourceManager();
			Assert.IsTrue(resources.Assets.Count == 0);
		}

		[Test]
		public void IncludeFile_test()
		{
			var resources = CreateDefaultResourceManager();
			resources.Include("js1.js");
			Assert.IsTrue(resources.Assets.Count == 1);
			Assert.IsTrue(resources.Assets[0].Content.Length > 0);
			Console.WriteLine(resources.Assets[0].Content );
		}

		[Test]
		public void IncludeDirectory_test()
		{
			var resources = CreateDefaultResourceManager();
			resources.IncludeDirectory("js");
			Assert.IsTrue(resources.Assets.Count == 3);
		}

		[Test]
		public void Combine_multiple_test()
		{
			var resources = CreateDefaultResourceManager();
			resources.Include("js1.js");
			resources.Include("js2.js");
			
			Assert.AreEqual(resources.Assets.Sum(x => x.Content.Length),  resources.Combine().Length );
		}

		[Test]
		public void CheckAssetType_Script_test()
		{
			var resources = CreateDefaultResourceManager();
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
			var resources = CreateDefaultResourceManager();
			resources.Include("template1.thtml");

			Assert.IsTrue(resources.Assets.Single(x=>x.AssetTypeExtended == AssetTypeExtened.HtmlTemplate).IsHtmlTemplate);
			Assert.IsTrue(resources.Assets.Single(x => x.AssetTypeExtended == AssetTypeExtened.HtmlTemplate).Path.Contains("template1.thtml"));
		}
	
	}
}
