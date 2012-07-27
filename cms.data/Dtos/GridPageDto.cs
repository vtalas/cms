using System.Collections.Generic;
using System.Runtime.Serialization;
using cms.data.Shared.Models;

namespace cms.data.Dtos
{
	public class GridPageDto
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public bool Home { get; set; }

		//TODO: tady by melo byt ResourceDto
		public ResourceDto Resource { get; set; }
		public IList<IEnumerable<GridElementDto>> Lines{get; set; }
	}

}