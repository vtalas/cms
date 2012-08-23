﻿using System;
using System.Web.Mvc;

namespace cms.Controllers.Api
{
    public class ClientApiController : ApiControllerBase
    {
		public ActionResult ViewPageJson(string link)
		{
			using (var db = SessionProvider.CreateSession)
			{
				var a = db.GetPage(link);
				return new Code.JSONNetResult(a);
			}
		}

		public ActionResult GetGrid(Guid id)
		{
			using (var db = SessionProvider.CreateSession)
			{
				var a = db.GetPage(id);
				return new Code.JSONNetResult(a);
			}
		}
	}
}
