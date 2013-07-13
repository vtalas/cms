namespace cms.Code.Graphic
{
	public static class ImageSizeTypeExtension
	{
		public static string ToSymbol(this ImageSizeType source)
		{
			switch (source)
			{
				case ImageSizeType.MaxHeight:
					return "h";
				case ImageSizeType.MaxWidth:
					return "w";
				case ImageSizeType.MaxSize:
					return "s";
			}
			return "w";
		}
	}
}