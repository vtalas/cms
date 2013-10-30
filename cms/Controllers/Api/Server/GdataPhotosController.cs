using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using cms.Code.Graphic;
using cms.Code.LinkAccounts.Picasa;
using WebAPI.OutputCache;

namespace cms.Controllers.Api.Server
{
	public class GdataPhotosController : WebApiPicasaControllerBase
	{
		public List<GdataPhotosSettings> AdminPhotoSettings { get; set; }

		public GdataPhotosController()
		{
			AdminPhotoSettings = new List<GdataPhotosSettings>
			{
				new GdataPhotosSettings(ImageSizeType.MaxSize, 95, true),
			};
		}


		[CacheOutput(ClientTimeSpan = 3600, ServerTimeSpan = 3600)]
		public IDictionary<string, AlbumDecorator> GetAlbums()
		{
			var albums = PicasaProvider.Session.GetAlbums().Select(x => new AlbumDecorator(x, AdminPhotoSettings));
			return albums.ToDictionary(key => key.Id);
		}

		[CacheOutput(ClientTimeSpan = 3600, ServerTimeSpan = 3600)]
		public AlbumDecorator GetAlbum(string id, bool refreshCache = false)
		{
			return PicasaProvider.Session.GetAlbum(id, refreshCache);
		}

		[CacheOutput(ClientTimeSpan = 3600, ServerTimeSpan = 3600)]
		public IEnumerable<PhotoDecorator> GetAlbumPhotos(string id, bool refreshCache = false)
		{
			return PicasaProvider.Session.GetAlbumPhotos(id, refreshCache, AdminPhotoSettings);
		}

		[CacheOutput(ClientTimeSpan = 3600, ServerTimeSpan = 3600)]
		public IEnumerable<AlbumPhoto> GetPhotos(bool refreshCache = false)
		{
			return PicasaProvider.Session.GetPhotos(refreshCache);
		}

		public void RefreshAlbum(string id)
		{
			var cache = Configuration.CacheOutputConfiguration().GetCacheOutputProvider(Request);
			
			cache.RemoveStartsWith(Configuration.CacheOutputConfiguration().MakeBaseCachekey((GdataPhotosController b) => b.GetAlbum(id, true)));
			cache.RemoveStartsWith(Configuration.CacheOutputConfiguration().MakeBaseCachekey((GdataPhotosController b) => b.GetAlbums()));
			cache.RemoveStartsWith(Configuration.CacheOutputConfiguration().MakeBaseCachekey((GdataPhotosController b) => b.GetAlbumPhotos(id, true)));
			cache.RemoveStartsWith(Configuration.CacheOutputConfiguration().MakeBaseCachekey((GdataPhotosController b) => b.GetPhotos(true)));

			//cache.RemoveStartsWith(Configuration.CacheOutputConfiguration().MakeBaseCachekey((ClientApiController b) => b.GetAlbum(id)));
			cache.RemoveStartsWith(Configuration.CacheOutputConfiguration().MakeBaseCachekey((ClientApiController b) => b.GetAlbums()));
			cache.RemoveStartsWith(Configuration.CacheOutputConfiguration().MakeBaseCachekey((ClientApiController b) => b.GetAlbumPhotos(id)));
			cache.RemoveStartsWith(Configuration.CacheOutputConfiguration().MakeBaseCachekey((ClientApiController b) => b.GetPhotos()));
		}

		// POST api/gdataphotos
		public void Post([FromBody]string value)
		{
		}

		// PUT api/gdataphotos/5
		public void Put(int id, [FromBody]string value)
		{
		}

		// DELETE api/gdataphotos/5
		public void Delete(int id)
		{
		}
	}
}
