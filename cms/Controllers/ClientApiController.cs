﻿using System;
using System.Web.Mvc;
using cms.Code;

namespace cms.Controllers
{
    public class ClientApiController : ApiControllerBase
    {
		public ActionResult ViewPageJson(string link)
		{
			using (var db = SessionProvider.CreateSession)
			{
				var a = db.GetGridPage(link);
				return new Code.JSONNetResult(a);
			}
		}

		public ActionResult GetGrid(Guid id)
		{
			using (var db = SessionProvider.CreateSession)
			{
				var a = db.GetGridPage(id);
				return new Code.JSONNetResult(a);
			}
		}
	}
}
