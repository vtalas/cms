using System.Web.Mvc;
using Newtonsoft.Json.Linq;

namespace cms.Controllers
{
	public class ClientControllerBase : Controller
	{
		public string ViewPath(string view)
		{
			return string.Format("{0}/{1}", Application, view);
		}
		public string Application { get;  private set; }
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

	public class ClientController : ClientControllerBase
    {
        public ActionResult Index()
        {
			return View(ViewPath("index"));
        }

    }
}
