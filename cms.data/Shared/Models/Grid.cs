using System;
using System.Collections.Generic;

namespace cms.data.Shared.Models
{
	public class Menu :IEntity
	{
		public Menu()
		{
			Id = Guid.NewGuid();
		}

		public Guid Id { get; set; }
		public string Name { get; set; }
		public virtual ICollection<Resource> Resources { get; set; }
	}

	public class Grid :IEntity
	{
		public Grid()
		{
			//GridType = DomainModels.GridType.Content;
			//Status = DomainModels.Status.Private;
			Id = Guid.NewGuid();
			GridElements = new List<GridElement>();
		}
		public Guid Id { get; set; }
		public string Name { get; set; }
		//public string Link { get; set; }
		public bool Home { get; set; }
		//public GridTypeWrapper GridType { get; set; }
		//public StatusWrapper Status { get; set; }
		public virtual ApplicationSetting ApplicationSettings { get; set; }
		public virtual Resource Resource { get; set; }
		public virtual ICollection<GridElement> GridElements { get; set; }
	}
}