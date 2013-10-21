using System.Globalization;
using Google.Picasa;

namespace cms.Controllers.Api
{
	public class WebImage : IPhoto
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

		public WebImage(Photo photo)
		{
			Width = photo.Width;
			Height = photo.Height;
			PhotoUri = photo.PicasaEntry.Media.Thumbnails[0].Url;
		}

		public int Width { get; set; }
		public int Height { get; set; }
		public string PhotoUri { get; set; }
	}
}