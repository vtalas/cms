using System.Collections.Generic;
using cms.data.EF.DataProvider;
using cms.data.Shared.Models;
using cms.shared;

namespace cms.data.tests._Common
{
	public static class GridHelper
	{
		private static readonly string CurrentCulture = SharedLayer.Culture;

		public static Resource CheckResource(this Grid item, string key)
		{
			return item.CheckResource(key, CurrentCulture);
		}

		public static Grid WithResource(this Grid item, string key, string value, string culture = "cs", int id = 0)
		{
			item.Resources.Add(JsonDataEfHelpers.GetResource(key, value, item.Id, culture, id));
			return item;
		}

		public static Grid WithResource(this Grid item, IList<Resource> allResources, string key, string value,
		                                string culture = "cs", int id = 0)
		{
			var newitem = JsonDataEfHelpers.GetResource(key, value, item.Id, culture, id);
			item.Resources.Add(newitem);
			allResources.Add(newitem);
			return item;
		}
	
	}
}