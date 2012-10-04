
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

		public IResourceManager CreateDefaultResourceManager()
		{
			var app = CreateDefaultApp();
			return UserResourceManager.Create(app.Id, HttpApplicationInfoObject(), FileSystemWrapperObject());
		}
		
		[TestFixtureSetUp]
		public void Setup()
		{
			RootDir = Path.GetFullPath("../../Content" );
		}

		[TestFixtureTearDown]
		public void Cleanup()
		{
			if (Directory.Exists(RootDir))
			{
				Directory.Delete(RootDir, true);
			}
		}


		[Test]
		public void CreateAndCheckIfExists_test()
		{
			var app = CreateDefaultApp();
			var res = UserResourceManager.Create(app.Id, HttpApplicationInfoObject(), new FileSystemWrapper());
			Assert.IsTrue(res.Exist(app.Id));
			Console.WriteLine(RootDir);
			Cleanup();
		}

		[Test]
		public void TryCreateWithSameName_test()
		{
			var app = CreateDefaultApp();
			var res = UserResourceManager.Create(app.Id, HttpApplicationInfoObject(), FileSystemWrapperObject());
//			Assert.Throws<Exception>(x =>  UserResourceManager.Create(app.Id, HttpApplicationInfoObject(), FileSystemWrapperObject()));
			Assert.IsTrue(false);
			Cleanup();
		}

		[Test]
		public void GetApp_test()
		{
			var app = CreateDefaultApp();
			var res = UserResourceManager.Get(app.Id, HttpApplicationInfoObject(), FileSystemWrapperObject());
			Assert.IsTrue(res.Exist(app.Id));
			Cleanup();
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

	
	}
}
