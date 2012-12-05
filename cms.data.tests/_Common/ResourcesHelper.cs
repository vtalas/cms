using System;
using System.Collections.Generic;
using NUnit.Framework;
using cms.data.Dtos;
using cms.data.EF.DataProvider;
using cms.data.Shared.Models;

namespace cms.data.tests._Common
{
	public static class ResourcesHelper
	{

		public static Dictionary<string, ResourceDtoLoc> EmptyResourcesDto()
		{
			return new Dictionary<string, ResourceDtoLoc>();
		}

		public static Resource CheckResource(this Grid item, string key, string culture)
		{
			var getbyLink = item.Resources.GetByKey(key, culture);
			Assert.IsNotNull(getbyLink);
			return getbyLink;
		}


		public static Resource ValueIs(this Resource item, string value)
		{
			Assert.AreEqual(value, item.Value);
			return item;
		}

		public static Resource OwnerIs(this Resource item, Guid expectedId)
		{
			Assert.AreEqual(expectedId, item.Owner);
			return item;
		}


		public static Dictionary<string, ResourceDtoLoc> WithResource(this Dictionary<string, ResourceDtoLoc> item, string key,
		                                                              string value, int id)
		{
			item.Add(key, new ResourceDtoLoc() {Id = id, Value = value});
			return item;
		}

	}

}
