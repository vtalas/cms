using cms.Controllers.Api;
using NUnit.Framework;

namespace cms.web.tests
{
	[TestFixture]
	public class ImageStringConvert_test
	{
		[Test]
		public void test()
		{
			var x = ImageStringConvert.Create(1900, 1200, ImageSizeType.MaxWidth, 100);


			Assert.AreEqual();

		}

	}
}