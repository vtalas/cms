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
        public ActionResult ApplicationAdd(ApplicationSetting data)
        {
        	var a = db.Add(data);		
			return new JSONNetResult(a);
		}

        public ActionResult Show()
        {
			var g = db.GetGrid(1);
        	var grid = g;

			return new JSONNetResult(grid);
		}

		public ActionResult Grids()
        {
			var g = db.Grids();
			return new JSONNetResult(g);
		}

		public ActionResult GetGrid(int id)
        {
			var g = db.GetGrid(id);
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
