using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;
using cms.Code.Graphic;
using Google.GData.Extensions;
using Google.GData.Extensions.MediaRss;
using Google.Picasa;
using cms.Code.LinkAccounts.Picasa;

namespace cms.Controllers.Api
{
	public class PhotoDecorator
	{
		private Uri _defaultUri;
		private ImageStringConvert Convertor { get; set; }

		public IList<WebImage> Thumbnails { get; set; }
		public WebImage FullSize { get; set; }
		public WebImage Small { get; set; }
		public WebImage Medium { get; set; }
		public WebImage Large { get; set; }

		public PhotoDecorator(IPhoto photo, List<GdataPhotosSettings> gdataPhotosSettings)
		{
			var settings = gdataPhotosSettings ?? new List<GdataPhotosSettings>();
			Convertor = ImageStringConvert.Init(photo.Width, photo.Height);

			LoadDefaultUri(photo);

			var s1 = settings.ElementAtOrDefault(0) != null ? settings[0] : new GdataPhotosSettings(ImageSizeType.MaxSize, 95, true);
			var s2 = settings.ElementAtOrDefault(1) != null ? settings[1] : new GdataPhotosSettings(ImageSizeType.MaxSize, 230, true);
			var s3 = settings.ElementAtOrDefault(2) != null ? settings[2] : new GdataPhotosSettings(ImageSizeType.MaxHeight, 480, true);

			Thumbnails = new List<WebImage>(3)
			{
				GetWebImage(Convertor.Convert(s1)),
				GetWebImage(Convertor.Convert(s2)),
				GetWebImage(Convertor.Convert(s3))
			};

			Small = GetWebImageWithMaxSize(480, 300);
			Medium = GetWebImageWithMaxSize(768, 480);
			Large = GetWebImageWithMaxSize(1200, 768);
			
			FullSize = GetWebImage(Convertor.Convert(ImageSizeType.MaxWidth, photo.Width));
		}


		private WebImage GetWebImageWithMaxSize(int width, int height)
		{
			var isHorizontal = Convertor.Orientation == Orientation.Horizontal;
			return GetWebImage(isHorizontal ? Convertor.Convert(ImageSizeType.MaxWidth, width) : Convertor.Convert(ImageSizeType.MaxHeight, height));
		}

		private void LoadDefaultUri(IPhoto photo)
		{
			//var x = photo.PicasaEntry.Media.Thumbnails[0];
			_defaultUri = new Uri(photo.PhotoUri);
		}

		private WebImage GetWebImage(ImageStringConvert x)
		{
			var url = ReplaceSizeDefinition(x.DimensionString);
			return new WebImage(x.Width, x.Height, url);
		}

		private string ReplaceSizeDefinition(string sizestring)
		{
			var b = new UriBuilder(_defaultUri);
			var parts = b.Path.Split('/');

			parts[parts.Length - 2] = sizestring;
			b.Path = String.Join("/", parts);

			return b.ToString();
		}

		private ExtensionCollection<MediaThumbnail> GetDefaultThumbs(Photo photo)
		{
			return photo.PicasaEntry.Media.Thumbnails;
		}

	}
}