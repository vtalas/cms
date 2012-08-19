using System;
using System.Collections.Generic;

namespace cms.data.Dtos
{
	public class GridElementDto 
	{
		public Guid Id { get; set; }
		public int Line { get; set; }
		public int Width { get; set; }
		public int Position { get; set; }
		public string Content { get; set; }
		public string Type { get; set; }
		public string Skin { get; set; }
		public IEnumerable<ResourceDto> Resources { get; set; }
		public IDictionary<string, ResourceDtoLoc> ResourcesLoc { get; set; }
		public IDictionary<string, string> Res { get; set; }
	}

}