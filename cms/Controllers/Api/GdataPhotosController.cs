using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Http;
using Google.GData.Extensions.MediaRss;
using Google.Picasa;

namespace cms.Controllers.Api
{
	public class WebImage
	{
		public WebImage(string width, string height, string url)
		{
			Width = int.Parse(width, CultureInfo.CurrentCulture);
			Height = int.Parse(height, CultureInfo.CurrentCulture); ;
			Url = url;
		}

		public int Width { get; set; }
		public int Height { get; set; }
		public string Url { get; set; }
	}
	public class AlbumDecorator
	{
		public MediaThumbnail ThumbnailXXX { get; set; }
		public Album Album { get; set; }
		public AlbumDecorator(Album album)
		{
			Album = album;
			var xxx = Album.PicasaEntry.Media.Thumbnails[0];
			Thumbnail = new WebImage(xxx.Width, xxx.Height, xxx.Url);
			ThumbnailXXX = xxx;
		}

		public string Link { get { return Album.Title; } }
		public string Id { get { return Album.Id; } }
		public DateTime Updated { get { return Album.Updated; } }
		public WebImage Thumbnail { get; set; }
	}

	public class GdataPhotosController : WebApiPicasaControllerBase
	{
		public IEnumerable<AlbumDecorator> GetAlbums()
		{
			return Picasa.GetAlbums().Select(x => new AlbumDecorator(x)); ;
		}

		public AlbumDecorator GetAlbum(string link)
		{
			return Picasa.GetAlbum(link);
		}

		public IEnumerable<Photo> GetAlbumPhotos(string albumid)
		{
			return Picasa.GetAlbumPhotos(albumid);
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
