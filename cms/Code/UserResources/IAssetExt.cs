
using BundleTransformer.Core.Assets;

namespace cms.Code.UserResources
{

	public interface IAssetExt : IAsset
	{
		AssetTypeExtened AssetTypeExtended { get; }
		bool IsHtmlTemplate { get; }
		bool IsAdmin { get; }
		string FileName { get; }
	}
}