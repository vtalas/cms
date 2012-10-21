using System;
using System.Collections.Generic;
using cms.data.Shared.Models;
using cms.shared;

namespace cms.data.Extensions
{
	public static class GridExt
	{

		public static Grid WithResource(this Grid source, string key, string value, int id = 0)
		{
			return source.WithResourceByInterface(key, value, id) as Grid;
		}

		public static GridElement WithResource(this GridElement source, string key, string value, int id = 0)
		{
			return source.WithResourceByInterface(key, value, id) as GridElement;
		}

		public static GridElement WithParent(this GridElement source, GridElement parent)
		{
			source.Parent = parent;
			return source;
		}

		public static GridElement WithResource(this GridElement source, string key, string value, string culture, int id = 0)
		{
			return source.WithResourceByInterface(key, value, culture, id) as GridElement;
		}

		public static Grid WithResource(this Grid source, string key, string value, string culture, int id = 0)
		{
			return source.WithResourceByInterface(key, value, culture, id) as Grid;
		}

		public static IEntityWithResource WithResourceByInterface(this IEntityWithResource source, string key, string value, int id = 0)
		{
			var cultureUpadated = JsonDataEfHelpers.CorrectCulture(key, SharedLayer.Culture);
			return source.WithResourceByInterface(key, value, cultureUpadated, id);
		}

		public static IEntityWithResource WithResourceByInterface(this IEntityWithResource source, string key, string value, string culture, int id = 0)
		{
			var res = JsonDataEfHelpers.GetResource(key, value, source.Id, culture, id);
			source.Resources.Add(res);
			return source;
		}

		public static Grid WithCategory(this Grid source, string category)
		{
			source.Category = category;
			return source;
		}

		public static Grid WithGridElements(this Grid source, List<GridElement> gridElements)
		{
			source.GridElements = gridElements;
			return source;
		}

		public static Grid WithGridElement(this Grid source, GridElement gridElement)
		{
			source.GridElements.Add(gridElement);
			return source;
		}



	}
}