using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using cms.data;
using cms.data.Files;
using cms.data.Models;

namespace cms.Controllers
{
    public class AdminApiController : ApiControllerBase
    {
        public ActionResult Applications()
        {
        	var applications = db.Applications();
			return new JSONNetResult(applications);
		}
        public ActionResult AddApplication(ApplicationSetting data)
        {
        	var a = db.Add(data);		
			return new JSONNetResult(a);
		}
        public ActionResult AddGridElement(GridElement data, int gridId)
        {
        	var grid = db.GetGrid(gridId);
        	data.Grid.Add(grid);
			
			var newitem = db.Add(data);
			return new JSONNetResult(newitem);
		}

        public ActionResult AddGrid(Grid data)
        {
        	var newgrid =  db.Add(data);
			return new JSONNetResult(newgrid);
        }

        public ActionResult DeleteGridElement(GridElement data)
        {
			db.DeleteGridElement(data.Id); 
			return new JSONNetResult(null);
		}
		
		public ActionResult Grids()
        {
			var g = db.Grids();
			return new JSONNetResult(g);
		}
		public ActionResult GetGrid(int id)
        {
			var g = db.GetGridPage(id);
			return new JSONNetResult(g);
		}
		
		public ActionResult UpdateGridElement(GridElement item)
		{
			var g = db.Update(item);
			return new JSONNetResult(g);
		}
		public ActionResult GetGridElement(int id)
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
