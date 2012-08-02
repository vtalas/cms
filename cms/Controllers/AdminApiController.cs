using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using cms.data;
using cms.data.Dtos;
using cms.data.EF;
using cms.data.Shared.Models;

namespace cms.Controllers
{
    public class AdminApiController : ApiControllerBase
    {
		//public ActionResult Applications()
		//{
		//    var applications = db.Applications();
		//    return new JSONNetResult(applications);
		//}
		
		[HttpPost]
		public ActionResult AddApplication(ApplicationSetting data)
        {
        	var a = db.Add(data);		
			return new JSONNetResult(a);
		}
		
		[HttpPost]
		public ActionResult AddGridElement(GridElement data, Guid gridId)
        {
			var newitem = db.AddGridElementToGrid(data, gridId);
			return new JSONNetResult(newitem.ToDto());
		}

		[HttpPost]
		public ActionResult AddGrid(GridPageDto data)
        {
			using ( var dataEf = new JsonDataEf(ApplicationId))
			{
				var newgrid = dataEf.Add(data);
				return new JSONNetResult(newgrid);
			}
        }

		[HttpPost]
		public ActionResult DeleteGridElement(GridElement data, Guid gridId)
        {
			db.DeleteGridElement(data.Id,gridId); 
			return new JSONNetResult(null);
		}

		[HttpPost]
		public ActionResult DeleteGrid(Guid id)
        {
			db.DeleteGrid(id); 
			return new JSONNetResult(null);
		}
		
		[HttpPost]
		public ActionResult UpdateGrid(GridPageDto data)
        {
			var updated = db.Update(data); 
			return new JSONNetResult(updated);
		}
		
		public ActionResult Grids()
        {
			var g = db.GridPages();
			return new JSONNetResult(g);
		}
		public ActionResult GetGrid(Guid? id)
        {
			var g = db.GetGridPage(id.Value);
			return new JSONNetResult(g);
		}

		[HttpPost]
		public ActionResult UpdateGridElement(GridElement data)
		{
			var g = db.Update(data);
			return new JSONNetResult(g.ToDto());
		}
		public ActionResult GetGridElement(Guid id)
        {
			var g = db.GetGridElement(id);
			return new JSONNetResult(g);
		}
		public ActionResult Error()
		{
			var e = new JObject()
        	        	{
        	        		{"error", true},
        	        		{"message", "Not Fount"}
        	        	};
			return new JSONNetResult(e);

		}
    }

}
