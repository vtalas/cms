using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using cms.data.EF;

namespace cms.Controllers
{
    public class HomeController : Controller
    {
		
		public ActionResult Index()
		{
			return View();
        }
		public ActionResult sd()
        {
            return View();
        }

    }
}
