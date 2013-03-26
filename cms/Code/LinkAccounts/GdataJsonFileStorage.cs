using System;
using Google.GData.Client;
using Newtonsoft.Json;
using cms.Code.UserResources;

namespace cms.Code.LinkAccounts
{
	public class GdataJsonFileStorage : IGdataStorage
	{
		private GdataJsonFileStorage Source { get; set; }
		public IKeyValueStorage Storage { get; set; }

		public GdataJsonFileStorage(IKeyValueStorage storage)
		{
			Storage = storage;
			Data = new OAuth2Parameters();
			Source = JsonConvert.DeserializeObject<GdataJsonFileStorage>(Storage.SettingsStorage("GdataPicasa.json"));
		}

		public OAuth2Parameters Data { get; set; }
		public string ClientId { get; set; }
		public string ClientSecret { get; set; }
		public string AccessToken { get; set; }
		public string AccessCode { get; set; }
		
		public void Save()
		{
			Storage.SettingsStorage("GdataPicasa.json", JsonConvert.SerializeObject(this));
		}

		public string RefreshToken { get; set; }
		public DateTime TokenExpiry { get; set; }
		public string RedirectUrl { get; set; }

	}
}