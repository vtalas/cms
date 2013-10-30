using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace cms.data.Dtos
{
	public class GridElementDto 
	{
		public Guid Id { get; set; }
		public int Position { get; set; }

		public JObject Content { get; set; }
		public string Type { get; set; }
		public string Skin { get; set; }
	
		public string ParentId { get; set; }
		public IDictionary<string, ResourceDtoLoc> Resources { get; set; }

		public int Width { get; set; }
		public IList<GridElementGroupDto> Group { get; set; }
	}
}