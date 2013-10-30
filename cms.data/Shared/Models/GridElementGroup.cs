namespace cms.data.Shared.Models
{
	public class GridElementGroup : IEntity
	{
		public int Id { get; set; }
		public virtual Resource Name { get; set; }
	}
}