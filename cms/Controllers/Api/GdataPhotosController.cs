using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Google.Picasa;

namespace cms.Controllers.Api
{
	public class GdataPhotosController : WebApiPicasaControllerBase
	{
		public IDictionary<string, AlbumDecorator> GetAlbums()
		{
			var albums = Enumerable.Select<Album, AlbumDecorator>(PicasaProvider.Session.GetAlbums(), x => new AlbumDecorator(x));
			return albums.ToDictionary(key => key.Id);
		}

		public AlbumDecorator GetAlbum(string id, bool refreshCache = false)
		{
			return PicasaProvider.Session.GetAlbum(id, refreshCache);
		}

		public IEnumerable<PhotoDecorator> GetAlbumPhotos(string id, bool refreshCache = false)
		{
			return PicasaProvider.Session.GetAlbumPhotos(id, refreshCache);
		}

		public IEnumerable<AlbumPhoto> GetPhotos(bool refreshCache = false)
		{
			return PicasaProvider.Session.GetPhotos(refreshCache);
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
