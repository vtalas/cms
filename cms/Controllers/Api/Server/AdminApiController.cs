using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using cms.Code.UserResources;
using cms.Controllers.Api.Server.Attributes;
using cms.data.Dtos;
using cms.data.Extensions;
using cms.data.Shared.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebMatrix.WebData;

namespace cms.Controllers.Api.Server
{
	[System.Web.Http.Authorize(Roles = "admin")]
	public class AdminApiController : WebApiControllerBase
	{ 
		[System.Web.Mvc.HttpPost]
		public ApplicationSettingDto AddApplication(ApplicationSetting data)
		{
			using (var db = SessionProvider.CreateSession())
			{
				var a = db.Session.Add(data, WebSecurity.CurrentUserId);
				UserResourcesManagerProvider.CreateApplication(a.Id);
				return a.ToDto();
			}
		}
		
		public string PutCulture([FromBody]string culture)
		{
			//TODO:ocekovat povolene
			shared.SharedLayer.SetCulture(culture);
			return culture;
		}


		[System.Web.Http.HttpPut]
		public async void SaveJson([FromBody]object id)
		{
			var path = Path.Combine(HttpContext.Current.Server.MapPath("~/"), "App_Data\\ApplicationData", ApplicationId.ToString(),  "chuj.json");
			using (var outfile = new StreamWriter(@path, false, new UTF8Encoding()))
			{
				await outfile.WriteAsync(id.ToString());
			}
		}

		[System.Web.Http.HttpGet]
		public HttpResponseMessage GetJson()
		{
			var path = Path.Combine(HttpContext.Current.Server.MapPath("~/"), "App_Data\\ApplicationData", ApplicationId.ToString(), "chuj.json");
			var result = new HttpResponseMessage(HttpStatusCode.OK);
			if (!File.Exists(path))
			{
				return result;
			}
	
			var stream = new FileStream(path, FileMode.Open);
			result.Content = new StreamContent(stream);
			result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
			return result;
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




//		public List<string> AvailableTemplates()
//		{
//			
//		}

			/// ////////////////////////////////////////////////////////////////////////////////////
		//TODO: PRESUNOUT DO SAMOSTATNYHO C	ONTROLu

		[System.Web.Mvc.HttpPost]
		[InvalidateCacheOutputClientApiController]
		public GridPageDto AddGrid(GridPageDto data)
		{
			using (var db = SessionProvider.CreateSession())
			{
				var newgrid = db.Session.Page.Add(data);
				return newgrid;
			}
		}

		[InvalidateCacheOutputClientApiController]
		public void DeleteGrid([FromBody]Guid id)
		{
			using (var db = SessionProvider.CreateSession())
			{
				db.Session.Page.Delete(id);
			}
		}

		[System.Web.Mvc.HttpPost]
		[InvalidateCacheOutputClientApiController]
		public GridPageDto UpdateGrid(GridPageDto data)
		{
			using (var db = SessionProvider.CreateSession())
			{
				var updated = db.Session.Page.Update(data);
				return updated;
			}
		}

		[System.Web.Mvc.AcceptVerbs("GET")]
		[System.Web.Http.HttpGet]
		public IEnumerable<GridListDto> Grids()
		{
			using (var db = SessionProvider.CreateSession())
			{
				var g = db.Session.Grids();
				return g;
			}
		}

		//TODO: prejmenovat na Get
		public GridPageDto GetGrid(Guid? id)
		{
			using (var db = SessionProvider.CreateSession())
			{
				var g = db.Session.Page.Get(id.Value);
				return g;
			}
		}

	}

}
