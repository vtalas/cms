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

		public GridElementDto Post(GridElementDto gridElementDto)
		{
			using (var db = SessionProvider.CreateSession)
			{
				return db.Page.Add(gridElementDto);
			}
		}

		public GridElementDto Get(Guid id)
		{
			var x =  new GridElementDto {Id = id, Content = "lknasljkdnasjkld"};
			return x;
		}

		public void Delete(GridElement data, Guid gridId)
		{
			using (var db = SessionProvider.CreateSession)
			{
				db.Page.Delete(data.Id, gridId);
			}
		}

		public GridElementDto Put(GridElementDto data)
		{
			using (var db = SessionProvider.CreateSession)
			{
				return db.Page.Update(data);
			}
		}

	}

}
