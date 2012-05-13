using System.Collections.Generic;
using System.Runtime.Serialization;

namespace cms.data.Dtos
{
	[DataContract]
	public class GridPageDto
	{
		[DataMember]
		public int Id { get; set; }

		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public string Link { get; set; }

		[DataMember]
		public bool Home { get; set; }

		[DataMember]
		public IList<IEnumerable<GridElementDto>> Lines{get; set; }
	}
}