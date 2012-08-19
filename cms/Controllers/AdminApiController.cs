using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using cms.Code;
using cms.data;
using cms.data.Dtos;
using cms.data.EF;
using cms.data.Shared.Models;

namespace cms.Controllers
{
    //AJAX ax=kce
	public class AdminApiController : ApiControllerBase
    {
		
		[HttpPost]
		public ActionResult AddApplication(ApplicationSetting data)
        {
			using (var db = SessionProvider.CreateSession)
			{
				var a = db.Add(data);
				return new JSONNetResult(a);
			}
        }
		
		[HttpPost]
		public ActionResult AddGridElement(GridElement data, Guid gridId)
        {
			using (var db = SessionProvider.CreateSession)
			{
				var newitem = db.AddGridElementToGrid(data, gridId);
				return new JSONNetResult(newitem.ToDto());
			}
        }

		[HttpPost]
		public ActionResult AddGrid(GridPageDto data)
        {
			using (var db = SessionProvider.CreateSession)
			{
				var newgrid = db.Add(data);
				return new JSONNetResult(newgrid);
			}
        }

		[HttpPost]
		public ActionResult DeleteGridElement(GridElement data, Guid gridId)
        {
			using (var db = SessionProvider.CreateSession)
			{
				db.DeleteGridElement(data.Id, gridId);
				return new JSONNetResult(null);
			}
        }

		[HttpPost]
		public ActionResult DeleteGrid(Guid id)
        {
			using (var db = SessionProvider.CreateSession)
			{
				db.DeleteGrid(id);
				return new JSONNetResult(null);
			}
        }
		
		[HttpPost]
		public ActionResult UpdateGrid(GridPageDto data)
        {
			using (var db = SessionProvider.CreateSession)
			{
				var updated = db.Update(data);
				return new JSONNetResult(updated);
			}
        }
		
		public ActionResult Grids()
        {
			using (var db = SessionProvider.CreateSession)
			{
				var g = db.GridPages();
				return new JSONNetResult(g);
			}
        }
		
		public ActionResult GetGrid(Guid? id)
        {
			using (var db = SessionProvider.CreateSession)
			{
				var g = db.GetGridPage(id.Value);
				return new JSONNetResult(g);
			}
		}

		[HttpPost]
		[JObjectFilter(Param = "data")]
		public ActionResult UpdateGridElement(GridElementDto data)
		//public ActionResult UpdateGridElement(GridElementDto data)
		{
			using (var db = SessionProvider.CreateSession)
			{
				var g = db.Update(data);
				return new JSONNetResult(g);
			}
		}
		
		public ActionResult GetGridElement(Guid id)
        {
			using (var db = SessionProvider.CreateSession)
			{
				var g = db.GetGridElement(id);
				return new JSONNetResult(g);
			}
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
		
		public ActionResult SetCulture(string culture)
		{
			//TODO:ocekovat povolene
			shared.SharedLayer.Culture = culture;
			return new EmptyResult();
		}

		public class JObjectFilter : ActionFilterAttribute
		{

			public string Param { get; set; }

			public override void OnActionExecuting(ActionExecutingContext filterContext)
			{
				if ((filterContext.HttpContext.Request.ContentType ?? string.Empty).Contains("application/json"))
				{
					

					var dto = filterContext.ActionParameters[Param] as GridElementDto;
					if (dto != null)
					{
						var stream = filterContext.HttpContext.Request.InputStream;
						stream.Position = 0;
						var streamReader = new StreamReader(stream);
						var jobject = JsonConvert.DeserializeObject<JObject>(streamReader.ReadToEnd());

						var resources = jobject["data"]["ResourcesLoc"]
							.ToDictionary(token => token["aaa"], token => token.Values());
						//var xx = resources as IDictionary<string, ResourceDtoLoc>;
						filterContext.ActionParameters[Param] = dto;
					}


				}
			}
		}

    }

}
