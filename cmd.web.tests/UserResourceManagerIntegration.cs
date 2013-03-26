using System;
using System.IO;
using System.Runtime.InteropServices;
using BundleTransformer.Core.FileSystem;
using BundleTransformer.Core.Web;
using Google.GData.Client;
using NUnit.Framework;
using Newtonsoft.Json;
using cms.Code.LinkAccounts;
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

		public void Cleanup(Guid id)
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
			var appId = Guid.NewGuid();
			var res = UserResourceManager.Create(appId, HttpApplicationInfoIntegratonObject(), FileSystemWrapperObject());
			Assert.IsTrue(res.Exist());
			Cleanup(appId);
		}

		[Test]
		public void TryCreateWithSameName_test()
		{
			var appId = Guid.NewGuid();
			UserResourceManager.Create(appId, HttpApplicationInfoIntegratonObject(), FileSystemWrapperObject());
			Assert.Throws<Exception>(() => UserResourceManager.Create(appId, HttpApplicationInfoIntegratonObject(), FileSystemWrapperObject()));
			Cleanup(appId);
		}

		[Test]
		public void GetApp_ApplicationExists_test()
		{
			var appid = Guid.NewGuid();
			UserResourceManager.Create(appid, HttpApplicationInfoIntegratonObject(), FileSystemWrapperObject());
			var res = UserResourceManager.Get(appid, HttpApplicationInfoIntegratonObject(), FileSystemWrapperObject());
			Assert.IsTrue(res.Exist());
			Cleanup(appid);
		}

		[Test]
		public void GetApp_ApplicationDoesntExists_test()
		{
			var appId = Guid.NewGuid();
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
			Assert.AreEqual(4, resources.Assets.Count);
		}

		[Test]
		public void SettingsStorage_test()
		{
			var appid = Guid.NewGuid();
			UserResourceManager.Create(appid, HttpApplicationInfoIntegratonObject(), FileSystemWrapperObject());
			var res = UserResourceManager.Get(appid, HttpApplicationInfoIntegratonObject(), FileSystemWrapperObject());
			res.SettingsStorage("xxx", "value");
			var r = res.SettingsStorage("xxx");
			Assert.AreEqual("value", r);
			Cleanup(appid);
		}

		[Test]
		public void SettingsStorage_json_test()
		{
			var appid = Guid.NewGuid();
			UserResourceManager.Create(appid, HttpApplicationInfoIntegratonObject(), FileSystemWrapperObject());
			var res = UserResourceManager.Get(appid, HttpApplicationInfoIntegratonObject(), FileSystemWrapperObject());

			var parameters = new OAuth2Parameters()
				{
					AccessCode = "AccessCode",
					AccessToken = "Access token",
					ClientId = "client id",
					ClientSecret = "Client secret",
					TokenExpiry = DateTime.Now
				};
			var aa = new aaa(parameters, res);


			var x = aa.ToJson();

			Console.WriteLine(x);


		}



	}
}