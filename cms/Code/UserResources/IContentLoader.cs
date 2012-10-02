using System;
using BundleTransformer.Core.Assets;

namespace cms.Code.UserResources
{
	public interface IContentLoader
	{
		Asset GetAsset(string path);
	}

	public class FileSystemContentLoader : IContentLoader
	{
		public Asset GetAsset(string path)
		{
			throw new NotImplementedException();
		}
	}


}