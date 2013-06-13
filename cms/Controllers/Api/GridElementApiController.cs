using System;
using cms.data.Dtos;

namespace cms.Controllers.Api
{
	[System.Web.Mvc.Authorize]
	public class GridElementApiController : WebApiControllerBase
	{ 
		public GridElementDto PostToGrid(GridElementDto gridElementDto, Guid gridId)
		{
			using (var db = SessionProvider.CreateSession())
			{
				return db.Session.Page.AddToGrid(gridElementDto, gridId);
			}
		}

		public GridElementDto Post(GridElementDto gridElementDto)
		{
			using (var db = SessionProvider.CreateSession())
			{
				return db.Session.Page.Add(gridElementDto);
			}
		}

		public void Delete(Guid id )
		{
			using (var db = SessionProvider.CreateSession())
			{
				db.Session.Page.DeleteGridElement(id);
			}
		}

		public GridElementDto Put(GridElementDto data)
		{
			using (var db = SessionProvider.CreateSession())
			{
				return db.Session.Page.Update(data);
			}
		}

	}

}
