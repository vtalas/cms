using System;
using System.Configuration;
using Google.GData.Client;
using cms.Code.UserResources;
using cms.data.EF;

namespace cms.Code.LinkAccounts
{
	public class OAuth2ParametersStorageFactory
	{
		private static readonly string ClientId = ConfigurationManager.AppSettings["Gdata_ClientId"];
		private static readonly string ClientSecret = ConfigurationManager.AppSettings["Gdata_ClientSecret"];

		private static OAuth2Parameters GetDefaults()
		{
			return new OAuth2Parameters()
			{
				ClientId = ClientId,
				ClientSecret = ClientSecret,
				Scope = "https://picasaweb.google.com/data",
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