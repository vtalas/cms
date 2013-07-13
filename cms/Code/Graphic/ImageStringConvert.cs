using System.Web.UI.WebControls;

namespace cms.Code.Graphic
{
	public class ImageStringConvert
	{
		private readonly int _width;
		private readonly int _height;
		private ImageSizeType _type;
		private int _value;
		private bool _square;

		private double Ratio { get; set; }
		public Orientation Orientation { get; private  set; }

		private ImageStringConvert(int width, int height)
		{
			_width = width;
			_height = height;

			Orientation = _width > _height ? Orientation.Horizontal : Orientation.Vertical;
			Ratio = (double)_width / _height;
		}

		public ImageStringConvert Convert(ImageSizeType type, int value, bool isSquare = false)
		{
			_type = type;
			_value = value;
			_square = isSquare;

			return this;
		}

		public ImageStringConvert ConvertToSquare(int value)
		{
			_type = ImageSizeType.MaxWidth;
			_value = value;
			_square = true;

			return this;
		}

		public static ImageStringConvert Init(int width, int height)
		{
			var instance = new ImageStringConvert(width, height);
			return instance;
		}

		public int Width
		{
			get
			{
				switch (_type)
				{
					case ImageSizeType.MaxWidth:
						return _value;
					case ImageSizeType.MaxHeight:
						return _square ? _value : (int) (_value*Ratio);
					case ImageSizeType.MaxSize:
						return Orientation == Orientation.Horizontal || _square ? _value : (int)(_value * Ratio);
					default:
						return _value;
				}
			}
		}

		public int Height
		{
			get
			{
				switch (_type)
				{
					case ImageSizeType.MaxWidth:
						return _square ? _value : (int)(_value / Ratio);
					case ImageSizeType.MaxHeight:
						return _value;
					case ImageSizeType.MaxSize:
						return Orientation == Orientation.Vertical || _square ? _value : (int)(_value / Ratio);
					default:
						return _value;
				}
			}
		}

		public string DimensionString
		{
			get
			{
				return _type.ToSymbol() + _value + (_square ? "-c" : "");
			}
		}

	}
}