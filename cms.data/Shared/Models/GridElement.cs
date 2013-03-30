using System;
using System.Collections.Generic;

namespace cms.data.Shared.Models
{
	public class GridElement : IEntity, IEntityWithResource
	{
		public GridElement()
		{
			Id = Guid.NewGuid();
			Resources = new List<Resource>();
			Type = "text";
		}

		public Guid Id { get; set; }
		public int Width { get; set; }
		public int Position { get; set; }
		public string Content { get; set; }
	
		public string Type { get; set; }
		public string Skin { get; set; }
			
		public virtual ApplicationSetting AppliceSetting { get; set; }
		public virtual GridElement Parent { get; set; }
		public virtual Grid Grid { get; set; }
		public virtual ICollection<Resource> Resources { get; set; }

		public static bool EqualsById(GridElement g1, GridElement g2)
		{
			if (g1 == null && g2 == null)
			{
				return true;
			}
			return (g1 != null && g2 != null && g1.Id == g2.Id);
		}

	}


}