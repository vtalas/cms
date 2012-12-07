using System;
using System.Collections.Generic;
using cms.data.Shared.Models;
using cms.shared;

namespace cms.data.Extensions
{
	public static class GridExtensions
	{
		public static Grid WithResource(this Grid source, string key, string value, int id = 0)
		{
			var cultureUpadated = JsonDataEfHelpers.CorrectCulture(key, SharedLayer.Culture);
			return source.WithResource(key, value, cultureUpadated, id);
		}
		public static Grid WithResource(this Grid source, string key, string value,string culture, int id = 0)
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

		public static Grid CreateGrid(Guid id, ApplicationSetting application)
		{
			var grid = new Grid
				{
					Id = id,
					ApplicationSettings = application,
				};
			return grid;
		}

	}
}