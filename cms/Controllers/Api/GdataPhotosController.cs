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
			PhotoUri = url;
		}
		public WebImage(int width, int height, string url)
		{
			Width = width;
			Height = height;
			PhotoUri = url;
		}

		public int Width { get; set; }
		public int Height { get; set; }
		public string PhotoUri { get; set; }
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

	public class PhotoDecorator
	{
		public Photo Photo { get; set; }

		public PhotoDecorator(Photo photo)
		{
			Photo = photo;
			Small = new WebImage(100, 100, null);
			Medium = new WebImage(100, 100, null);
			Large = new WebImage(100, 100, null);
		}

		public WebImage Small { get; set; }
		public WebImage Medium { get; set; }
		public WebImage Large { get; set; }
	}


	public class GdataPhotosController : WebApiPicasaControllerBase
	{
		public IDictionary<string, AlbumDecorator> GetAlbums()
		{
			var albums = Picasa.GetAlbums().Select(x => new AlbumDecorator(x));
			return albums.ToDictionary(key => key.Id);
		}

		public AlbumDecorator GetAlbum(string id)
		{
			
			return Picasa.GetAlbum(id);
		}

		public IEnumerable<PhotoDecorator> GetAlbumPhotos(string id)
		{
			using (var settings = SessionProvider.CreateKeValueSession)
			{
			}

			return Picasa.GetAlbumPhotos(id);
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
