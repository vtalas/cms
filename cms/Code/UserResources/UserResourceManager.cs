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
		Guid Id { get; }
		IList<IAssetExt> Assets { get; }
		void IncludeDirectory(string path);
		void Include(string path);
	
		bool Exist();
		string RenderScripts();
		string RenderStyleSheets();
		string RenderHtmlTemplates();
	}

	public class UserResourceManager: IResourceManager
	{
		public Guid Id { get; private set; }
		private IList<IAssetExt> AssetsValues { get; set; }
		private string BaseDir { get; set; }

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
			BaseDir = Path.Combine( HttpAppInfo.RootPath, Id.ToString());
		}

		public static IResourceManager Get(Guid id)
		{
			return new UserResourceManager(id, BundleTransformerContext.Current.GetApplicationInfo(), BundleTransformerContext.Current.GetFileSystemWrapper());
		}

		public static IResourceManager Get(Guid id, IHttpApplicationInfo httpApp, IFileSystemWrapper fileSystemWrapper)
		{
			var resourceobj = new UserResourceManager(id, httpApp, fileSystemWrapper);
			if (!resourceobj.Exist())
			{
				throw new DirectoryNotFoundException();
			}
			return resourceobj;
		}

		public static IResourceManager Create(Guid id, IHttpApplicationInfo httpApp, IFileSystemWrapper fileSystemWrapper)
		{
			var r = new UserResourceManager(id, httpApp, fileSystemWrapper);
			r.CreateNewApplication();
			return r;
		}

		public static IResourceManager Create(Guid id)
		{
			return Create(id, BundleTransformerContext.Current.GetApplicationInfo(), BundleTransformerContext.Current.GetFileSystemWrapper());
		}
		
		public bool Exist()
		{
			var appResourcesPath = Path.Combine(HttpAppInfo.RootPath, Id.ToString());
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
			var fullPath = Path.Combine(HttpAppInfo.RootPath, Id.ToString(), path);
			if (!FileSystemWrapper.DirectoryExists(fullPath))
			{
				return;
			}

			var files = Directory.GetFiles(fullPath, "*.*", SearchOption.AllDirectories);
			foreach (var file in files)
			{
				Include(file);
			}
		}

		public void Include(string path)
		{
			var asset = new AssetDecorator(new Asset(Path.Combine(HttpAppInfo.RootPath, Id.ToString(), path), HttpAppInfo, FileSystemWrapper));
			AssetsValues.Add(asset);
		}

		public string RenderScripts()
		{
			var contentToRender = Assets.Where(x => x.IsScript).ToList();
			var s = new StringBuilder();
			foreach (var asset in contentToRender)
			{
				switch (asset.AssetTypeExtended)
				{
					case AssetTypeExtened.TypeScript:
					case AssetTypeExtened.JavaScript:
						s.Append(asset.Content);
						break;
					case AssetTypeExtened.CoffeeScript:
						var x = new BundleTransformer.CoffeeScript.Translators.CoffeeScriptTranslator();
						s.Append(x.Translate(asset));
						break;
				}
				
			}
			return s.ToString();
		}

		public string RenderStyleSheets()
		{
			var contentToRender = Assets.Where(x => x.IsStylesheet).ToList();
			var s = new StringBuilder();
			foreach (var asset in contentToRender)
			{
				switch (asset.AssetTypeExtended)
				{
					case AssetTypeExtened.Css:
						s.Append(asset.Content);
						break;
					case AssetTypeExtened.Sass:
					case AssetTypeExtened.Scss:
					case AssetTypeExtened.Less:
						var x = new BundleTransformer.Less.Translators.LessTranslator();
						s.Append(x.Translate(asset));
						break;
				}

			}
			return s.ToString();
		}

		public string RenderHtmlTemplates()
		{
			var contentToRender = Assets.Where(x => x.IsHtmlTemplate).ToList();
			var s = new StringBuilder();
			foreach (var asset in contentToRender)
			{
				switch (asset.AssetTypeExtended)
				{
					case AssetTypeExtened.HtmlTemplate:
						s.Append(asset.Content);
						break;
				}
			}
			return s.ToString();
		}
	}


	
}