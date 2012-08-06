using System;
using System.Collections.Generic;

namespace cms.data.Shared.Models
{
	public class ApplicationSetting :IEntity
	{
		public ApplicationSetting()
		{
			Id = Guid.NewGuid();
		}

		public Guid Id { get; set; }
		public string Name { get; set; }
		public string DefaultLanguage { get; set; }
		public virtual ICollection<Grid> Grids { get; set; }
	}
}