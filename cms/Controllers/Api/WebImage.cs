using System.Globalization;

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
}