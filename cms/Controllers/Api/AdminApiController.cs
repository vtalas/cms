using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebMatrix.WebData;
using cms.Code.UserResources;
using cms.data.Dtos;
using cms.data.Extensions;
using cms.data.Shared.Models;

namespace cms.Controllers.Api
{
	//AJAX akce
	[System.Web.Mvc.Authorize]
	public class AdminApiController : WebApiControllerBase
	{ 
		[System.Web.Mvc.HttpPost]
		public ApplicationSetting AddApplication(ApplicationSetting data)
		{
			using (var db = SessionProvider.CreateSession)
			{
				var a = db.Add(data, WebSecurity.CurrentUserId);
				UserResourcesManagerProvider.CreateApplication(a.Id);
				return a;
			}
		}
		
		public string PutCulture([FromBody]string culture)
		{
			//TODO:ocekovat povolene
			shared.SharedLayer.SetCulture(culture);
			return culture;
		}

		public class JObjectFilter : ActionFilterAttribute
		{
			public string Param { get; set; }
			public override void OnActionExecuting(ActionExecutingContext filterContext)
			{
				if ((filterContext.HttpContext.Request.ContentType ?? string.Empty).Contains("application/json"))
				{

					var stream = filterContext.HttpContext.Request.InputStream;
					stream.Position = 0;
					var streamReader = new StreamReader(stream);
					var jobject = JsonConvert.DeserializeObject<JObject>(streamReader.ReadToEnd());

					filterContext.ActionParameters[Param] = jobject;
				}
			}
		}

/// ////////////////////////////////////////////////////////////////////////////////////
		//TODO: PRESUNOUT DO SAMOSTATNYHO CONTROLu

		[System.Web.Mvc.HttpPost]
		public GridPageDto AddGrid(GridPageDto data)
		{
			using (var db = SessionProvider.CreateSession)
			{
				var newgrid = db.Page.Add(data);
				return newgrid;
			}
		}

		public void DeleteGrid([FromBody]Guid id)
		{
			using (var db = SessionProvider.CreateSession)
			{
				db.Page.Delete(id);
			}
		}

		[System.Web.Mvc.HttpPost]
		public GridPageDto UpdateGrid(GridPageDto data)
		{
			using (var db = SessionProvider.CreateSession)
			{
				var updated = db.Page.Update(data);
				return updated;
			}
		}

		[System.Web.Mvc.AcceptVerbs("GET")]
		[System.Web.Http.HttpGet]
		public IEnumerable<GridListDto> Grids()
		{
			using (var db = SessionProvider.CreateSession)
			{
				var g = db.Grids();
				return g;
			}
		}

		//TODO: prejmenovat na Get
		public GridPageDto GetGrid(Guid? id)
		{
			using (var db = SessionProvider.CreateSession)
			{
				var g = db.Page.Get(id.Value);
				return g;
			}
		}

	}

}
