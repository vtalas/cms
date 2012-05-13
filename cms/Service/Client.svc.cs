using System;
using System.ServiceModel;
using System.ServiceModel.Activation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using cms.data;
using cms.data.Dtos;
using cms.data.EF;

namespace cms.Service
{
	[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
	public class Client : IClient
	{
		public string GetGridpageAction(string application, string link)
		{
			var db = new JsonDataEf(application);
			var data = db.GetGridPage(link);

			var settings = new JsonSerializerSettings()
			{
				ReferenceLoopHandling = ReferenceLoopHandling.Ignore
			};
			var ret = JsonConvert.SerializeObject(data, settings);

			return ret;
		}

		public GridPageDto GetGridpageJson(string ApplicationName, string Link)
		{
			if (Link == null) throw new ArgumentNullException("Link");
			if (ApplicationName == null) throw new ArgumentNullException("ApplicationName");

			var db = new JsonDataEf(ApplicationName);
			return db.GetGridPage(Link);
		}
		
		public string EchoJson(string message)
		{
			return "xxx" + message;
		}

		public string Echo()
		{
			//var fault = new MyApplicationFault
			//{
			//    Message = "ljkabsdjbasjkdbjkasd EEE"
			//};
			//throw new FaultException<MyApplicationFault>(fault);

			return "xkjbsjkabd";
		}

	}
}
