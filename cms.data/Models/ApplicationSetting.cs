using System.Collections.Generic;

namespace cms.data.Models
{
	public class ApplicationSetting :IEntity
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public virtual ICollection<Grid> Grids { get; set; }
	}

	public class Grid :IEntity
	{
		public Grid()
		{
			//GridType = DomainModels.GridType.Content;
			//Status = DomainModels.Status.Private;
			GridElements = new List<GridElement>();
		}
		public int Id { get; set; }
		public string Name { get; set; }
		public string Link { get; set; }
		public bool Home { get; set; }
		//public GridTypeWrapper GridType { get; set; }
		//public StatusWrapper Status { get; set; }

		public virtual ApplicationSetting ApplicationSettings { get; set; }
		public virtual ICollection<GridElement> GridElements { get; set; }
	}

	public class GridElement : IEntity
	{
		public GridElement()
		{
			//Status = DomainModels.Status.Private;
			Grid = new List<Grid>();
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