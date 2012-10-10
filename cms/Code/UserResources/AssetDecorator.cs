using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using BundleTransformer.CoffeeScript.Translators;
using BundleTransformer.Core.Assets;
using BundleTransformer.Less.Translators;

namespace cms.Code.UserResources
{
	public class AssetDecorator : IAssetExt
	{
		string _content;
		private static readonly Lazy<LessTranslator> LessTranslatorObject = new Lazy<LessTranslator>(() => new LessTranslator());
		private static readonly Lazy<CoffeeScriptTranslator> CoffeeTranslatorObject = new Lazy<CoffeeScriptTranslator>(() => new CoffeeScriptTranslator());

		Asset AssetObject { get; set; }
		public AssetDecorator(Asset asset)
		{
			AssetObject = asset;
			FileName = System.IO.Path.GetFileName(asset.Path.ToLower());
			AssetTypeExtended = GetAssetType();
			IsAdmin = FileName.Contains("_admin.");
		}

		private AssetTypeExtened GetAssetType()
		{
			var extension = System.IO.Path.GetExtension(FileName);
			switch (extension)
			{
				case ".coffee":
					return AssetTypeExtened.CoffeeScript;
				case ".js":
					return AssetTypeExtened.JavaScript;
				case ".ts":
					return AssetTypeExtened.TypeScript;
				case ".css":
					return AssetTypeExtened.Css;
				case ".thtml":
					return AssetTypeExtened.HtmlTemplate;
				case ".scss":
					return AssetTypeExtened.Scss;
				case ".sass":
					return AssetTypeExtened.Sass;
				case ".less":
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
		public string Content
		{
			get { return _content ?? (_content = RenderAsset()); }
			set
			{
				AssetObject.Content = value;
			}
		}
		public bool IsStylesheet { get { return AssetObject.IsStylesheet; } }
		public bool IsScript { get { return AssetObject.IsScript; } }

		public bool IsHtmlTemplate { get { return AssetTypeExtended == AssetTypeExtened.HtmlTemplate; } }
		public bool IsAdmin { get; private set; }
		public string FileName { get; private set; }
		public AssetTypeExtened AssetTypeExtended { get; private set; }
		
		private string html()
		{
			var content = AssetObject.Content.Replace(Environment.NewLine, "");
			return string.Format("<script id=\"{0}\" type=\"text/ng-template\" >{1}</script>", FileName, content );
		}
		private string RenderAsset()
		{
			switch (AssetTypeExtended)
			{
				case AssetTypeExtened.TypeScript:
				case AssetTypeExtened.JavaScript:
				case AssetTypeExtened.Css:
					return AssetObject.Content;
				case AssetTypeExtened.CoffeeScript:
					return CoffeeTranslatorObject.Value.Translate(AssetObject).Content;
				case AssetTypeExtened.HtmlTemplate:
					return html();
				case AssetTypeExtened.Sass:
				case AssetTypeExtened.Scss:
				case AssetTypeExtened.Less:
					return LessTranslatorObject.Value.Translate(AssetObject).Content;
			}
			return "";
		}

	}
}