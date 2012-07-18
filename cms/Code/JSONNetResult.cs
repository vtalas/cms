using System.Web.Mvc;
using Newtonsoft.Json;

namespace cms.Code
{
	public class JSONNetResult : ActionResult
	{
		private readonly object _data;
		public JSONNetResult(object data)
		{
			_data = data;
		}

		public static string ToJson(object data)
		{
			var settings = new JsonSerializerSettings()
			               	{
			               		ReferenceLoopHandling = ReferenceLoopHandling.Ignore
			               	};
			return JsonConvert.SerializeObject(data, Formatting.Indented, settings);
		}

		public override void ExecuteResult(ControllerContext context)
		{
			var response = context.HttpContext.Response;
			response.ContentType = "application/json";
			response.Write(ToJson(_data));
		}
	}
}