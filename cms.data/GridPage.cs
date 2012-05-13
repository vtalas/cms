using System.Collections.Generic;
using cms.data.Models;

namespace cms.data
{
	public class GridPage
	{
		public GridPage()
		{
			var lines = new List<GridElement>();
			Lines = new List<IEnumerable<GridElement>> {lines};
		}
		public int Id { get; set; }
		public string Name { get; set; }
		public string Link { get; set; }
		public bool Home { get; set; }
		public IList<IEnumerable<GridElement>> Lines { get; set; }
	}
}