using Google.GData.Extensions.MediaRss;
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
		  public class Photox : PicasaEntity
		  {
			  protected override void EnsureInnerObject()
			  {
				  throw new System.NotImplementedException();
			  }
		  }

		[Test]
		public void sdkhskd()
		{
			var settings = new GdataPhotosSettings();
			var photo = new Photo
			{
				Width = 1200,
				Height = 1600,
			};
//			var mockThumb = new Mock<MediaThumbnail>();
//			//mockThumb.Setup(x => x.Url).Returns("https://lh4.googleusercontent.com:443/sd/jbjsb/AM/3VY/w1024/029.jpg");
//		
//			var mock = new Mock<PicasaEntry>();
//			mock.Setup(x => x.Media).Returns(null);
//				
//			var mockxx = new Mock<Photo>();
//				mockxx.Setup(x => x.PicasaEntry).Returns(mock.Object);
//
//			var ax = new PhotoDecorator(mockxx.Object, settings);
//


		}
	}
}