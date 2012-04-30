using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Newtonsoft.Json.Linq;

namespace cms.Controllers
{
    public class ClientApiController : ClientControllerBase
    {
        public JsonResult Test()
        {
			 var list = new List<ListItem>() {
				 new ListItem() { Value = "1", Text = "VA" },
				 new ListItem() { Value = "2", Text = "MD" },
				 new ListItem() { Value = "3", Text = Application }
			 };
			 return Json(list);
		}
        public ActionResult ContentElement()
        {
        	var a = new JObject()
        	        	{
        	        		{"header", "laksnlansdasnlk d head"},
        	        		{"text", "l *aksnla*nsdasnlk d head"},
        	        		{
        	        			"image", new JObject()
        	        			         	{
        	        			         		{"thumb", "thumb"},
        	        			         		{"fullsize", "fullsize"}
        	        			         	}
        	        			},
        	        	};
			return new JSONNetResult(a);
		}


        public ActionResult Index()
        {
        	return View();
        }


    }
}
