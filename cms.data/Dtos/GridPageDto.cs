using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using cms.data.Shared.Models;

namespace cms.data.Dtos
{
	public class GridPageDto
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public bool Home { get; set; }

		//TODO: tady by melo byt ResourceDto
		public ResourceDtoLoc ResourceDto { get; set; }
		public IList<IEnumerable<GridElementDto>> Lines{get; set; }
	}

}