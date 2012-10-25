using System;
using System.Collections.Generic;

namespace cms.data.Shared.Models
{
	public class GridElement : IEntity, IEntityWithResource
	{
		public GridElement()
		{
			//Status = DomainModels.Status.Private;
			Id = Guid.NewGuid();
			//Grid = new List<Grid>();
			Resources = new List<Resource>();
			Type = "text";
		}

		public Guid Id { get; set; }
		public int Width { get; set; }
		public int Position { get; set; }
		public string Content { get; set; }
		//public StatusWrapper Status { get; set; }
	
		public string Type { get; set; }
		public string Skin { get; set; }
			
		public virtual GridElement Parent { get; set; }
		public virtual Grid Grid { get; set; }
		public virtual ICollection<Resource> Resources { get; set; }

	}


}