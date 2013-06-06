using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Caching;
using System.Xml.Serialization;
using Google.GData.Client;
using Google.GData.Photos;
using Google.Picasa;
using cms.Controllers.Api;
using cms.data.EF;

namespace cms.Code.LinkAccounts.Picasa
{
	public class PicasaWrapper
	{
		private PicasaService Service { get; set; }
		private PicasaRequest PicasaRequest { get; set; }
		

		public PicasaWrapper(SessionProvider sessionProvider)
		{
			var oauth2ParametersStorage = OAuth2ParametersStorageFactory.StorageDatabase(sessionProvider);
			//var gdataAuth = new GoogleDataOAuth2Service(OAuth2ParametersStorageFactory.StorageJsonFile(ApplicationId));
			var gdataAuth = new GoogleDataOAuth2Service(oauth2ParametersStorage);
			var picasaFactory = new PicasaServiceFactory(gdataAuth.GetRequestDataFactoryInstance("https://picasaweb.google.com/data"));
			Service = picasaFactory.GetService();

			PicasaRequest = new PicasaRequest(new RequestSettings("x", gdataAuth.GetValidOAuth2Parameters()));

			ObjectCache cache = MemoryCache.Default;
//			var x = cache.Add();
//			cache.Get()
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

			a.SmallHeight = 400;

			var photos = PicasaRequest.GetPhotosInAlbum(id);

			return photos.Entries.Select(x => new PhotoDecorator(x, a));

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