using System.Collections.Generic;
using System.Linq;
using cms.data.Dtos;
using cms.data.Models;

namespace cms.data.Files
{
	public class JsonDataFiles : JsonDataProvider
	{
		List<ApplicationSetting> apps  = new List<ApplicationSetting>()
		                                 	{
		                                 		new ApplicationSetting(){Id = 1, Name = "xxxxx"},
		                                 		new ApplicationSetting(){Id = 2, Name = "xxxxxxxx"}
		                                 	}; 

		public JsonDataFiles(string application) : base(application)
		{
		}

		public override ApplicationSetting GetApplication(int id)
		{
			return apps.Single(x => x.Id == id);
		}

		public override IEnumerable<ApplicationSetting> Applications()
		{
			return apps;
		}


		public override void DeleteApplication(int id)
		{
			throw new System.NotImplementedException();
		}

		public override GridElement Add(GridElement gridElement)
		{
			throw new System.NotImplementedException();
		}

		public override ApplicationSetting Add(ApplicationSetting newitem)
		{
			throw new System.NotImplementedException();
		}

		public override void DeleteGridElement(int id)
		{
			throw new System.NotImplementedException();
		}

		public override GridElement GetGridElement(int id)
		{
			throw new System.NotImplementedException();
		}

		public override GridElement Update(GridElement item)
		{
			throw new System.NotImplementedException();
		}

		public override GridPageDto GetGridPage(string link)
		{
			throw new System.NotImplementedException();
		}

		public override T Add<T>(T newitem)
		{
			throw new System.NotImplementedException();
		}

		public override IEnumerable<Grid> Grids()
		{
			throw new System.NotImplementedException();
		}

		public override Grid GetGrid(int id)
		{
			var gridElems = new List<GridElement>()
			                	{
			                		new GridElement {Content = "aaa", Line = 0, Position = 0, Width = 3, Type = "text"},
			                		new GridElement {Content = "aaa", Line = 0, Position = 3, Width = 3, Type = "text"},
			                		new GridElement {Content = "aaa", Line = 0, Position = 6, Width = 3, Type = "text"},
			                		new GridElement {Content = "aaa", Line = 2, Position = 5, Width = 3, Type = "text"},
			                		new GridElement {Content = "aaa", Line = 3, Position = 0, Width = 12, Type = "text"}
			                	};

			
			var grid = new Grid()
			           	{

			           		GridElements = gridElems
			           	};

			return grid;
		}

		public override GridPageDto GetGridPage(int id)
		{
			throw new System.NotImplementedException();
		}

		public override void DeleteGrid(int id)
		{
			throw new System.NotImplementedException();
		}

		public override Grid Add(Grid newitem)
		{
			throw new System.NotImplementedException();
		}
	}
}