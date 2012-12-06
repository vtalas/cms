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

	}
}