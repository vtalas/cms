using System;
using Google.GData.Extensions.MediaRss;
using Google.Picasa;

namespace cms.Controllers.Api
{
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
}