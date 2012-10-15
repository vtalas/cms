using System;
using System.Threading;
using System.Web.Mvc;
using WebMatrix.WebData;
using cms.data.EF;

namespace cms.Controllers.Attributes
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
	public sealed class InitializeSimpleMembershipAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			SecurityProvider.EnsureInitialized();
		}

	}
}