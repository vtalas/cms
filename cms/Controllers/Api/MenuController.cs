using System;
using System.Web.Mvc;
using cms.data.Dtos;

namespace cms.Controllers.Api
{
	[Authorize]
	public class MenuController : ApiControllerBase
	{
		[HttpPost]
		public ActionResult Add(MenuDto data)
		{
			using (var db = SessionProvider.CreateSession())
			{
				data.Category = shared.CategoryEnum.Menu;

				var newgrid = db.Session.Menu.Add(data);
				return new JSONNetResult(newgrid);
			}
		}
		[HttpPost]
		public ActionResult AddMenuItem(MenuItemDto data, Guid gridid)
		{
			using (var db = SessionProvider.CreateSession())
			{
				var newgrid = db.Session.Menu.AddMenuItem(data, gridid);
				return new JSONNetResult(newgrid);
			}
		}

		[HttpGet]
		public ActionResult Get(Guid? id)
		{
			using (var db = SessionProvider.CreateSession())
			{
				var g = db.Session.Menu.Get(id.Value);
				return new JSONNetResult(g);
			}
		}

		[HttpPost]
		public ActionResult Delete(Guid id)
		{
			using (var db = SessionProvider.CreateSession())
			{
				db.Session.Menu.Delete(id);
				return new JSONNetResult(null);
			}
		}

		[HttpPost]
		public ActionResult Update(GridPageDto data)
		{
			using (var db = SessionProvider.CreateSession())
			{
				data.Category = shared.CategoryEnum.Menu;

				var updated = db.Session.Page.Update(data);
				return new JSONNetResult(updated);
			}
		}

		public ActionResult List()
		{
			using (var db = SessionProvider.CreateSession())
			{
				var g = db.Session.Menu.List();
				return new JSONNetResult(g);
			}
		}

	}
}
