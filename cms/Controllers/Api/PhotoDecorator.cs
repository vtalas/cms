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
			Small = new WebImage(_settings.SmallWidth, _settings.SmallHeight, null);
			Medium = new WebImage(_settings.MediumWidth, _settings.MediumHeight, null);
			Large = new WebImage(_settings.LargeWidth, _settings.LargeHeight, null);
		}

		public WebImage Small { get; set; }
		public WebImage Medium { get; set; }
		public WebImage Large { get; set; }
	}
}