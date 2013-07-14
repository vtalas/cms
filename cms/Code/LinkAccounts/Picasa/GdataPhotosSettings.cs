using cms.Code.Graphic;

namespace cms.Code.LinkAccounts.Picasa
{
	public class GdataPhotosSettings
	{
		public GdataPhotosSettings(ImageSizeType type, int value, bool isSquare)
		{
			Type = type;
			Value = value;
			IsSquare = isSquare;
		}

		public ImageSizeType Type { get; set; }
		public int Value { get; set; }
		public bool IsSquare { get; set; }
	}

}