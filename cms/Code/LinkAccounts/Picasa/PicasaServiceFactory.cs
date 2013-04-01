using Google.GData.Client;
using Google.GData.Photos;

namespace cms.Code.LinkAccounts.Picasa
{
	public class PicasaServiceFactory
	{
		public IGDataRequestFactory RequestFactory { get; set; }

		public PicasaServiceFactory(IGDataRequestFactory requestFactory)
		{
			RequestFactory = requestFactory;
		}

		public PicasaService GetService()
		{
			return new PicasaService("appname") { RequestFactory = RequestFactory };
		}
	}
}