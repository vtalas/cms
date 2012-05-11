using System.Runtime.Serialization;

namespace cms.Service
{
	[DataContract]
	public class JsonFault: BaseFault
	{
		[DataMember]
		public BaseFault Detail{get;set;}

		[DataMember]
		public string FaultType{get;set;}

	}
}