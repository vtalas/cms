using Google.Picasa;
using NUnit.Framework;
using cms.Code.LinkAccounts.Picasa;
using cms.Controllers.Api;

namespace cms.web.tests
{
	[TestFixture]
	public class PhotoDecoratorTest
	{
		[Test]
		public void sdkhskd()
		{
			var settings = new GdataPhotosSettings();
			var photo = new Photo
			{
				Width = 1200,
				Height = 1600,
			};


			var x = new PhotoDecorator(photo, settings);

			x.FullSize
		}
	}
}