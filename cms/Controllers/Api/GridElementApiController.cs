using System;
using cms.data.Dtos;
using cms.data.Extensions;
using cms.data.Shared.Models;

namespace cms.Controllers.Api
{
	[System.Web.Mvc.Authorize]
	public class GridElementApiController : WebApiControllerBase
	{ 
		public GridElementDto PostGridElement(GridElement data, Guid gridId)
		{
			using (var db = SessionProvider.CreateSession)
			{
				var newitem = db.Page.AddToGrid(data, gridId);
				return newitem.ToDto();
			}
		}

		public GridElementDto Get(Guid gridId)
		{
			return new GridElementDto();
		}

		[System.Web.Mvc.HttpPost]
		public void DeleteGridElement(GridElement data, Guid gridId)
		{
			using (var db = SessionProvider.CreateSession)
			{
				db.Page.Delete(data.Id, gridId);
			}
		}

		[System.Web.Mvc.HttpPost]
		public GridElementDto UpdateGridElement(GridElementDto data)
		{
			using (var db = SessionProvider.CreateSession)
			{
				return db.Page.Update(data);
			}
		}

	}

}
