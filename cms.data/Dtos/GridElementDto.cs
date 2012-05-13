using System.Runtime.Serialization;

namespace cms.data.Dtos
{
	[DataContract]
	public class GridElementDto 
	{

		[DataMember]
		public int Id { get; set; }
		
		[DataMember]
		public int Line { get; set; }
		
		[DataMember]
		public int Width { get; set; }
	
		[DataMember]
		public int Position { get; set; }

		[DataMember]
		public string Content { get; set; }

		[DataMember]
		public string Type { get; set; }
		
		[DataMember]
		public string Skin { get; set; }

	}
}