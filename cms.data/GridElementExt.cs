using System.Collections.Generic;
using System.Linq;
using cms.data.Dtos;
using cms.data.DtosExtensions;
using cms.data.Shared.Models;

namespace cms.data
{
	public static class GridElementExt
	{
		public static IEnumerable<MenuItemDto> ToChildren(this IEnumerable<GridElement> source)
		{
			var rootlist = source.Where(x => x.Parent == null).Select(x=>x.ToMenuItemDto(source)).ToList();
			return rootlist;
		}

	}
}