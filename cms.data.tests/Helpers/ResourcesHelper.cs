using System;
using System.Collections.Generic;
using NUnit.Framework;
using cms.data.Dtos;
using cms.data.EF.DataProvider;
using cms.data.Shared.Models;
using cms.shared;

namespace cms.data.tests.Helpers
	
{
	public static class ResourcesHelper
	{
		private static readonly string CurrentCulture = SharedLayer.Culture;

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

		public static Resource CheckResource(this Grid item, string key)
		{
			return item.CheckResource(key, CurrentCulture);
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

		public static Grid WithResource(this Grid item, string key, string value, string culture = "cs", int id = 0)
		{
			item.Resources.Add(GetResource(key, value, item.Id, culture, id));
			return item;
		}

		public static Grid WithResource(this Grid item, IList<Resource> allResources, string key, string value, string culture = "cs", int id = 0)
		{
			var newitem = GetResource(key, value, item.Id, culture, id);
			item.Resources.Add(newitem);
			allResources.Add(newitem);
			return item;
		}

		public static Dictionary<string, ResourceDtoLoc> WithResource(this Dictionary<string, ResourceDtoLoc> item, string key, string value, int id)
		{
			item.Add(key, new ResourceDtoLoc() { Id = id, Value = value });
			return item;
		}

		public static Resource GetResource(string key, string value, Guid parentId, string culture, int id)
		{
			return new Resource { Id = id, Value = value, Key = key, Culture = culture, Owner = parentId };
		}


	}
}