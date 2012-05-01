using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using cms.data.Models;

namespace cms.data
{
	public interface IDataProvider
	{
		IQueryable<ApplicationSetting> ApplicationSettings { get; }
		ApplicationSetting Add(ApplicationSetting newitem);
		ApplicationSetting Get(int id);
		ApplicationSetting Update(ApplicationSetting item);
		ApplicationSetting Find(string name);

	}
	public abstract class JsonDataProvider
	{
		protected JsonDataProvider(string application )
		{
			if (string.IsNullOrEmpty(application))
			{
				throw  new ArgumentNullException(application);
			}
		}

		public abstract JObject Applications();
		public abstract IEnumerable<Grid> Grids();
		public abstract Grid Grid(int id);
	}
}