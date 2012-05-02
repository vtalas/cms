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

		public abstract IEnumerable<ApplicationSetting> Applications();
		public abstract ApplicationSetting GetApplication(int id);
		public abstract ApplicationSetting DeleteApplication(int id);
		public abstract ApplicationSetting Add(ApplicationSetting newitem);

		public abstract IEnumerable<Grid> Grids();
		public abstract Grid GetGrid(int id);
		public abstract Grid DeleteGrid(int id);
		public abstract Grid Add(Grid newitem);
	}
}