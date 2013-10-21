using System.Collections.Generic;
using cms.Code.LinkAccounts.Picasa;

namespace cms.Controllers.Api
{
	public class AlbumPhoto : PhotoDecorator
	{
		public string AlbumId { get; set; }
		
		public AlbumPhoto(IPhoto photo, List<GdataPhotosSettings> gdataPhotosSettings, string albumId) : base(photo, gdataPhotosSettings)
		{
			AlbumId = albumId;
		}
	}
}