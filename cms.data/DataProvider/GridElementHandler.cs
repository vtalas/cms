using cms.data.Dtos;
using cms.data.Repository;
using cms.data.Shared.Models;

namespace cms.data.DataProvider
{
	public class GridElementHandler
	{
		private IRepository repository;
		private GridElementDto dto;
		
		public GridElementHandler(GridElementDto item, IRepository db)
		{
			repository = db;
			dto = item;
		}

		public GridElement EntityObject()
		{
			return  new GridElement();
		}
	}
}