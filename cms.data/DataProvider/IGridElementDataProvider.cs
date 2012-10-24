using System;
using cms.data.Dtos;
using cms.data.Shared.Models;

namespace cms.data.DataProvider
{
	public interface IGridElementDataProvider
	{
		GridElement AddToGrid(GridElement gridElement, Guid gridId);
		void Delete(Guid guid, Guid gridid);
		GridElementDto Update(GridElementDto item);

	}
}