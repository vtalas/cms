using System.Web.UI.WebControls;

namespace cms.Controllers.Api
{
	public enum ImageSizeType
	{
		MaxWidth,
		MaxHeight,
		MaxSize
	}
	
	public class ImageStringConvert
	{
		private readonly int _width;
		private readonly int _height;
		private readonly ImageSizeType _type;
		private readonly int _value;
		private readonly bool _square;

		private double Ratio { get; set; }
		private Orientation Orientation { get; set; }

		private ImageStringConvert(int width, int height, ImageSizeType type, int value, bool square)
		{
			_width = width;
			_height = height;
			_type = type;
			_value = value;
			_square = square;

			Orientation = _width > _height ? Orientation.Horizontal : Orientation.Vertical;
			Ratio = (double)_width / _height;
		}

		public static ImageStringConvert Create(int width, int height, ImageSizeType type, int value, bool isSquare = false)
		{
			var instance =  new ImageStringConvert(width, height, type,value, isSquare);
			return instance;
		}


		public int Width
		{
			get
			{
				return 0;
			}
		}
		
		public int Height
		{
			get
			{
				return 0;
			}
		}

		public string DimensionString
		{
			get
			{
				return 
			}
		}

	}
}