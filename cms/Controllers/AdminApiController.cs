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

namespace cms.Controllers
{
    public class AdminApiController : ApiControllerBase
    {

        public ActionResult Applications()
        {
        	var applications = db.Applications();
			return new JSONNetResult(applications);
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
        public ActionResult Show()
        {
        	return View();
        }

    }
}
