using System;
using System.IO;
using System.Web;
using BundleTransformer.Core;
using BundleTransformer.Core.FileSystem;
using BundleTransformer.Core.Web;

namespace cms.Code.UserResources
{
	public class UserResourcesManagerProvider
	{
		public static IResourceManager GetApplication(Guid applicationId)
		{
			var applicationInfo = new HttpApplicationInfo(VirtualPathUtility.ToAbsolute("~/"),
			                                              Path.Combine(HttpContext.Current.Server.MapPath("~/"), "App_Data\\ApplicationData"));

			var files = (IFileSystemWrapper)BundleTransformerContext.Current.GetFileSystemWrapper();
			var resources = UserResourceManager.Get(applicationId, applicationInfo, files);
			return resources;
		}

		public static IResourceManager CreateApplication(Guid applicationId)
		{
			var resources = UserResourceManager.Create(applicationId, GetApplicationInfo(), GetFilesystemWrapper());
			return resources;
		}

		private static IFileSystemWrapper GetFilesystemWrapper()
		{
			return (IFileSystemWrapper)BundleTransformerContext.Current.GetFileSystemWrapper();
		}

		private static IHttpApplicationInfo GetApplicationInfo()
		{
			return new HttpApplicationInfo(VirtualPathUtility.ToAbsolute("~/"),
			                               Path.Combine(HttpContext.Current.Server.MapPath("~/"), "App_Data\\ApplicationData"));
		}

	}
}