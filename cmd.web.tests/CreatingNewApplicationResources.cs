
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
		readonly ApplicationSetting defaultApp = new ApplicationSetting
		{
			Name = "prdel",
			DefaultLanguage = "cs",
			Id = Guid.NewGuid()
		};

		IHttpApplicationInfo HttpApplicationInfoMock
		{
			get
			{
				var assetMock = new Mock<IHttpApplicationInfo>();
				assetMock.Setup(x => x.RootPath).Returns(Path.GetFullPath("../../"));
				return assetMock.Object;
			}
		}
		IFileSystemWrapper FileSystemWrapperMock
		{
			get
			{
				var filesystemWrapper = new Mock<IFileSystemWrapper>();
				filesystemWrapper.Setup(x => x.GetFileTextContent("js1.js")).Returns("console.log('hvshs js1. jsbjdsbjdb sjb')");
				filesystemWrapper.Setup(x => x.GetFileTextContent("js2.js")).Returns("console.log('hvshs js22222222. jsbjdsbjdb sjb')");
				filesystemWrapper.Setup(x => x.GetFileTextContent("js3.js")).Returns("console.log('hvshs js33333333. jsbjdsbjdb sjb')");
				filesystemWrapper.Setup(x => x.FileExists(It.IsAny<string>())).Returns(true);
				return filesystemWrapper.Object;
			}
		}

		public ApplicationSetting CreateDefaultApp()
		{
		
			var mock = new Mock<JsonDataProvider>("00000000-0000-0000-0000-000000000000");
			mock.Setup(x => x.Add(It.IsAny<ApplicationSetting>()))
				.Returns(defaultApp);
			var db = mock.Object;

			return db.Add(new ApplicationSetting());
		}

		public IResourceManager CreateDefaultResourceManager()
		{
			var app = CreateDefaultApp();
			return UserResourceManager.Create(app.Id, HttpApplicationInfoMock, FileSystemWrapperMock);
		}

		[TestFixtureSetUp]
		public void Setup()
		{
		}


		[Test]
		public void CreateAndCheckIfExists_test()
		{
			var app = CreateDefaultApp();
			var res = UserResourceManager.Get(app.Id, HttpApplicationInfoMock, FileSystemWrapperMock);
			Assert.IsTrue(res.Exist(app.Id));
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
