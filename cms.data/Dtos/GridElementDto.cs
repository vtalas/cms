using System;
using System.Collections.Generic;
using cms.data.EF.DataProvider;

namespace cms.data.Dtos
{
	public class GridElementDto : IResourceElement
	{
		public Guid Id { get; set; }
		public int Line { get; set; }
		public int Width { get; set; }
		public int Position { get; set; }
		public string Content { get; set; }
		public string Type { get; set; }
		public string Skin { get; set; }
	
		public string ParentId { get; set; }
		public IDictionary<string, ResourceDtoLoc> ResourcesLoc { get; set; }
	}
}