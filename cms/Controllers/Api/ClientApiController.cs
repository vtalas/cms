﻿using System;
using System.Linq;
using System.Web.Mvc;
using cms.data.Dtos;
using cms.data.EF;
using cms.data.EF.DataProviderImplementation;
using cms.data.EF.RepositoryImplementation;

namespace cms.Controllers.Api
{
    public class ClientApiController : WebApiControllerBase
    {
		public GridPageDto GetPage(string id)
		{
			//return new GridPageDto()
			//{
				return db.Page.Get(id);
			//};
			}
		}

		//public ActionResult GetGrid(Guid id)
		//{
		//	using (var db = SessionProvider.CreateSession)
		//	{
		//		var a = db.Page.Get(id);
		//		return new Code.JSONNetResult(a);
		//	}
		//}
	}
}

		//public ActionResult GetGrid(Guid id)
		//{
		//	using (var db = SessionProvider.CreateSession)
		//	{
		//		var a = db.Page.Get(id);
		//		return new Code.JSONNetResult(a);
		//	}
		//}
