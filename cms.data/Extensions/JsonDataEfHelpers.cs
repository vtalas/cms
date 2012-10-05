using System;
using System.Collections.Generic;
using System.Linq;
using cms.data.DataProvider;
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

		public static void RefreshPositions(this GridElement itemDb, GridElementDto newValues, GridElement newParent)
		{
			var maxPosiblePosition = 0;
			var groupDestination = itemDb.Grid.GridElements.Where(x => Shared.Models.GridElement.EqualsById(x.Parent, newParent));
			maxPosiblePosition = groupDestination.Count();
			
			if (Shared.Models.GridElement.EqualsById(itemDb.Parent, newParent))
			{
				groupDestination.CorrectPostion(itemDb.Position, newValues.Position);
			}
			else
			{
				var groupSource = itemDb.Grid.GridElements.Where(x => Shared.Models.GridElement.EqualsById(x.Parent, itemDb.Parent));
				groupSource.CorrectPostionSoftRemove(itemDb.Position);
				groupDestination.CorrectPostionSoftAdd(newValues.Position);
			}
			itemDb.Position = newValues.Position > maxPosiblePosition && maxPosiblePosition > 0  ? maxPosiblePosition - 1  : newValues.Position;
		}

		public static void UpdateResourceList(this IEntityWithResource currentItem, IDictionary<string, ResourceDtoLoc> resourcesDto, string culture, IRepository repo)
		{
			if (resourcesDto == null)
			{
				return;
			}
			foreach (var i in resourcesDto)
			{
				var cultureCorrected = CorrectCulture(i.Key, culture);
				var resourceByKey = GetByKey(currentItem.Resources, i.Key, cultureCorrected);

				if (i.Value.Id == 0)
				{
					if (resourceByKey != null && resourceByKey.Owner == currentItem.Id)
					{
						resourceByKey.Value = i.Value.Value;
					}
					if (resourceByKey == null)
					{
						currentItem.AddNewResource(i.Key, i.Value, cultureCorrected, repo);
					}
					if (resourceByKey != null && resourceByKey.Owner != currentItem.Id)
					{
						currentItem.Resources.Remove(resourceByKey);
						currentItem.AddNewResource(i.Key, i.Value, cultureCorrected, repo);
					}
				}
				else
				{
					var resourceById = GetById(repo.Resources, i.Value.Id, cultureCorrected);
					if (resourceById != null )
					{
						if (resourceById.Owner == currentItem.Id)
						{
							resourceById.Value = i.Value.Value;
						}
						if (resourceByKey != null)
						{
							currentItem.Resources.Remove(resourceByKey);
							currentItem.Resources.Add(resourceById);
						}
						if (resourceByKey == null)
						{
							currentItem.Resources.Add(resourceById);
						}
					}
				}
			}
		}
	}
}