using System;
using System.Web.Http.Filters;
using WebAPI.OutputCache;

namespace cms.Controllers.Api.Server.Attributes
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
	public sealed class InvalidateCacheOutputClientApiControllerAttribute : BaseCacheAttribute
	{
		public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
		{
			if (actionExecutedContext.Response != null && !actionExecutedContext.Response.IsSuccessStatusCode)
				return;

			const string controller = "ClientApi";
			var config = actionExecutedContext.ActionContext.ControllerContext.Configuration;
			var cache = config.CacheOutputConfiguration().GetCacheOutputProvider(actionExecutedContext.Request);

			cache.RemoveStartsWith(config.CacheOutputConfiguration().MakeBaseCachekey(controller, "GetAlbums"));
			cache.RemoveStartsWith(config.CacheOutputConfiguration().MakeBaseCachekey(controller, "GetPage"));
			cache.RemoveStartsWith(config.CacheOutputConfiguration().MakeBaseCachekey(controller, "GetPages"));
			cache.RemoveStartsWith(config.CacheOutputConfiguration().MakeBaseCachekey(controller, "GetAlbum"));
			cache.RemoveStartsWith(config.CacheOutputConfiguration().MakeBaseCachekey(controller, "GetAlbums"));
			cache.RemoveStartsWith(config.CacheOutputConfiguration().MakeBaseCachekey(controller, "GetAlbumPhotos"));
			cache.RemoveStartsWith(config.CacheOutputConfiguration().MakeBaseCachekey(controller, "GetPhotos"));
		}
	}
}