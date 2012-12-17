using System;
using System.Collections.Generic;
using System.Linq;
using cms.data.Dtos;
using cms.data.Repository;
using cms.data.Shared.Models;
using cms.shared;

namespace cms.data.Extensions
{
	public static class JsonDataEfHelpers
	{
		public static Resource GetResource(string key, string value, Guid parentId, string culture, int id)
		{
			var cultureupdated = CorrectCulture(key, culture);
			return new Resource { Id = id, Value = value, Key = key, Culture = cultureupdated, Owner = parentId };
		}

		public static Resource GetByKey(this IEnumerable<Resource> resources, string key, string culture)
		{
			return resources.SingleOrDefault(a => a.Key == key && a.Culture == culture);
		}

		public static string CorrectCulture(string key, string culture)
		{
			return key == SpecialResourceEnum.Link ? null : culture;
		}
		public static string GetValueByKey(this IEnumerable<Resource> resources, string key, string culture)
		{
			var cultureUpdated = CorrectCulture(key, culture);
			var x = resources.GetByKey(key, cultureUpdated);
			return x == null ? "" : x.Value;
		}

		public static bool ContainsKeyValue(this IEnumerable<Resource> resources, string key, string value, string culture)
		{
			var cultureUpdated = CorrectCulture(key, culture);
			var res = resources.GetByKey(key, cultureUpdated);
			return res != null && res.Value == value;
		}

		public static Resource GetById(this IEnumerable<Resource> resources, int id, string culture)
		{
			return resources.SingleOrDefault(a => a.Id == id );
		}

		private static void AddNewResource(this IEntityWithResource currentItem, string key, ResourceDtoLoc i, string culture, IRepository repo)
		{
			var resource = repo.Add(i.ToResource(key, currentItem.Id, culture));
			repo.SaveChanges();
			currentItem.Resources.Add(resource);
		}

		public static GridElement GridElement(this Grid grid, Guid id)
		{
			return grid.GridElements.Single(x => x.Id == id);
		}


		public static void UpdateResourceList(this IEntityWithResource currentItem, IDictionary<string, ResourceDtoLoc> resourcesDto, string culture, IRepository repo)
		{
			foreach (var i in resourcesDto)
			{
				var cultureCorrected = CorrectCulture(i.Key, culture);
				var resourceByKey = GetByKey(currentItem.Resources, i.Key, cultureCorrected);
				var resourceById = i.Value.Id != 0 ? GetById(repo.Resources, i.Value.Id, cultureCorrected) : null;

				if (resourceByKey != null)
				{
					currentItem.Resources.Remove(resourceByKey);
				}

				if (resourceById != null)
				{
					currentItem.Resources.Add(resourceById);
				}
				else
				{
					currentItem.AddNewResource(i.Key, i.Value, cultureCorrected, repo);
				}
			}
		}
	}
}