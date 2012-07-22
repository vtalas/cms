using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using cms.data.Dtos;
using cms.data.Shared.Models;

namespace cms.data.EF
{
	public class JsonDataEf : JsonDataProvider
	{
		private EfContext db { get; set; }

		public JsonDataEf(string application, EfContext context) : base(application)
		{
			db = context;

			var app = GetApplication(application);
			ApplicationId = app == null ?  new Guid("00000000-0000-0000-0000-000000000000") : app.Id;

			//Database.SetInitializer(new CreateIfNotExists());
//			Database.SetInitializer(new DropAndCreateTables());
		}

		public JsonDataEf(Guid application, EfContext context) : base(application)
		{
			db = context;

			var app = GetApplication(application);
			ApplicationName =  app==null ? "" : app.Name ;
		}

		public JsonDataEf(string application): this(application,new EfContext()){}

		public JsonDataEf(Guid applicationId): this(applicationId,new EfContext()){}

		public override ApplicationSetting CurrentApplication{get { return db.ApplicationSettings.Single(x => x.Name == ApplicationName); }} 

		public static IEnumerable<ApplicationSettingDto> Applications()
		{
			using (var a = new EfContext() )
			{
				return a.ApplicationSettings.ToList().ToDtos();
			}
		}
	
		public override IEnumerable<Grid> Grids()
		{
			var a=  db.Grids.Where(x=>x.ApplicationSettings.Name == ApplicationName);
			return a;
		}

		public override sealed ApplicationSetting GetApplication(Guid id)
		{
			return db.ApplicationSettings.SingleOrDefault(x => x.Id == id);
		}

		public override sealed ApplicationSetting GetApplication(string name)
		{
			return db.ApplicationSettings.SingleOrDefault(x => x.Name == name);
		}

		public override Grid GetGrid(int id)
		{
			return Grids().Single(x => x.Id == id);
		}
		public override GridPageDto GetGridPage(int id)
		{
			var a = Grids().Single(x => x.Id == id);
			return a.ToGridPageDto();
		}
		
		
		//TODO: pokud nenanjde melo by o vracet homepage
		public override GridPageDto GetGridPage(string link)
		{
			var a = Grids().FirstOrDefault(x => x.Link == link);
			if (a == null)
			{
				throw new ObjectNotFoundException(string.Format("'{0}' not found", link));
			}
			//if (a.Count() > 1)
			//{
				
			//}
			return a.ToGridPageDto();
		}
		public override GridElement GetGridElement(int id)
		{
			return db.GridElements.Single(x => x.Id == id);
		}

		public override void DeleteGrid(int id)
		{
			var delete = GetGrid(id);
			db.Grids.Remove(delete);
			db.SaveChanges();
		}
		public override void DeleteGridElement(int id, int gridid)
		{
			var grid = GetGrid(gridid);
			var delete = grid.GridElements.Single(x => x.Id == id);
			db.GridElements.Remove(delete);
			var deletedline = delete.Line;

			if (!grid.GridElements.Any(x=>x.Line == deletedline))
			{
				foreach (var item in grid.GridElements.OrderBy(x=>x.Line))
				{
					if (item.Line > deletedline && item.Line > 0) item.Line--;
				}
			}
			
			db.SaveChanges();
		}
		
		public override GridElement Update(GridElement item)
		{
			db.Entry(item).State = EntityState.Modified;
			db.SaveChanges();
			return item;
		}

		public override Grid Update(GridPageDto item)
		{
			var grid = new Grid
			           	{
			           		Id = item.Id,
			           		Home = item.Home,
			           		Link = item.Link,
			           		Name = item.Name
			           	};
			db.Entry(grid).State = EntityState.Modified;
			db.SaveChanges();
			return grid;
		}
		public override void DeleteApplication(Guid id)
		{
			var delete = GetApplication(id);
			db.ApplicationSettings.Remove(delete);
		}

		public override T Add<T>(T newitem)
		{
			return newitem;
		}

		public override Grid Add(Grid newitem)
		{
			CurrentApplication.Grids.Add(newitem);
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

	}
}