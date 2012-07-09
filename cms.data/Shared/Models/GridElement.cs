using System.Collections.Generic;

namespace cms.data.Shared.Models
{
	public class GridElement : IEntity
	{
		public GridElement()
		{
			//Status = DomainModels.Status.Private;
			Grid = new List<Grid>();
			Type = "text";
		}

		public int Id { get; set; }
		public int Line { get; set; }
		public int Width { get; set; }
		public int Position { get; set; }
		public string Content { get; set; }

		//public StatusWrapper Status { get; set; }
	
		public string Type { get; set; }
		public string Skin { get; set; }

		public virtual ICollection<Grid> Grid { get; set; }

	}
}