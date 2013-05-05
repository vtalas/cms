using System;
using System.Web.Mvc;

namespace cms.Controllers
{
	[Authorize]
	public class GDataAuthController : Controller
	{
		public ActionResult Authenticate(string code, string state)
		{
			return RedirectToAction("Authenticate", "GData", new {code = code, applicationId = state});
		}
	}

}
