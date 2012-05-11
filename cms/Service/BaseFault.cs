using System.Runtime.Serialization;

namespace cms.Service
{
	[DataContract]
	public abstract class BaseFault
	{
		[DataMember]
		public string Message{get;set;}
	}
}