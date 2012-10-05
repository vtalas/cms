using System;
using System.Collections.Generic;
using BundleTransformer.Core.Assets;

namespace cms.Code.UserResources
{

	public interface IAssetExt : IAsset
	{
		AssetTypeExtened AssetTypeExtended { get; }
		bool IsHtmlTemplate { get; }
	}

	public enum AssetTypeExtened
	{
		Unknown,
		Css,
		JavaScript,
		Less,
		Sass,
		Scss,
		CoffeeScript,
		HtmlTemplate,
		TypeScript
	}

	public class AssetDecorator : IAssetExt
	{
		Asset AssetObject { get; set; }
		public AssetDecorator(Asset asset)
		{
			AssetObject = asset;
			AssetTypeExtended = GetAssetType(asset.Path);
		}

		private AssetTypeExtened GetAssetType(string path)
		{
			var extension = System.IO.Path.GetExtension(path.ToLower());
			switch (extension)
			{
				case ".coffee" : 
					return AssetTypeExtened.CoffeeScript;
				case ".js" : 
					return AssetTypeExtened.JavaScript;
				case ".ts" : 
					return AssetTypeExtened.TypeScript;
				case ".css" : 
					return AssetTypeExtened.Css;
				case ".thtml" : 
					return AssetTypeExtened.HtmlTemplate;
				case ".scss": 
					return AssetTypeExtened.Scss;
				case ".sass" : 
					return AssetTypeExtened.Sass;
				case ".less" : 
					return AssetTypeExtened.Less;
			}
			return AssetTypeExtened.Unknown;
			
		}

		public void RefreshContent()
		{
			AssetObject.RefreshContent();
		}
		public string Path { get { return AssetObject.Path; } set { AssetObject.Path = value; } }
		public IList<string> RequiredFilePaths { get { return AssetObject.RequiredFilePaths; } set { AssetObject.RequiredFilePaths = value; } }
		public string Url { get { return AssetObject.Url; } }
		public AssetType AssetType { get { return AssetObject.AssetType; } }
		public bool Minified { get { return AssetObject.Minified; } set { AssetObject.Minified = value; } }
		public DateTime LastModifyDateTimeUtc { get { return AssetObject.LastModifyDateTimeUtc; } }
		public string Content { get { return AssetObject.Content; } set { AssetObject.Content = value; } }
		public bool IsStylesheet { get { return AssetObject.IsStylesheet;  } }
		public bool IsScript { get { return AssetObject.IsScript; }  }

		public bool IsHtmlTemplate { get { return AssetTypeExtended == AssetTypeExtened.HtmlTemplate; } }
		public AssetTypeExtened AssetTypeExtended { get; private set; }
	}
}