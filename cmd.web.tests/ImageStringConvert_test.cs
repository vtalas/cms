using System;
using System.Web.UI.WebControls;
using cms.Code.Graphic;
using NUnit.Framework;

namespace cms.web.tests
{
	[TestFixture]
	public class ImageStringConvert_test
	{
		private void log(ImageStringConvert x)
		{
			Console.WriteLine("{0}, {1}  {2} --- {3}", x.Width, x.Height, x.DimensionString, x.Orientation);
		}
		[Test]
		public void Test_with_width_horizontal()
		{
			var x = ImageStringConvert.Init(1900, 1200).Convert( ImageSizeType.MaxWidth, 100);
			log(x);
			Assert.AreEqual("w100", x.DimensionString);
			Assert.AreEqual(100, x.Width);
			Assert.AreEqual(x.Orientation, Orientation.Horizontal);
			Assert.IsTrue(x.Height < x.Width) ;

		}
		[Test]
		public void Test_with_width_vertical()
		{
			var x = ImageStringConvert.Init(1200, 1900).Convert(ImageSizeType.MaxWidth, 100);
			log(x);
			Assert.AreEqual("w100", x.DimensionString);
			Assert.AreEqual(x.Orientation, Orientation.Vertical);
			Assert.AreEqual(100, x.Width);
			Assert.IsTrue(x.Height > x.Width) ;
		}
		[Test]
		public void Test_with_width_square()
		{
			var x = ImageStringConvert.Init(1200, 1900).Convert(ImageSizeType.MaxWidth, 100, true);
			log(x);
			Assert.AreEqual("w100-c", x.DimensionString);
			Assert.AreEqual(100, x.Width);
			Assert.IsTrue(x.Height == x.Width) ;
		}

		[Test]
		public void Test_with_height_horizontal()
		{
			const ImageSizeType type = ImageSizeType.MaxHeight;

			var x = ImageStringConvert.Init(1900, 1200).Convert(type, 100);
			log(x);
			Assert.AreEqual("h100", x.DimensionString);
			Assert.AreEqual(x.Orientation, Orientation.Horizontal);
			Assert.AreEqual(100, x.Height);
			Assert.IsTrue(x.Height < x.Width);
		}

		[Test]
		public void Test_with_height_vertical()
		{
			const ImageSizeType type = ImageSizeType.MaxHeight;
			var x = ImageStringConvert.Init(1200, 1900).Convert(type, 100);
			log(x);
			Assert.AreEqual(x.Orientation, Orientation.Vertical);
			Assert.AreEqual("h100", x.DimensionString);
			Assert.AreEqual(100, x.Height);
			Assert.IsTrue(x.Height > x.Width) ;

		}
		[Test]
		public void Test_with_height_square()
		{
			const ImageSizeType type = ImageSizeType.MaxHeight;
			var x = ImageStringConvert.Init(1200, 1900).Convert(type, 100, true);
			log(x);
			Assert.AreEqual("h100-c", x.DimensionString);
			Assert.AreEqual(100, x.Height);
			Assert.IsTrue(x.Height == x.Width) ;
		}

		[Test]
		public void Test_with_size_horizontal()
		{
			const ImageSizeType type = ImageSizeType.MaxSize;

			var x = ImageStringConvert.Init(1900, 1200).Convert(type, 100);
			log(x);
			Assert.AreEqual(x.Orientation, Orientation.Horizontal);
			Assert.AreEqual("s100", x.DimensionString);
			Assert.AreEqual(100, x.Width);
			Assert.IsTrue(x.Height < x.Width) ;
		}
		
		[Test]
		public void Test_with_size_veritacal()
		{
			const ImageSizeType type = ImageSizeType.MaxSize;

			var x = ImageStringConvert.Init(1200, 1900).Convert(type, 100);
			log(x);
			Assert.AreEqual("s100", x.DimensionString);
			Assert.AreEqual(x.Orientation, Orientation.Vertical);
			Assert.AreEqual(100, x.Height);
			Assert.IsTrue(x.Height > x.Width) ;
		}
	
		[Test]
		public void Test_with_size_square()
		{
			const ImageSizeType type = ImageSizeType.MaxSize;

			var x = ImageStringConvert.Init(1200, 1900).Convert(type, 100, true);
			log(x);
			Assert.AreEqual("s100-c", x.DimensionString);
			Assert.AreEqual(100, x.Width);
			Assert.IsTrue(x.Height == x.Width) ;
		}

	}
}