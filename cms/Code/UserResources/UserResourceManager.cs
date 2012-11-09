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
		private static readonly Lazy<LessTranslator> LessTranslatorObject =new Lazy<LessTranslator>(() => new LessTranslator());
		private static readonly Lazy<CoffeeScriptTranslator> CoffeeTranslatorObject = new Lazy<CoffeeScriptTranslator>(() => new CoffeeScriptTranslator());


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
			if (AssetsValues.ContainsKey(asset.FileName))
			{
				AssetsValues[asset.FileName] = asset;
			}
			else
			{
				AssetsValues.Add(asset.FileName, asset);
			}

		}

		private string Render(IEnumerable<KeyValuePair<string, IAssetExt>> contentToRender)
		{
			//ITranslator translatorInstance = BundleTransformerContext.Current.GetJsTranslatorInstance("CoffeeScriptTranslator");
			var s = new StringBuilder();
			foreach (var asset in contentToRender)
			{
				switch (asset.Value.AssetTypeExtended)
				{
					case AssetTypeExtened.TypeScript:
					case AssetTypeExtened.JavaScript:
						s.Append(asset.Value.Content);
						break;
					case AssetTypeExtened.CoffeeScript:
						s.Append(CoffeeTranslatorObject.Value.Translate(asset.Value).Content);
						break;
					case AssetTypeExtened.Css:
						s.Append(asset.Value.Content);
						break;
					case AssetTypeExtened.Sass:
					case AssetTypeExtened.Scss:
					case AssetTypeExtened.Less:
						s.Append(LessTranslatorObject.Value.Translate(asset.Value).Content);
						break;
					case AssetTypeExtened.HtmlTemplate:
						s.Append(asset.Value.Content);
						break;
				}
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