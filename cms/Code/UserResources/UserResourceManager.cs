using System;
using System.Collections.Generic;
using System.IO;
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
		private string BaseDir { get; set; }

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
			BaseDir = Path.Combine( HttpAppInfo.RootPath, Id.ToString());
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
			var r = new UserResourceManager(id, httpApp, fileSystemWrapper);
			r.CreateNewApplication();
			return r;
		}

		public static IResourceManager Create(Guid id)
		{
			return Create(id, BundleTransformerContext.Current.GetApplicationInfo(), (IFileSystemWrapper)BundleTransformerContext.Current.GetFileSystemWrapper());
		}
		
		public bool Exist(Guid id)
		{
			var appResourcesPath = Path.Combine(HttpAppInfo.RootPath, id.ToString());
			return FileSystemWrapper.DirectoryExists(appResourcesPath);
		}

		private void CreateNewApplication()
		{
			if (FileSystemWrapper.DirectoryExists(BaseDir))
			{
				throw new Exception("directory exists");
			}
			FileSystemWrapper.CreateDirectory(BaseDir);
		}

		public void IncludeDirectory(string path)
		{
			throw new NotImplementedException();
		}

		public void Include(string path)
		{
			var asset = new Asset(Path.Combine(BaseDir, path), HttpAppInfo, FileSystemWrapper);
			AssetsValues.Add(asset);
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
						//translator udelat singleton 
						var x = new BundleTransformer.CoffeeScript.Translators.CoffeeScriptTranslator();
						s.Append(x.Translate(asset));
						break;
				}
			}
			return s.ToString();
		}
	}


	
}