using System;
using System.Collections.Generic;
using System.Text;
using BundleTransformer.Core;
using BundleTransformer.Core.Assets;
using BundleTransformer.Core.FileSystem;
using BundleTransformer.Core.Web;

namespace cms.Code.UserResources
{
	public interface IResourceManager
	{
		IList<IAsset> Assets { get; }
		void IncludeDirectory(string path);
		void Include(string path);
		string Combine();
		bool Exist(Guid id);
	}

	public class UserResourceManager: IResourceManager
	{
		private Guid Id { get; set; }
		private IList<IAsset> AssetsValues { get; set; }
		public IFileSystemWrapper AssetsFilesystem { get; private set; }
		public IList<IAsset> Assets
		{
			get { return AssetsValues; }
		}
		public IHttpApplicationInfo HttpAppInfo { get; private set; }
		public IFileSystemWrapper FileSystemWrapper { get; private set; }

		private UserResourceManager(Guid id, IHttpApplicationInfo httpApp, IFileSystemWrapper fileSystemWrapper)
		{
			HttpAppInfo = httpApp;
			FileSystemWrapper = fileSystemWrapper;
			Id = id;
			AssetsValues = new List<IAsset>();
		}

		public static IResourceManager Get(Guid id)
		{
			return new UserResourceManager(id, BundleTransformerContext.Current.GetApplicationInfo(), (IFileSystemWrapper)BundleTransformerContext.Current.GetFileSystemWrapper());
		}

		public static IResourceManager Get(Guid id, IHttpApplicationInfo httpApp, IFileSystemWrapper fileSystemWrapper)
		{
			return new UserResourceManager(id, httpApp, fileSystemWrapper);
		}

		public static IResourceManager Create(Guid id, IHttpApplicationInfo httpApp, IFileSystemWrapper fileSystemWrapper)
		{
			var r =  new UserResourceManager(id, httpApp, fileSystemWrapper);
			r.CreateNewApplication(id);
			return r;
		}

		public static IResourceManager Create(Guid id)
		{
			return Create(id, BundleTransformerContext.Current.GetApplicationInfo(), (IFileSystemWrapper)BundleTransformerContext.Current.GetFileSystemWrapper());
		}
		
		public bool Exist(Guid id)
		{
			return false;
		}

		private void CreateNewApplication(Guid id)
		{

		}

		public void IncludeDirectory(string path)
		{
			throw new NotImplementedException();
		}

		public void Include(string path)
		{
			AssetsValues.Add(new Asset(path, HttpAppInfo, FileSystemWrapper));
		}

		public string Combine()
		{
			var s = new StringBuilder();
			foreach (var asset in AssetsValues)
			{
				switch (asset.AssetType)
				{
					case AssetType.JavaScript :
						s.Append(asset.Content);
						break;
					case AssetType.CoffeeScript :
						//TODO: udelej translaci
						break;
				}
			}
			return s.ToString();
		}
	}


	
}