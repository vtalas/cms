using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Google.GData.Client;
using Google.GData.Photos;
using Google.Picasa;
using WebGrease.Css.Extensions;
using cms.Controllers.Api;
using cms.data.EF;

namespace cms.Code.LinkAccounts.Picasa
{
	public class PicasaWrapper
	{
		private readonly SessionProvider _sessionProvider;
		private PicasaRequest PicasaRequest { get; set; }
		public PicasaService Service { get; set; }

		public PicasaWrapper(PicasaService service, OAuth2Parameters parameters, SessionProvider sessionProvider)
		{
			_sessionProvider = sessionProvider;
			Service = service;
			var settings = new RequestSettings("x", parameters);
			PicasaRequest = new PicasaRequest(settings);
		}

		public IEnumerable<Album> GetAlbums()
		{
			var albums = PicasaRequest.GetAlbums();
			return albums.Entries;
		}

		public AlbumDecorator GetAlbum(string id)
		{
			var albums = PicasaRequest.GetAlbums();
			
			var xx = albums.Entries.SingleOrDefault(x => x.Id == id);
			return new AlbumDecorator(xx);
		}

		public IEnumerable<PhotoDecorator> GetAlbumPhotos(string id)
		{
			var a = new GdataPhotosSettings();
	
			using (var db = _sessionProvider.CreateKeyValueSession)
			{
				//db.SettingsStorage()
			}
			

			var photos = PicasaRequest.GetPhotosInAlbum(id);

			return  photos.Entries.Select(x => new PhotoDecorator(x, a));

		}
	}

	public static class ccc
	{
		public static string SerializeObject<T>(this T toSerialize)
		{
			var xmlSerializer = new XmlSerializer(toSerialize.GetType());
			var textWriter = new StringWriter();

			xmlSerializer.Serialize(textWriter, toSerialize);
			return textWriter.ToString();
		}
	} 
}