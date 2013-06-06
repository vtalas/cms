using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
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
		private double Ratio { get; set; }
		private Orientation Orientation { get; set; }
		private int Height { get; set; }
		private int Width { get; set; }

		public IList<WebImage> Thumbnails { get; set; }
		public WebImage FullSize { get; set; }
		public WebImage Small { get; set; }
		public WebImage Medium { get; set; }
		public WebImage Large { get; set; }

		public PhotoDecorator(Photo photo, GdataPhotosSettings gdataPhotosSettings)
		{
			_settings = gdataPhotosSettings;
			
			Orientation = photo.Width > photo.Height ?  Orientation.Horizontal : Orientation.Vertical;
			Ratio = (double)photo.Width / photo.Height;
			Width = photo.Width;
			Height = photo.Height;

			LoadDefaultUri(photo);

			Thumbnails = new List<WebImage>(3)
			{
				 GetWebImage(95, 95),
				 GetWebImage(200, 200),
				 GetWebImage(350, 350)
			};

			Small = GetWebImageWithMaxSize(480, 300);
			Medium = GetWebImageWithMaxSize(768, 480);
			Large = GetWebImageWithMaxSize(1200, 768);
			FullSize = GetWebImage(photo.Width, photo.Height);
		}
			
		private WebImage GetWebImageWithMaxSize(int width, int height)
		{
			if (Orientation == Orientation.Horizontal)
			{
				return GetWebImage(width, (int)(width / Ratio));
			}
			return GetWebImage((int)(height * Ratio), height);
		}


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

		private WebImage GetWebImage(int maxsize)
		{
			var url = GetCorrectUrl(maxsize, 0);
			return new WebImage(maxsize, 0, url);
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

			return "w" + width;
		}

		private ExtensionCollection<MediaThumbnail> GetDefaultThumbs(Photo photo)
		{
			return photo.PicasaEntry.Media.Thumbnails;
		}

	}
}