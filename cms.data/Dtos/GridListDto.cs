using System;

namespace cms.data.Dtos
{
	public class GridListDto
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string Link { get; set; }
		public bool Home { get; set; }
		public string Category { get; set; }
		public bool Authorize { get; set; }
	}
}