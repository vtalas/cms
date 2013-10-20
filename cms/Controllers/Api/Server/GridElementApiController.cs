using System;
using cms.Controllers.Api.Server.Attributes;
using cms.data.Dtos;


namespace cms.Controllers.Api.Server
{
	[System.Web.Mvc.Authorize]
	public class GridElementApiController : WebApiControllerBase
	{
		[InvalidateCacheOutputClientApiController]
		public GridElementDto PostToGrid(GridElementDto gridElementDto, Guid gridId)
		{
			using (var db = SessionProvider.CreateSession())
			{
				return db.Session.Page.AddToGrid(gridElementDto, gridId);
			}
		}

		[InvalidateCacheOutputClientApiController]
		public GridElementDto Post(GridElementDto gridElementDto)
		{
			using (var db = SessionProvider.CreateSession())
			{
				return db.Session.Page.Add(gridElementDto);
			}
		}

		[InvalidateCacheOutputClientApiController]
		public void Delete(Guid id)
		{
			using (var db = SessionProvider.CreateSession())
			{
				db.Session.Page.DeleteGridElement(id);
			}
		}

		[InvalidateCacheOutputClientApiController]
		public GridElementDto Put(GridElementDto data)
		{
			using (var db = SessionProvider.CreateSession())
			{
				return db.Session.Page.Update(data);
			}
		}

	}
}
