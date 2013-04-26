using System;
using Google.GData.Extensions;
using Google.GData.Extensions.MediaRss;
using Google.Picasa;
using cms.Code.LinkAccounts.Picasa;

namespace cms.Controllers.Api
{
	public class PhotoDecorator
	{
		private readonly GdataPhotosSettings _settings;
		public Photo Photo { get; set; }

		public PhotoDecorator(Photo photo, GdataPhotosSettings gdataPhotosSettings)
		{
			_settings = gdataPhotosSettings;

			Photo = photo;
			var defaultThumbs = GetDefaultThumbs(photo);

			Small = GetWebImage(defaultThumbs[0]);
			Medium = GetWebImage(defaultThumbs[1]);
			Large = GetWebImage(defaultThumbs[2]);
		}

		public WebImage Small { get; set; }
		public WebImage Medium { get; set; }
		public WebImage Large { get; set; }


		private Uri GetDefaultUri(Photo photo)
		{
			var x = photo.PicasaEntry.Media.Thumbnails[0];
			return new Uri(x.Url);
		}

		private WebImage GetWebImage(MediaThumbnail thumb)
		{
			return new WebImage(thumb.Width, thumb.Height, thumb.Url);
		}

		private ExtensionCollection<MediaThumbnail> GetDefaultThumbs(Photo photo)
		{
			return photo.PicasaEntry.Media.Thumbnails;
		}

	}
}