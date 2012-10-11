using System;
using System.Linq;
using cms.data.Shared.Models;

namespace cms.data.EF.DataProvider
{
	public class JsonDataEfHelpers
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

	}
}