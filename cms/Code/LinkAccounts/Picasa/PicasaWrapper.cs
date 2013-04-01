using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Google.GData.Client;
using Google.GData.Photos;
using Google.Picasa;
using WebGrease.Css.Extensions;
using cms.Controllers.Api;

namespace cms.Code.LinkAccounts.Picasa
{
	public class PicasaWrapper
	{
		private PicasaRequest PicasaRequest { get; set; }
		public PicasaService Service { get; set; }

		public PicasaWrapper(PicasaService service, OAuth2Parameters parameters)
		{
			Service = service;
			var settings = new RequestSettings("x", parameters);
			PicasaRequest = new PicasaRequest(settings);
		}

		public IEnumerable<Album> GetAlbums()
		{
			var albums = PicasaRequest.GetAlbums();
			return albums.Entries;
		}

		public AlbumDecorator GetAlbum(string link)
		{
			var albums = PicasaRequest.GetAlbums();
			var xx = albums.Entries.SingleOrDefault(x => x.Title == link);

			return new AlbumDecorator(xx);
		}

		public IEnumerable<Photo> GetAlbumPhotos(string id)
		{
			var x = PicasaRequest.GetPhotosInAlbum(id);
			return x.Entries;
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