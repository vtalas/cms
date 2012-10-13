using System;
using System.Collections.Generic;

namespace cms.data.Shared.Models
{
	public interface IEntityWithResource
	{
		Guid Id { get; set; }
		ICollection<Resource> Resources { get; set; }
	}
}