using System;
using System.Collections.Generic;
using BundleTransformer.Core.Assets;

namespace cms.Code.UserResources
{
	public class ResourceManager: IResourceManager
	{
		private Guid Id { get; set; }
		
		private ResourceManager(Guid id)
		{
			Id = id;
		}

		public static IResourceManager Get(Guid id)
		{
			return new ResourceManager(id);
		}

		public static IResourceManager Create(Guid id)
		{
			var r =  new ResourceManager(id);
			r.CreateNewApplication(id);
			return r;
		}

		private void CreateNewApplication(Guid id)
		{

		}

		public bool Exist(Guid id)
		{
			return false;
		}

		public IList<IAsset> Assets()
		{
			return null;
		}

		public IApplicationResource Javascript(string type, ViewModeType mode = ViewModeType.Client)
		{
			return new ResourcesObject();
		}

		public IApplicationResource HtmlTemplate(string type, string skin, ViewModeType mode = ViewModeType.Client)
		{
			return new ResourcesObject();
		}
		
		public IApplicationResource HtmlTemplate(string type, ViewModeType mode = ViewModeType.Client)
		{
			return HtmlTemplate(type, "", mode);
		}

		public IList<IApplicationResource> GetAll()
		{
			return new List<IApplicationResource>();
		}


	}
}