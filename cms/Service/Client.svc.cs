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
		public GridPage GetGridpage(GetGridpageRq rq)
		{
			var db = new JsonDataEf(rq.ApplicationName);
			try
			{
				return db.GetGridPage(rq.Link);
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

		public GridPage GetGridpageJson(GetGridpageRq rq)
		{
			if (rq == null) throw new ArgumentNullException("GetGridpageRq");
			if (rq.Link == null) throw new ArgumentNullException("Link");
			if (rq.ApplicationName == null) throw new ArgumentNullException("ApplicationName");

			var db = new JsonDataEf(rq.ApplicationName);
			return db.GetGridPage(rq.Link);
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
