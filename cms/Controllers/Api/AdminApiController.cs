using System;
using System.IO;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using cms.Code;
using cms.data.Dtos;
using cms.data.Shared.Models;
using cms.shared;

namespace cms.Controllers.Api
{
	//AJAX akce
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
		public ActionResult DeleteGridElement(GridElement data, Guid gridId)
		{
			using (var db = SessionProvider.CreateSession)
			{
				db.DeleteGridElement(data.Id, gridId);
				return new JSONNetResult(null);
			}
		}


		[HttpPost]
		[JObjectFilter(Param = "data")]
		public ActionResult UpdateGridElement(JObject data)
		//public ActionResult UpdateGridElement(GridElementDto data)
		{
			using (var db = SessionProvider.CreateSession)
			{
				var g = db.Update(data["data"].ToGridElementDto());
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

		[HttpPost]
		public ActionResult AddGrid(GridPageDto data)
		{
			using (var db = SessionProvider.CreateSession)
			{
				data.Category = CategoryEnum.Page;

				var newgrid = db.Page.Add(data);
				return new JSONNetResult(newgrid);
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
				var g = db.Grids();
				return new JSONNetResult(g);
			}
		}

		//TODO: prejmenovat na Get
		public ActionResult GetGrid(Guid? id)
		{
			using (var db = SessionProvider.CreateSession)
			{
				var g = db.Page.Get(id.Value);
				return new JSONNetResult(g);
			}
		}




	}

}
