using System;
using System.Collections.Generic;

namespace cms.data.Dtos
{
	public class GridPageDto
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public bool Home { get; set; }
		public string Category { get; set; }
		public string Link { get; set; }
		public IEnumerable<GridElementDto> GridElements{get; set; }
		public bool Authorize { get; set; }
	}

}