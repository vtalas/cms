using System;
using System.Collections.Generic;

namespace cms.data.Shared.Models
{
	public class Resource : IEntity
	{
		public int Id { get; set; }
		public string Culture { get; set; }

		//TODO: required dodelat
		//[Required]
		public string Key { get; set; }
		public string Value { get; set; }
		public Guid Owner { get; set; }
		public virtual ICollection<GridElement> GridElements{ get; set; }

	}



}