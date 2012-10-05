using System;
using System.Collections.Generic;
using System.Linq;
using cms.data.Shared.Models;

namespace cms.data.tests._Common
{
	public static class PrintHelpers
	{
		public static void PrintGridElements(this IEnumerable<GridElement> gridElements, Func<GridElement,string> print )
		{
			foreach (var element in gridElements)
			{
				Console.WriteLine(print(element));
			}
		}

		public static void PrintGridElementsPositons(this IEnumerable<GridElement> gridElements , Guid itemToHighlight, string message = "")
		{
			Func<GridElement, string> contenFormat =
				x => itemToHighlight == x.Id ? "[" + x.Position + "]" : x.Position.ToString();

			Console.WriteLine(message + gridElements.OrderBy(x => x.Position).Select(contenFormat)
				                            .Aggregate((x, y) => x + ", " + y));
		}

		public static string Serialize(this IEnumerable<GridElement> gridElements, string message = "")
		{
			return message + gridElements.OrderBy(x => x.Position).Select(x => x.Position.ToString()).Aggregate((x, y) => x + ", " + y);
		}
	}
}