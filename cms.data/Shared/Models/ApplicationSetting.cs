using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace cms.data.Shared.Models
{
	public class ApplicationSetting :IEntity
	{
		public ApplicationSetting()
		{
			Id = Guid.NewGuid();
			Users = new Collection<UserProfile>();
		}

		public Guid Id { get; set; }
		public ICollection<UserProfile> Users { get; set; }
		public string Name { get; set; }
		public string DefaultLanguage { get; set; }
		public virtual ICollection<Grid> Grids { get; set; }
	}
}