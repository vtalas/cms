using System.Collections.Generic;

namespace cms.data.Shared.Models
{
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
		//public string Link { get; set; }
		public bool Home { get; set; }
		//public GridTypeWrapper GridType { get; set; }
		//public StatusWrapper Status { get; set; }
		public virtual ApplicationSetting ApplicationSettings { get; set; }
		public virtual Resource Resource { get; set; }
		public virtual ICollection<GridElement> GridElements { get; set; }
	}
}