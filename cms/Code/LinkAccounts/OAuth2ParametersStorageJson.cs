﻿using Google.GData.Client;
using Newtonsoft.Json;
using cms.shared;

namespace cms.Code.LinkAccounts
{
	public class OAuth2ParametersStorageJson : OAuth2ParametersStorageAbstract
	{
		private IKeyValueStorage Storage { get; set; }

		public OAuth2ParametersStorageJson(IKeyValueStorage storage) :base ()
		{
			Storage = storage;
		}

		public OAuth2ParametersStorageJson(OAuth2Parameters parameters, IKeyValueStorage storage) :base(parameters)
		{
			Storage = storage;
		}

		public string ToJson()
		{
			return JsonConvert.SerializeObject(Parameters);
		}

		public override void Save()
		{
			Storage.SettingsStorage("GdataPicasa.json", ToJson());
		}

		public override void Load()
		{
			var dataString = Storage.SettingsStorage("GdataPicasa.json");
			Parameters = JsonConvert.DeserializeObject<OAuth2Parameters>(dataString);
		}
	}
}