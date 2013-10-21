using System.Data;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace cms.Controllers.Api.Filters
{
	public class ExceptionFilterAttribute : System.Web.Http.Filters.ExceptionFilterAttribute
	{
		public override void OnException(HttpActionExecutedContext context)
		{
			if (context.Exception is ObjectNotFoundException)
			{
				context.Response = new HttpResponseMessage(HttpStatusCode.NotFound);
			}
		}
	}
}