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
		private Uri _defaultUri;
		public Photo Photo { get; set; }

		public PhotoDecorator(Photo photo, GdataPhotosSettings gdataPhotosSettings)
		{
			_settings = gdataPhotosSettings;

			Photo = photo;
			LoadDefaultUri(Photo);

			var defaultThumbs = GetDefaultThumbs(photo);

			Small = GetWebImage(200, 200);
			Medium = GetWebImage(defaultThumbs[1]);
			Large = GetWebImage(400, 400);

		}

		public WebImage Small { get; set; }
		public WebImage Medium { get; set; }
		public WebImage Large { get; set; }


		private void LoadDefaultUri(Photo photo)
		{
			var x = photo.PicasaEntry.Media.Thumbnails[0];
			_defaultUri = new Uri(x.Url);
		}

		private WebImage GetWebImage(MediaThumbnail thumb)
		{
			return new WebImage(thumb.Width, thumb.Height, thumb.Url);
		}
		
		private WebImage GetWebImage(int width, int height)
		{
			var url = GetCorrectUrl(width, height);
			return new WebImage(width, height, url);
		}

		private string GetCorrectUrl(int width, int height)
		{
			var sizestring = SizeString(width, height);
			var b = new UriBuilder(_defaultUri);
			var parts = b.Path.Split('/');

			parts[parts.Length - 2] = sizestring;
			b.Path = String.Join("/", parts);

			return b.ToString();
		}

		private string SizeString(int width, int height)
		{
			if (width == height)
			{
				return "s" + width + "-c";
			}
	
			throw new NotImplementedException();
		}

		private ExtensionCollection<MediaThumbnail> GetDefaultThumbs(Photo photo)
		{
			return photo.PicasaEntry.Media.Thumbnails;
		}

	}
}