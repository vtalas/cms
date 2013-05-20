using System;
using System.Collections.Generic;
using cms.shared;

namespace cms.data.Shared.Models
{
	public class Grid :IEntity, IEntityWithResource
	{
		public Grid()
		{
			Id = Guid.NewGuid();
			GridElements = new List<GridElement>();
			Resources = new List<Resource>();
			Category = CategoryEnum.Page;
			Authorize = false;
		}

		public Guid Id { get; set; }
		public bool Home { get; set; }
		public bool Authorize { get; set; }
		public string Category { get; set; }
		public virtual ApplicationSetting ApplicationSettings { get; set; }
		public virtual ICollection<GridElement> GridElements { get; set; }
		public virtual ICollection<Resource> Resources { get; set; }
	}
}