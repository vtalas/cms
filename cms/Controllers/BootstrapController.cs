using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace cms.Controllers
{
    public class BootstrapController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult aaa()
        {
        	var a = "aasdsa";
        	return new JSONNetResult(a);
        }


        public ActionResult Current()
        {
			
			var variables = string.Format("~/App_Themes/bootstrap/less/user/variables_{0}.less", Session.SessionID);
			var bootstrap = string.Format("~/App_Themes/bootstrap/less/user/bootstrap_{0}.less", Session.SessionID);

			var file = Server.MapPath( variables);
        	if (!System.IO.File.Exists(file))
        	{
        		System.IO.File.Create(file);
        	}
			return new JSONNetResult(variables);
        }
    }
}
