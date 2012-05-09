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
		public abstract GridPage GetGridPage(int id);
		public abstract void DeleteGrid(int id);
		public abstract Grid Add(Grid newitem);
		public abstract ApplicationSetting Add(ApplicationSetting newitem);
		public abstract void DeleteGridElement(int id);
		public abstract GridElement GetGridElement(int id);
		public abstract GridElement Update(GridElement item);
	}

	public class GridPage
	{
		public GridPage()
		{
			var lines = new List<GridElement>();
			Lines = new List<IEnumerable<GridElement>> {lines};
		}

		public int Id { get; set; }
		public string Name { get; set; }
		public string Link { get; set; }
		public bool Home { get; set; }
		public IList<IEnumerable<GridElement>> Lines { get; set; }



	
	}
}

