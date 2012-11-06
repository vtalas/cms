using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using BundleTransformer.Core;
using BundleTransformer.Core.Assets;
using BundleTransformer.Core.FileSystem;
using BundleTransformer.Core.Web;
using System.Linq;

namespace cms.Code.UserResources
{
	public interface IResourceManager
	{
		IList<IAssetExt> Assets { get; }
		void IncludeDirectory(string path);
		void Include(string path);
	
		string Combine();
		string RenderScripts();
		string RenderStyleSheets();
		string RenderHtmlTemplates();

		bool Exist(Guid id);
	}

	public class UserResourceManager: IResourceManager
	{
		private Guid Id { get; set; }
		private IList<IAssetExt> AssetsValues { get; set; }
		public IFileSystemWrapper AssetsFilesystem { get; private set; }
		public IList<IAssetExt> Assets
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
			AssetsValues = new List<IAssetExt>();
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
			var appResourcesPath = Path.Combine(HttpAppInfo.RootPath, id.ToString());
			return FileSystemWrapper.DirectoryExists(appResourcesPath);
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
			var asset = new AssetExtended(Path.Combine(HttpAppInfo.RootPath, Id.ToString(), path), HttpAppInfo, FileSystemWrapper);
			AssetsValues.Add(asset);
		}

		public string Combine()
		{
		
			var s = new StringBuilder();
			foreach (var asset in AssetsValues)
			{
				switch (asset.AssetType)
				{
					case AssetTypeExtened.JavaScript :
						s.Append(asset.Content);
						break;
					case AssetTypeExtened.CoffeeScript:
						//translator udelat singleton 
						var x = new BundleTransformer.CoffeeScript.Translators.CoffeeScriptTranslator();
						s.Append(x.Translate(asset));
						break;
				}
			}
			return s.ToString();
		}

		public string RenderScripts()
		{
//			var contentToRender = Assets.Select()
			var s = new StringBuilder();
			return s.ToString();
		}

		public string RenderStyleSheets()
		{
			var s = new StringBuilder();
			return s.ToString();
		}
		public string RenderHtmlTemplates()
		{
			var s = new StringBuilder();
			return s.ToString();
		}
	}


	
}