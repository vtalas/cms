using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace cms.Service
{
	public class JsonErrorHandler : IErrorHandler
	{
		public bool HandleError(Exception error)
		{
			return true;
		}

		public void ProvideFault(Exception error, MessageVersion version,ref Message fault)
		{
			fault = this.GetJsonFaultMessage(version, error);

			this.ApplyJsonSettings(ref fault);
			this.ApplyHttpResponseSettings(ref fault, System.Net.HttpStatusCode.BadRequest, "status desrckslndf");
		}

		#region Protected Method(s)
		protected virtual void ApplyJsonSettings(ref Message fault)
		{
			// Use JSON encoding  
			var jsonFormatting =
				new WebBodyFormatMessageProperty(WebContentFormat.Json);
			fault.Properties.Add(WebBodyFormatMessageProperty.Name, jsonFormatting);
		}

		protected virtual void ApplyHttpResponseSettings(ref Message fault, System.Net.HttpStatusCode statusCode,string statusDescription)
		{
			var httpResponse = new HttpResponseMessageProperty()
			                   	{
			                   		StatusCode = statusCode,
			                   		StatusDescription = statusDescription
			                   	};
			fault.Properties.Add(HttpResponseMessageProperty.Name, httpResponse);
		}

		protected virtual Message GetJsonFaultMessage(MessageVersion version, Exception error)
		{
			BaseFault detail = null;
			var knownTypes = new List<Type>();
			string faultType = error.GetType().Name; //default  

			if ((error is FaultException) &&
			    (error.GetType().GetProperty("Detail") != null))
			{
				detail =
					(error.GetType().GetProperty("Detail").GetGetMethod().Invoke(
						error, null) as BaseFault);
				knownTypes.Add(detail.GetType());
				faultType = detail.GetType().Name;
			}

			var  jsonFault = new JsonFault
			                 	{
			                 		Message = error.Message,
			                 		Detail = detail,
			                 		FaultType = faultType
			                 	};

			var faultMessage = Message.CreateMessage(version, "", jsonFault,
			                                         new DataContractJsonSerializer(jsonFault.GetType(), knownTypes));

			return faultMessage;
		}
		#endregion
	}
}