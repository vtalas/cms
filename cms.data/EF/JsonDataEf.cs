using System.Collections.Generic;
using System.Data;
using System.Linq;
using cms.data.Models;

namespace cms.data.EF
{
	public class JsonDataEf : JsonDataProvider
	{
		private EfContext db { get; set; }

		public JsonDataEf(string application, EfContext context) : base(application)
		{
			db = context;
		}

		public JsonDataEf(string application)
			: this(application,new EfContext())
		{
		
		}

		public override ApplicationSetting GetApplication(int id)
		{
			return db.ApplicationSettings.Single(x => x.Id == id);
		}

		public override IEnumerable<ApplicationSetting> Applications()
		{
			return db.ApplicationSettings;
		}


		public override void DeleteApplication(int id)
		{
			var delete = GetApplication(id);
			db.ApplicationSettings.Remove(delete);
		}

		public override ApplicationSetting Add(ApplicationSetting newitem)
		{
			db.ApplicationSettings.Add(newitem);
			if (newitem.Grids == null)
			{
				newitem.Grids = new List<Grid>{
									new Grid
				        				{
				        					ApplicationSettings = newitem,
				        					Home = true,
				        					Name = "homepage",
				        				}				                		
				                	};
			}
			db.SaveChanges();
			
			return newitem;
		}

		public override IEnumerable<Grid> Grids()
		{
			return db.Grids.Where(x=>x.ApplicationSettings.Name == ApplicationName);
		}

		public override Grid GetGrid(int id)
		{
			return db.Grids.Single(x => x.Id == id);
		}

		public override void DeleteGrid(int id)
		{
			var delete = GetGrid(id);
			db.Grids.Remove(delete);
			db.SaveChanges();
		}
		public override void DeleteGridElement(int id)
		{
			var delete = GetGridElement(id);
			db.GridElements.Remove(delete);
			db.SaveChanges();
		}

		public override GridElement GetGridElement(int id)
		{
			return db.GridElements.Single(x => x.Id == id);
		}

		public override GridElement Update(GridElement item)
		{
			db.Entry(item).State = EntityState.Modified;
			//db.GridElements.Attach(item);
			db.SaveChanges();
			return item;
		}

		public override Grid Add(Grid newitem)
		{
			db.Grids.Add(newitem);
			db.SaveChanges();
			return newitem;
		}
		public override GridElement Add(GridElement newitem)
		{
			db.GridElements.Add(newitem);
			db.SaveChanges();
			return newitem;
		}

	}
}