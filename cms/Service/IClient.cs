using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using cms.data.Dtos;

namespace cms.Service
{
	[ServiceContract]
	public interface IClient
	{
		[OperationContract]
		[WebGet(ResponseFormat = WebMessageFormat.Json)]
		GridPageDto GetGridpageJson(string ApplicationName ,string Link );

		[OperationContract]
		[WebGet(ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
		string EchoJson(string message);

		[OperationContract]
		string Echo();
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
