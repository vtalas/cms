using System;
using System.Collections.Generic;
using BundleTransformer.Core.Assets;

namespace cms.Code.UserResources
{
	public interface IResourceManager
	{
		bool Exist(Guid id);
		
		IList<IAsset> Assets();
		void IncludeDirectory(string path);


		IApplicationResource Javascript(string type, ViewModeType mode = ViewModeType.Client);
		IApplicationResource HtmlTemplate(string type, string skin, ViewModeType mode = ViewModeType.Client);
		IApplicationResource HtmlTemplate(string type, ViewModeType mode = ViewModeType.Client);
		IList<IApplicationResource> GetAll();
	
	}
}