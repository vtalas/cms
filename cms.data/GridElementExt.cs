using System.Collections.Generic;
using System.Linq;
using cms.data.Models;

namespace cms.data
{
	public static class GridElementExt
	{

		public static GridPage ToGridPage(this Grid source)
		{
			return new GridPage
			{
				Lines = source.GridElements.ToLines(),
				Home = source.Home,
				Id = source.Id,
				Link = source.Link,
				Name = source.Name
			};

		}
		private static IEnumerable<GridElement> DefaultLine(int line)
		{
			var d = new List<GridElement>();
			for (int i = 0; i < 12; i++)
			{
				d.Add(new GridElement { Line = line, Width = 1, Position = i });
			}
			return d;
		}
		
		private static IEnumerable<GridElement> ToFixedLine(this IEnumerable<GridElement> source, int line)
		{
			var d = new List<GridElement>();
			var position = source.GetEnumerator();
			position.MoveNext();
			for (int i = 0; i < 12; i++)
			{
				var cur = position.Current;
				if (cur != null && cur.Position == i)
				{
					d.Add(cur);
					i += cur.Width - 1;
					position.MoveNext();
				}
				else
				{
					d.Add(new GridElement { Content = "nnn", Line = line, Grid = null, Position = i, 
					                        //Status = Status.Private, 
					                        Type = "", Width = 1 });
				}
			}
			return d;
		}

		public static IList<IEnumerable<GridElement>> ToLines(this IEnumerable<GridElement> source)
		{
			var a = new List<IEnumerable<GridElement>>();
			if (source == null || !source.Any())
			{
				a.Add(DefaultLine(0));
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