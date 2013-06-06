using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace cms.Controllers.Api
{
	public class GdataPhotosController : WebApiPicasaControllerBase
	{
		public IDictionary<string, AlbumDecorator> GetAlbums()
		{
			var albums = PicasaProvider.Session.GetAlbums().Select(x => new AlbumDecorator(x));
			return albums.ToDictionary(key => key.Id);
		}

		public AlbumDecorator GetAlbum(string id)
		{
			return PicasaProvider.Session.GetAlbum(id);
		}

		public IEnumerable<PhotoDecorator> GetAlbumPhotos(string id)
		{
			return PicasaProvider.Session.GetAlbumPhotos(id);
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
