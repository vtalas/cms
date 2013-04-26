using Google.GData.Client;
using Newtonsoft.Json;
using cms.data.EF;

namespace cms.Code.LinkAccounts
{
	public class OAuth2ParametersStorageEfContext : OAuth2ParametersStorageAbstract
	{
		public IXxx StorageProvider { get; set; }

		public OAuth2ParametersStorageEfContext(IXxx storageProvider) :base ()
		{
			StorageProvider = storageProvider;
		}

		public OAuth2ParametersStorageEfContext(OAuth2Parameters parameters, IXxx storageProvider)
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
			using (var x = StorageProvider.CreateKeyValueSession)
			{
				x.SettingsStorage("GdataPicasa.json", ToJson());
			}
		}

		public override void Load()
		{
			using (var x = StorageProvider.CreateKeyValueSession)
			{
				var dataString = x.SettingsStorage("GdataPicasa.json");
				Parameters = JsonConvert.DeserializeObject<OAuth2Parameters>(dataString);
			}
		}
	}
}