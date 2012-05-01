using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using cms.data.Models;

namespace cms.data.Files
{
	public class JsonDataFiles : JsonDataProvider
	{
		public JsonDataFiles(string application) : base(application)
		{
		}

		public override JObject Applications()
		{
			throw new System.NotImplementedException();
		}

		public override IEnumerable<Grid> Grids()
		{
			throw new System.NotImplementedException();
		}

		public override Grid Grid(int id)
		{
			var gridElems = new List<GridElement>()
			                	{
			                		new GridElement {Content = "aaa", Line = 0, Position = 0, Width = 3, Type = "text"},
			                		new GridElement {Content = "aaa", Line = 0, Position = 3, Width = 3, Type = "text"},
			                		new GridElement {Content = "aaa", Line = 0, Position = 6, Width = 3, Type = "text"},
			                		new GridElement {Content = "aaa", Line = 2, Position = 5, Width = 3, Type = "text"},
			                		new GridElement {Content = "aaa", Line = 3, Position = 0, Width = 12, Type = "text"}
			                	};

			
			var grid = new Grid()
			           	{

			           		GridElems = gridElems
			           	};

			return grid;
		}
	}
}