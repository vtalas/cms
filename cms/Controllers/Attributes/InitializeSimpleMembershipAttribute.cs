using System;
using System.Web.Mvc;
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