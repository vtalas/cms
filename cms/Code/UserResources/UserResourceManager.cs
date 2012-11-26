using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using BundleTransformer.CoffeeScript.Translators;
using BundleTransformer.Core;
using BundleTransformer.Core.Assets;
using BundleTransformer.Core.FileSystem;
using BundleTransformer.Core.Translators;
using BundleTransformer.Core.Web;
using System.Linq;
using BundleTransformer.Less.Configuration;
using BundleTransformer.Less.Translators;

namespace cms.Code.UserResources
{
	public interface IResourceManager
	{
		Guid Id { get; }
		IDictionary<string, IAssetExt> Assets { get; }
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
		private IDictionary<string, IAssetExt> AssetsValues { get; set; }
		private string BaseDir { get; set; }

		public IFileSystemWrapper AssetsFilesystem { get; private set; }
		public IDictionary<string, IAssetExt> Assets
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
			AssetsValues = new Dictionary<string, IAssetExt>();
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
				throw new DirectoryNotFoundException(Path.Combine(httpApp.RootPath, id.ToString()));
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
			return FileSystemWrapper.DirectoryExists(BaseDir);
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
			var asset = new Asset(Path.Combine(HttpAppInfo.RootPath, Id.ToString(), path), HttpAppInfo, FileSystemWrapper);
			var assetDecorator = new AssetDecorator(asset);

			if (AssetsValues.ContainsKey(assetDecorator.FileName))
			{
				AssetsValues[assetDecorator.FileName] = assetDecorator;
			}
			else
			{
				AssetsValues.Add(assetDecorator.FileName, assetDecorator);
			}

		}

		private string Render(IEnumerable<KeyValuePair<string, IAssetExt>> contentToRender)
		{
			//ITranslator translatorInstance = BundleTransformerContext.Current.GetJsTranslatorInstance("CoffeeScriptTranslator");
			var s = new StringBuilder();
			foreach (var asset in contentToRender)
			{
				s.Append(asset.Value.Content);
			}
			return s.ToString();
		}

		public string RenderScripts()
		{
			var contentToRender = Assets.Where(x => x.Value.IsScript).ToList();
			return Render(contentToRender);
		}

		public string RenderStyleSheets()
		{
			var contentToRender = Assets.Where(x => x.Value.IsStylesheet).ToList();
			return Render(contentToRender);
		}

		public string RenderHtmlTemplates(Func<IAssetExt, bool> restriction )
		{
			return Render(Assets.Where(x => x.Value.IsHtmlTemplate && restriction(x.Value)).ToList());
		}

		public string RenderHtmlTemplates()
		{
			var contentToRender = Assets.Where(x => x.Value.IsHtmlTemplate).ToList();
			return Render(contentToRender);
		}
	}


	
}