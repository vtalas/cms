using System;
using System.Collections.Generic;
using System.Linq;
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
		private List<GdataPhotosSettings> Settings { get; set; }

		public PicasaWrapper(SessionProvider sessionProvider)
		{
			var oauth2ParametersStorage = OAuth2ParametersStorageFactory.StorageDatabase(sessionProvider);
			//var gdataAuth = new GoogleDataOAuth2Service(OAuth2ParametersStorageFactory.StorageJsonFile(ApplicationId));
			var gdataAuth = new GoogleDataOAuth2Service(oauth2ParametersStorage);
			var picasaFactory = new PicasaServiceFactory(gdataAuth.GetRequestDataFactoryInstance("https://picasaweb.google.com/data"));
			Service = picasaFactory.GetService();

			Settings = new SettingsLoader<List<GdataPhotosSettings>>(sessionProvider, "GdataPhotosSettings_List.json").Get();

			PicasaRequest = new PicasaRequest(new RequestSettings("x", gdataAuth.GetValidOAuth2Parameters()));
		}

		public IEnumerable<Album> GetAlbums()
		{
			var albums = PicasaRequest.GetAlbums();
			return albums.Entries;
		}

		public AlbumDecorator GetAlbum(string id, bool refreshCache = false)
		{
			return GetAlbum(id, Settings, refreshCache);
		}

		public AlbumDecorator GetAlbum(string id, List<GdataPhotosSettings> settings, bool refreshCache = false)
		{
			var albums = PicasaRequest.GetAlbums();
			var albumobject = albums.Entries.SingleOrDefault(x => x.Id == id);
			var decorate = new AlbumDecorator(albumobject, settings);
			return decorate;
		}

		public IEnumerable<PhotoDecorator> GetAlbumPhotos(string id, bool refreshCache = false)
		{
			return GetAlbumPhotos(id, refreshCache, Settings);
		}

		public IEnumerable<AlbumPhoto> GetPhotos(bool refreshCache = false)
		{
			var response = PicasaRequest.GetPhotos().Entries.Select(x => new AlbumPhoto(new WebImage(x), Settings, x.AlbumId));
			return response;
		}

		public IEnumerable<PhotoDecorator> GetAlbumPhotos(string id, bool refreshCache, List<GdataPhotosSettings> photoSettings)
		{
			var photos = PicasaRequest.GetPhotosInAlbum(id);
			var response = photos.Entries.Select(x => new PhotoDecorator(new WebImage(x), photoSettings));
			return response;
		}
	}
}