using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using Newtonsoft.Json.Linq;
using cms.data;

namespace cms.Service
{
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IClient" in both code and config file together.
	[ServiceContract]
	public interface IClient
	{
		[OperationContract]
		//[WebInvoke(Method = "POST",RequestFormat = WebMessageFormat.Json,BodyStyle = WebMessageBodyStyle.Wrapped )]
		//[WebGet(BodyStyle = WebMessageBodyStyle.Wrapped )]
		[FaultContract(typeof (MyApplicationFault))]
		GridPage GetGridpage(GetGridpageRq rq);

		[OperationContract]
		[WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
		GridPage GetGridpageJson(GetGridpageRq rq);

		[OperationContract]
		[WebInvoke(Method = "GET",ResponseFormat = WebMessageFormat.Json,RequestFormat = WebMessageFormat.Json,
			BodyStyle = WebMessageBodyStyle.WrappedRequest)]
		string Echo();

		[OperationContract]
		string Ok();

		[OperationContract]
		string Fail();
	}

	[DataContract]
	public class GetGridpageRq
	{
		[DataMember]
		public string ApplicationName { get; set; }

		[DataMember]
		public string Link { get; set; }
	}

	public class MyApplicationFault
	{
		public string Message { get; set; }
	}

	public class JsonErrorWebHttpBehavior : WebHttpBehavior
	{
		protected override void AddServerErrorHandlers(ServiceEndpoint endpoint,System.ServiceModel.Dispatcher.EndpointDispatcher endpointDispatcher)
		{
			// clear default error handlers.  
			endpointDispatcher.ChannelDispatcher.ErrorHandlers.Clear();
			// add the Json error handler.  
			endpointDispatcher.ChannelDispatcher.ErrorHandlers.Add(new JsonErrorHandler());
		}
	}
	public class JsonErrorWebHttpBehaviorElement : BehaviorExtensionElement
	{
		public override Type BehaviorType
		{
			get
			{
				return typeof(JsonErrorWebHttpBehavior);
			}
		}

		protected override object CreateBehavior()
		{
			return new JsonErrorWebHttpBehavior();
		}
	}  
}
