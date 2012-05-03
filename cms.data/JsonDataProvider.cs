using System;
using System.Collections.Generic;
using cms.data.Models;

namespace cms.data
{

	public abstract class JsonDataProvider
	{
		protected string ApplicationName { get; set; }
		protected JsonDataProvider(string application )
		{
			//if (string.IsNullOrEmpty(application))
			//{
			//    throw  new ArgumentNullException(application);
			//}
			ApplicationName = application;
		}

		public abstract IEnumerable<ApplicationSetting> Applications();
		public abstract ApplicationSetting GetApplication(int id);
		public abstract void DeleteApplication(int id);
		public abstract GridElement Add(GridElement gridElement);

		public abstract IEnumerable<Grid> Grids();
		public abstract Grid GetGrid(int id);
		public abstract void DeleteGrid(int id);
		public abstract Grid Add(Grid newitem);
		public abstract ApplicationSetting Add(ApplicationSetting newitem);
	}
}

