using System;
using System.IO;
using BundleTransformer.Core.FileSystem;
using BundleTransformer.Core.Web;
using NUnit.Framework;
using cms.Code.UserResources;
using cms.web.tests.Code;

namespace cms.web.tests
{
	[TestFixture]
	class UserResourceManagerIntegration : UserResourceManagerTestBase
	{
		private string IntegrationRootDir { get; set; }

		public UserResourceManagerIntegration()
		{
			IntegrationRootDir = Path.GetFullPath("c:/lades/web/cms/cmd.web.tests/Integration");
		}

		IFileSystemWrapper FileSystemWrapperObject()
		{
			return new FileSystemWrapper();
		}

		IHttpApplicationInfo HttpApplicationInfoIntegratonObject()
		{
			return new HttpApplicationInfo(IntegrationRootDir, IntegrationRootDir);
		}

		public void Cleanup(Guid id )
		{
			var apppath = Path.Combine(IntegrationRootDir, id.ToString());
			if (Directory.Exists(apppath))
			{
				Directory.Delete(apppath, true);
			}
		}

		[Test]
		public void CreateAndCheckIfExists_test()
		{
			var appId = new Guid();
			var res = UserResourceManager.Create(appId, HttpApplicationInfoIntegratonObject(), FileSystemWrapperObject());
			Assert.IsTrue(res.Exist());
			Cleanup(appId);
		}

		[Test]
		public void TryCreateWithSameName_test()
		{
			var appId = new Guid();
			UserResourceManager.Create(appId, HttpApplicationInfoIntegratonObject(), FileSystemWrapperObject());
			Assert.Throws<Exception>(() => UserResourceManager.Create(appId, HttpApplicationInfoIntegratonObject(), FileSystemWrapperObject()));
			Cleanup(appId);
		}

		[Test]
		public void GetApp_ApplicationExists_test()
		{
			var appid = new Guid();
			UserResourceManager.Create(appid, HttpApplicationInfoIntegratonObject(), FileSystemWrapperObject());
			var res = UserResourceManager.Get(appid, HttpApplicationInfoIntegratonObject(), FileSystemWrapperObject());
			Assert.IsTrue(res.Exist());
			Cleanup(appid);
		}

		[Test]
		public void GetApp_ApplicationDoesntExists_test()
		{
			var appId = new Guid();
			Assert.Throws<DirectoryNotFoundException>(() => UserResourceManager.Get(appId, HttpApplicationInfoIntegratonObject(), FileSystemWrapperObject()));
		}

		[Test]
		public void IncludeDirectory_test()
		{
			var httpinfo = HttpApplicationInfoIntegratonObject();
			var fswrapper = FileSystemWrapperObject();

			Console.WriteLine(httpinfo.RootPath);
			var resources = UserResourceManager.Get(Id, httpinfo, fswrapper);
			resources.IncludeDirectory("js");
			Assert.AreEqual(3, resources.Assets.Count);
		}

	}
}