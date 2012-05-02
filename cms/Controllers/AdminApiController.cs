using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Newtonsoft.Json.Linq;

namespace cms.Controllers
{
    public class AdminApiController : ApiControllerBase
    {
        public ActionResult Index()
        {
			return new JSONNetResult(e);
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
