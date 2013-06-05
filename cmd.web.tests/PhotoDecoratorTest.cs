using Google.GData.Photos;
using Google.Picasa;
using Moq;
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
			var mock = new Mock<PicasaEntry>();
//			mock.Setup(x => x.Media.);

			var x = new PhotoDecorator(photo, settings);



		}
	}
}