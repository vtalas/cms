using System.Collections.Generic;
using System.Linq;
using cms.data.Dtos;
using cms.data.Shared.Models;

namespace cms.data
{
	public static class GridElementExt
	{
		private static int _minsize = 12;
		public static GridPageDto ToGridPageDto(this Grid source)
		{
			return new GridPageDto
			{
				Lines = source.GridElements.ToLines(),
				Home = source.Home,
				Id = source.Id,
				Link = source.Link,
				Name = source.Name
			};

		}
		private static IEnumerable<GridElementDto> DefaultLine(int line)
		{
			var d = new List<GridElementDto>();
			for (int i = 0; i < 12; i+=_minsize)
			{
				d.Add(new GridElementDto { Line = line, Width = _minsize, Position = i });
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
						Content = "nnn",
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
			bool any = source.Any();
			if (source == null || !any)
			{
				//a.Add(DefaultLine(0));
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
					a.Add(DefaultLine(i));
				}

			}
			return a;
		}
	}
}