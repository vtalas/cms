using System;
using System.ServiceModel;
using System.ServiceModel.Activation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using cms.data;
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
		public string GetGridpage(string application, string link)
		{
			try
			{
				return GetGridpageAction(application, link);
			}
			catch (Exception e)
			{

				var fault = new MyApplicationFault
				{
					Message = "ljkabsdjbasjkdbjkasd EEE"
				};
				throw new FaultException<MyApplicationFault>(fault);

			}

		}

		public string GetGridpageJson(string application, string link)
		{
			return GetGridpageAction(application, link);
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

		public string Ok()
		{
			return "xxx";
		}

		public string Fail()
		{
			var fault = new MyApplicationFault
			{
				Message = "ljkabsdjbasjkdbjkasd EEE"
			};
			throw new FaultException<MyApplicationFault>(fault);

			return "xkjbsjkabd";
		}
	}
}
