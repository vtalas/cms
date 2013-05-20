using Google.GData.Client;

namespace cms.Code.LinkAccounts
{
	public abstract class OAuth2ParametersStorageAbstract
	{
		public OAuth2Parameters Parameters { get; set; }

		protected OAuth2ParametersStorageAbstract()
		{
			Parameters = new OAuth2Parameters();
		}

		protected OAuth2ParametersStorageAbstract(OAuth2Parameters parameters)
		{
			Parameters = parameters;
		}

		public abstract void Save();
		public abstract void Load();
	}
}