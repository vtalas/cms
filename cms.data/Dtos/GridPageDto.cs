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
		public ResourceDtoLoc ResourceDto { get; set; }
		public IList<IEnumerable<GridElementDto>> Lines{get; set; }
	}

}