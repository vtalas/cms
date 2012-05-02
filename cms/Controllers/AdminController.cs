﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using cms.data;
using cms.data.EF;
using cms.data.Files;

namespace cms.Controllers
{

	public class AdminControllerBase : Controller
	{

		public string ViewPath(string view)
		{
			return string.Format("{0}/{1}", Application, view);
		}
		public string Application { get; private set; }
		protected override void Initialize(System.Web.Routing.RequestContext requestContext)
		{
			base.Initialize(requestContext);
			var a = this.RouteData;
			Application = a.Values["application"].ToString();

		}
		public class JSONNetResult : ActionResult
		{
			private readonly JObject _data;
			public JSONNetResult(JObject data)
			{
				_data = data;
			}

			public override void ExecuteResult(ControllerContext context)
			{
				var response = context.HttpContext.Response;
				response.ContentType = "application/json";
				response.Write(_data.ToString(Newtonsoft.Json.Formatting.None));
			}
		}
	}

	public class AdminController : AdminControllerBase
    {
		private JsonDataProvider aaa { get; set; }

		public AdminController()
		{
			aaa = new JsonDataFiles("xxx");
		}

		public ActionResult Index()
		{
			if (!string.IsNullOrEmpty(Application))
			{
				return RedirectToAction("Show");
			}

			var applications = aaa.Applications();
			return View(applications);
        }
		
		public ActionResult Show()
		{
			return View();
        }

    }
}
