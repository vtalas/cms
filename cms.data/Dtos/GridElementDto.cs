using System.Collections.Generic;
using cms.data.Shared.Models;

namespace cms.data.Dtos
{
	public class GridElementDto 
	{
		public int Id { get; set; }
		public int Line { get; set; }
		public int Width { get; set; }
		public int Position { get; set; }
		public string Content { get; set; }
		public string Type { get; set; }
		public string Skin { get; set; }
		public IEnumerable<ResourceDto> Resources { get; set; }
	}
}