using System;
using System.Collections.Generic;
using cms.data.Dtos;

namespace cms.data.EF.DataProvider
{
	public interface IResourceElement
	{
		Guid Id { get; set; }
		IDictionary<string, ResourceDtoLoc> ResourcesLoc { get; set; }
	}
}