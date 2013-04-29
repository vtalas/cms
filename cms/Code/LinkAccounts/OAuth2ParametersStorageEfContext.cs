using Google.GData.Client;
using Newtonsoft.Json;
using cms.data.EF;
using cms.shared;

namespace cms.Code.LinkAccounts
{
	public class OAuth2ParametersStorageEfContext : OAuth2ParametersStorageAbstract
	{
		public IKeyValueStorage StorageProvider { get; set; }

		public OAuth2ParametersStorageEfContext(IKeyValueStorage storageProvider)
			: base()
		{
			StorageProvider = storageProvider;
		}

		public OAuth2ParametersStorageEfContext(OAuth2Parameters parameters, IKeyValueStorage storageProvider)
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
			StorageProvider.SettingsStorage("GdataPicasa.json", ToJson());
		}

		public override void Load()
		{
			var dataString = StorageProvider.SettingsStorage("GdataPicasa.json");
			Parameters = JsonConvert.DeserializeObject<OAuth2Parameters>(dataString);
		}
	}
}