using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using cms.data.Dtos;
using cms.data.Shared.Models;
using log4net.Core;

namespace cms.data.EF.DataProvider
{
	public static class JsonDataEfHelpers
	{
		public static void UpdateResources(IResourceElement item, EfContext db, string currentCulture, Guid applicationId)
		{
			var currentEl = db.GridElements.Get(item.Id, applicationId);
			foreach (var resUpdate in item.ResourcesLoc)
			{
				if (db.Resources.Exist(resUpdate.Value.Id))
				{
					var res = db.Resources.Get(resUpdate.Value.Id);
					if ((res.Owner == currentEl.Id))
					{
						res.Value = resUpdate.Value.Value;
					}
					else 
					{
						//pridat referenci pokud jeste neni pridana
						if (ReferenceExist(currentEl, resUpdate.Key, currentCulture))
						{
							//oddelat puvodni a pridat novou 
							currentEl.Resources.Remove(GetResource(currentEl, resUpdate.Key, currentCulture));
						}
						currentEl.Resources.Add(res);
					}
				}
				else
				{
					if (ReferenceExist(currentEl, resUpdate.Key, currentCulture))
						throw new ArgumentException("same resource exists");

					var newres = new Resource()
						             {
							             Owner = item.Id,
							             Culture = currentCulture,
							             Key = resUpdate.Key,
							             Value = resUpdate.Value.Value
						             };
					db.Resources.Add(newres);
					db.GridElements.Get(item.Id, applicationId).Resources.Add(newres);
				}
			}
		}
		static bool ReferenceExist(GridElement curEl, string key, string culture)
		{
			return curEl.Resources.Any(x => x.Key == key && x.Culture == culture);
		}

		static Resource GetResource(GridElement curEl, string key, string culture)
		{
			return curEl.Resources.Single(x => x.Key == key && x.Culture == culture);
		}

		public static Resource GetByKey(this IEnumerable<Resource> resources,  string key, string culture)
		{
			return resources.SingleOrDefault(a => a.Key == key && a.Culture == culture);
		}

		public static Resource GetById(this IEnumerable<Resource> resources, int id, string culture)
		{
			return resources.SingleOrDefault(a => a.Id == id && a.Culture == culture);
		}

		private static void AddNewResource(this IEntityWithResource currentItem, string key, ResourceDtoLoc i, string culture)
		{
			currentItem.Resources.Add(i.ToResource(key, currentItem.Id, culture));
		}

		public static void UpdateResourceList(this IEntityWithResource currentItem, IDictionary<string, ResourceDtoLoc> resourcesDto, string culture, IRepository repo)
		{
			foreach (var i  in resourcesDto )
			{
				var rrr = i.Value.Id == 0 ? GetByKey(currentItem.Resources, i.Key, culture) : GetById(repo.Resources, i.Value.Id,culture);
				if (rrr == null)
				{
					currentItem.AddNewResource(i.Key, i.Value, culture);
				}
				else
				{
					if (rrr.Owner == currentItem.Id)
					{
						rrr.Value = i.Value.Value;
					}
					else
					{
						var currentItemResource = GetByKey(currentItem.Resources, i.Key, culture);
						if (currentItemResource == null)
						{
							currentItem.Resources.Add(GetById(repo.Resources, i.Value.Id, culture));
						}
						else
						{
							currentItem.Resources.Remove(currentItemResource);
							currentItem.Resources.Add(GetById(repo.Resources, i.Value.Id, culture));
						}
					}
				}
				
			}
		}
	}

	public interface IRepository
	{
		IQueryable<Resource> Resources { get; }
	}

	public class EfRepository : IRepository
	{
		private EfContext db { get; set; }

		public EfRepository(EfContext db)
		{
			this.db = db;
		}

		public void Attach(Resource itemToAttach)
		{
			db.Resources.Attach(itemToAttach);
			db.SaveChanges();
		}

		public IQueryable<Resource> Resources { get
		{
			return db.Resources;
		} }
	}
}