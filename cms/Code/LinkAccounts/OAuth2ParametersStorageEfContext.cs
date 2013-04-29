using Google.GData.Client;
using Newtonsoft.Json;
using cms.data.EF;
using cms.shared;

namespace cms.Code.LinkAccounts
{
	public class OAuth2ParametersStorageEfContext : OAuth2ParametersStorageAbstract
	{
		public SessionProvider StorageProvider { get; set; }

		public OAuth2ParametersStorageEfContext(SessionProvider storageProvider)
			: base()
		{
			StorageProvider = storageProvider;
		}

		public OAuth2ParametersStorageEfContext(OAuth2Parameters parameters, SessionProvider storageProvider)
			: base(parameters)
		{
			StorageProvider = storageProvider;
		}

		public string ToJson()
		{
			return JsonConvert.SerializeObject(Parameters);
		}

		public override void Save()
		{
			using (var db = StorageProvider.CreateSession())
			{
				db.Session.Settings.SettingsStorage("GdataPicasa.json", ToJson());
			}
		}

		public override void Load()
		{
			using (var db = StorageProvider.CreateSession())
			{
				var dataString = db.Session.Settings.SettingsStorage("GdataPicasa.json");
				Parameters = JsonConvert.DeserializeObject<OAuth2Parameters>(dataString);
			}
		}
	}
}