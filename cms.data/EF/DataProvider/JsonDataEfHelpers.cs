using System;
using System.Collections.Generic;
using System.Linq;
using cms.data.Dtos;
using cms.data.Shared.Models;

namespace cms.data.EF.DataProvider
{
	public static class JsonDataEfHelpers
	{
		public static Resource GetResource(string key, string value, Guid parentId, string culture, int id)
		{
			return new Resource { Id = id, Value = value, Key = key, Culture = culture, Owner = parentId };
		}

		public static Resource GetByKey(this IEnumerable<Resource> resources, string key, string culture)
		{
			return resources.SingleOrDefault(a => a.Key == key && a.Culture == culture);
		}

		public static bool ContainsKeyValue(this IEnumerable<Resource> resources, string key, string value, string culture)
		{
			var res = resources.GetByKey(key, culture);
			return res != null && res.Value == value;
		}

		public static Resource GetById(this IEnumerable<Resource> resources, int id, string culture)
		{
			return resources.SingleOrDefault(a => a.Id == id && a.Culture == culture);
		}

		private static void AddNewResource(this IEntityWithResource currentItem, string key, ResourceDtoLoc i, string culture, IRepository repo)
		{
			var resource = repo.Add(i.ToResource(key, currentItem.Id, culture));
			repo.SaveChanges();
			currentItem.Resources.Add(resource);
		}

		public static void UpdateResourceList(this IEntityWithResource currentItem, IDictionary<string, ResourceDtoLoc> resourcesDto, string culture, IRepository repo)
		{
			foreach (var i in resourcesDto)
			{
				var resourceByKey = GetByKey(currentItem.Resources, i.Key, culture);
				var resourceById = i.Value.Id != 0 ? GetById(repo.Resources, i.Value.Id, culture) : null;

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
					currentItem.AddNewResource(i.Key, i.Value, culture, repo);
				}
			}
		}
	}

	public interface IRepository
	{
		IQueryable<Resource> Resources { get; }
		IQueryable<Grid> Grids { get; }
		IQueryable<ApplicationSetting> ApplicationSettings { get; }
		ApplicationSetting Add(ApplicationSetting item);
		Resource Add(Resource itemToAttach);
		Grid Add(Grid item);
		void Remove(Grid item);
		void SaveChanges();

	}

	public class EfRepository : IRepository
	{
		private EfContext db { get; set; }

		public EfRepository(EfContext db)
		{
			this.db = db;
		}

		public IQueryable<Resource> Resources{ get { return db.Resources;}}
		public IQueryable<Grid> Grids { get { return db.Grids; } }
		public IQueryable<ApplicationSetting> ApplicationSettings { get { return db.ApplicationSettings;} }

		public ApplicationSetting Add(ApplicationSetting item)
		{
			return db.ApplicationSettings.Add(item);
		}

		public Resource Add(Resource itemToAdd)
		{
			return db.Resources.Add(itemToAdd);
		}

		public Grid Add(Grid item)
		{
			return db.Grids.Add(item);
		}

		public void Remove(Grid item)
		{
			db.Grids.Remove(item);
		}

		public void SaveChanges()
		{
			db.SaveChanges();
		}

	}
}