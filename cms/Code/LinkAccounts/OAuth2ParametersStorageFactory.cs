using System;
using Google.GData.Client;
using cms.Code.UserResources;
using cms.data.EF;
using cms.shared;

namespace cms.Code.LinkAccounts
{
	public class OAuth2ParametersStorageFactory
	{
		private const string ClientId = "568637062174-4s9b1pohf5p0hkk8o4frvesnhj8na7ug.apps.googleusercontent.com";
		private const string ClientSecret = "YxM0MTKCTSBV5vGlU05Yj63h";

		private static OAuth2Parameters GetDefaults()
		{
			return new OAuth2Parameters()
			{
				ClientId = ClientId,
				ClientSecret = ClientSecret,
				Scope = "https://picasaweb.google.com/data",
				RedirectUri = "http://localhost:62728/api/c78ee05e-1115-480b-9ab7-a3ab3c0f6643/GData/Authenticate"
			};
		}

		public static OAuth2ParametersStorageAbstract StorageJsonFile(Guid applicationId)
		{
			var resources = UserResourcesManagerProvider.GetApplication(applicationId);

			var gg = new OAuth2ParametersStorageJson(resources);
			gg.Load();

			if (gg.Parameters == null)
			{
				gg.Parameters = GetDefaults();
			}
			return gg;
		}

		public static OAuth2ParametersStorageAbstract StorageDatabase(SessionProvider sessionProvider)
		{
			var efContextStorage = new OAuth2ParametersStorageEfContext(sessionProvider);
			efContextStorage.Load();

			if (efContextStorage.Parameters == null)
			{
				efContextStorage.Parameters = GetDefaults();
			}
			return efContextStorage;
		}

	}
}