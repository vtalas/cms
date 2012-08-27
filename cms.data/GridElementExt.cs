using System.Collections.Generic;
using System.Linq;
using cms.data.Dtos;
using cms.data.Shared.Models;

namespace cms.data
{
	public static class GridElementExt
	{
		private const int ElementMinWidth = 12;

		private static IEnumerable<GridElementDto> EmptyLine(int line)
		{
			var d = new List<GridElementDto>();
			for (int i = 0; i < 12; i+=ElementMinWidth)
			{
				d.Add(new GridElementDto { Line = line, Width = ElementMinWidth, Position = i });
			}
			return d;
		}

		private static IEnumerable<GridElementDto> ToFixedLine(this IEnumerable<GridElement> source, int line)
		{
			var d = new List<GridElementDto>();
			var position = source.GetEnumerator();
			position.MoveNext();
			for (int i = 0; i < 12; i++)
			{
				var cur = position.Current;
				if (cur != null && cur.Position == i)
				{
					d.Add(cur.ToDto());
					i += cur.Width - 1;
					position.MoveNext();
				}
				else
				{
					d.Add(new GridElementDto
					{
						Content = "new empty element",
						Line = line,
						Position = i, 
						//Status = Status.Private, 
						Type = "", Width = 1 });
				}
			}
			return d;
		}

		public static IList<IEnumerable<GridElementDto>> ToLines(this IEnumerable<GridElement> source)
		{
			var a = new List<IEnumerable<GridElementDto>>();
			if (source == null || !source.Any())
			{
				return a;
			}
						
			var c = source.Max(x => x.Line);

			for (int i = 0; i <= c; i++)
			{
				var xx = source.Where(x => x.Line == i);
				if (xx.Any())
				{
					a.Add(xx.ToFixedLine(i).ToList());
				}
				else
				{
					a.Add(EmptyLine(i));
				}

			}
			return a;
		}

		public static IEnumerable<MenuItemDto> ToChildren(this IEnumerable<GridElement> source)
		{
			var rootlist = source.Where(x => x.Parent == null);
			var a = new List<MenuItemDto>();
			var m = new MenuDto();

			foreach (var item in rootlist)
			{
				m.Children = getchildren(item, source);
				
			}
		
			return a;
		}

		private static IEnumerable<MenuItemDto> getchildren(GridElement item, IEnumerable<GridElement> source)
		{
			var children = new List<MenuItemDto>();
			if (source.Any(x => x.Parent.Id == item.Id))
			{
				children = source.Where(x => x.Parent.Id == item.Id);
				foreach (var item in children)
				{
					
				}
			}

		}
	}
}