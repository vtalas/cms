using System;
using System.Collections.Generic;
using cms.Code.LinkAccounts.Picasa;
using Google.Picasa;

namespace cms.Controllers.Api
{
	public class AlbumDecorator
	{
		public Album Album { get; set; }
		public AlbumDecorator(Album album, List<GdataPhotosSettings> settings)
		{
			Album = album;
			var image = Album.PicasaEntry.Media.Thumbnails[0];
			Thumbnail = new PhotoDecorator(new WebImage(image.Width, image.Height, image.Url), settings);
		}
		public string Title { get { return Album.Title; } }
		public string Link { get { return Album.Title; } }
		public string Id { get { return Album.Id; } }
		public DateTime Updated { get { return Album.Updated; } }	
		public PhotoDecorator Thumbnail { get; set; }
	}
}