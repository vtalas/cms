using System;

namespace cms.data.Dtos
{
	public class GridListDto
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public bool Home { get; set; }
		public string Category { get; set; }
		public ResourceDtoLoc ResourceDto { get; set; }
	}
}