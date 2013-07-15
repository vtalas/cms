using System.Collections.Generic;
using System.Runtime.Caching;
using cms.Code.LinkAccounts;
using cms.Code.LinkAccounts.Picasa;

namespace cms.Controllers.Api
{
	[System.Web.Mvc.Authorize(Roles = "admin")]
	public class SettingApiController : WebApiControllerBase
	{
		public List<GdataPhotosSettings> GetPhotosSettings()
		{
			var x = new SettingsLoader<List<GdataPhotosSettings>>(SessionProvider, "GdataPhotosSettings_List.json").Get();
			return x ?? new List<GdataPhotosSettings>();
		}
		public void PutPhotosSettings(List<GdataPhotosSettings> data)
		{
			MvcApplication.Cache.Dispose();
			MvcApplication.Cache = MemoryCache.Default;

			new SettingsLoader<List<GdataPhotosSettings>>(SessionProvider, "GdataPhotosSettings_List.json").Set(data);
		}
	}
}
