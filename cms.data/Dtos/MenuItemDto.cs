using System;
using System.Collections.Generic;

namespace cms.data.Dtos
{
	public class MenuItemDto 
	{
		public Guid Id { get; set; }
		public int Position { get; set; }

		public string Content { get; set; }
		public string Type { get; set; }
		public string Skin { get; set; }
		
		public string ParentId { get; set; }
		public IDictionary<string, ResourceDtoLoc> ResourcesLoc { get; set; }

		public IEnumerable<MenuItemDto> Children { get; set; }

	}
}