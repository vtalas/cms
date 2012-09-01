using System;
using System.Web.Mvc;
using cms.data.Dtos;

namespace cms.Controllers.Api
{
    public class MenuController : ApiControllerBase
    {
		[HttpPost]
		public ActionResult Add(MenuDto data)
		{
			using (var db = SessionProvider.CreateSession)
			{
				data.Category = shared.CategoryEnum.Menu;
				
				var newgrid = db.Menu.Add(data);
				return new JSONNetResult(newgrid);
			}
		}
		[HttpPost]
		public ActionResult AddMenuItem(MenuItemDto data, Guid gridid)
		{
			using (var db = SessionProvider.CreateSession)
			{
				var newgrid = db.Menu.AddMenuItem(data, gridid);
				return new JSONNetResult(newgrid);
			}
		}

		[HttpGet]
		public ActionResult Get(Guid? id)
		{
			using (var db = SessionProvider.CreateSession)
			{
				var g = db.Menu.Get(id.Value);
				return new JSONNetResult(g);
			}
		}

		[HttpPost]
		public ActionResult Delete(Guid id)
		{
			using (var db = SessionProvider.CreateSession)
			{
				db.DeleteGrid(id);
				return new JSONNetResult(null);
			}
		}

		[HttpPost]
		public ActionResult Update(GridPageDto data)
		{
			using (var db = SessionProvider.CreateSession)
			{
				data.Category = shared.CategoryEnum.Menu;
				
				var updated = db.Update(data);
				return new JSONNetResult(updated);
			}
		}

		public ActionResult List()
		{
			using (var db = SessionProvider.CreateSession)
			{
				var g = db.Menu.List();
				return new JSONNetResult(g);
			}
		}

	}
}
