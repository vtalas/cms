using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Security;
using cms.data.Dtos;
using cms.data.Shared.Models;

namespace cms.data.EF
{
	public class JsonDataEf : JsonDataProvider
	{
		private EfContext db { get; set; }

		public JsonDataEf(string application, EfContext context)
			: base(application)
		{
			db = context;
			var app = GetApplication(application);
			ApplicationId = app == null ? new Guid("00000000-0000-0000-0000-000000000000") : app.Id;

			//Database.SetInitializer(new CreateIfNotExists());
			//			Database.SetInitializer(new DropAndCreateTables());
		}

		public JsonDataEf(Guid application, EfContext context)
			: base(application)
		{
			db = context;

			var app = GetApplication(application);
			ApplicationName = app == null ? "" : app.Name;
		}

		public JsonDataEf(string application) : this(application, new EfContext()) { }

		public JsonDataEf(Guid applicationId) : this(applicationId, new EfContext()) { }

		public override ApplicationSetting CurrentApplication { get { return db.ApplicationSettings.Single(x => x.Name == ApplicationName); } }

		public static IEnumerable<ApplicationSettingDto> Applications()
		{
			using (var a = new EfContext())
			{
				return a.ApplicationSettings.ToList().ToDtos();
			}
		}

		IQueryable<Grid> AvailableGrids()
		{
			var a = db.Grids.Where(x => x.ApplicationSettings.Id == ApplicationId);
			return a;
		}

		public override IEnumerable<GridPageDto> GridPages()
		{
			var a = db.Grids.Where(x => x.ApplicationSettings.Name == ApplicationName).ToList();
			return a.Select(grid => grid.ToGridPageDto()).ToList();
		}

		public override ResourceDto Add(ResourceDto resource)
		{
			var item = new Resource()
						{
							Key = resource.Key,
							Value = resource.Value,
							Culture = resource.Culture,
						};
			db.Resources.Add(item);
			db.SaveChanges();
			return item.ToDto();
		}

		Resource GetResource(int id)
		{
			return db.Resources.SingleOrDefault(x => x.Id == id);
		}

		Resource GetResource(Guid elementId, Resource resource)
		{
			var el = GetGridElement(elementId);
			if (el.Resources != null)
			{
				return el.Resources.SingleOrDefault(x =>
					(x.Key == resource.Key && x.Culture == resource.Culture)
					|| resource.Id != 0 && x.Id == resource.Id
				);
			}
			return null;
		}

		public override ResourceDto GetResourceDto(Guid elementId, string key, string culture)
		{
			//var r = GetResource(elementIdt, key, culture);
			//if (r != null) 
			//    return r.ToDto();
			//throw new ObjectNotFoundException(string.Format("resource {0} not found {1}", key, culture));
			throw new NotImplementedException();
		}

		public override void Dispose()
		{
			if (db != null) db.Dispose();
		}

		public override sealed ApplicationSetting GetApplication(Guid id)
		{
			return db.ApplicationSettings.SingleOrDefault(x => x.Id == id);
		}

		public override sealed ApplicationSetting GetApplication(string name)
		{
			return db.ApplicationSettings.SingleOrDefault(x => x.Name == name);
		}

		public override Grid GetGrid(Guid id)
		{
			return AvailableGrids().Single(x => x.Id == id);
		}

		public override GridPageDto GetGridPage(Guid id)
		{
			var a = AvailableGrids().Single(x => x.Id == id);
			return a.ToGridPageDto();
		}
		//TODO: pokud nenanjde melo by o vracet homepage
		public override GridPageDto GetGridPage(string link)
		{
			var a = db.Grids.FirstOrDefault(x => x.ApplicationSettings.Name == ApplicationName && x.Resource.Value == link);
			//var a = Grids().FirstOrDefault(x => x.Resource.Value == link);
			if (a == null)
			{
				throw new ObjectNotFoundException(string.Format("'{0}' not found", link));
			}
			//if (a.Count() > 1)
			//{

			//}
			return a.ToGridPageDto();
		}

		public override GridElement GetGridElement(Guid id)
		{
			return db.GridElements.Single(x => x.Id == id);
		}

		public override void DeleteGrid(Guid id)
		{
			var delete = GetGrid(id);
			db.Grids.Remove(delete);
			db.SaveChanges();
		}

		public override void DeleteGridElement(Guid id, Guid gridid)
		{
			var grid = GetGrid(gridid);
			var delete = grid.GridElements.Single(x => x.Id == id);
			db.GridElements.Remove(delete);
			var deletedline = delete.Line;

			if (!grid.GridElements.Any(x => x.Line == deletedline))
			{
				foreach (var item in grid.GridElements.OrderBy(x => x.Line))
				{
					if (item.Line > deletedline && item.Line > 0) item.Line--;
				}
			}

			db.SaveChanges();
		}

		bool IsOwner(Guid id, Resource resource)
		{
			return resource.Owner == id;
		}

		public override GridElement Update(GridElement item)
		{

			if (item.Resources != null)
			{
				foreach (var resource in item.Resources)
				{
					var rdb = GetResource(item.Id, resource);
					if (rdb != null && IsOwner(item.Id, resource))
					{
						rdb.Value = resource.Value;
						continue;
					}

					if (rdb == null && db.Resources.Any(x => x.Id == resource.Id))
					{
						//pridej referenci
						GetGridElement(item.Id).Resources.Add(GetResource(resource.Id));
						continue;
					}

					if (rdb == null)
					{
						resource.Owner = item.Id;
						db.Resources.Add(resource);
						GetGridElement(item.Id).Resources.Add(resource);
					}
				}
			}
			var el = GetGridElement(item.Id);
			//TODO:nahovno, udelat lip
			el.Line = item.Line;
			el.Position = item.Position;
			el.Skin = item.Skin;
			el.Type = item.Type;
			el.Width = item.Width;
			el.Content = item.Content;

			db.SaveChanges();
			return item;
		}

		public override GridPageDto Update(GridPageDto item)
		{
			if (item.ResourceDto != null)
			{
				if (item.ResourceDto.Id != 0)
				{
					db.Resources.Single(x => x.Id == item.ResourceDto.Id).UpdateValues(item.ResourceDto);
				}
			}
			var grid = GetGrid(item.Id);

			grid.Name = item.Name;
			grid.Home = item.Home;

			db.Entry(grid).State = EntityState.Modified;
			db.SaveChanges();
			return grid.ToGridPageDto();
		}

		public override void DeleteApplication(Guid id)
		{
			var delete = GetApplication(id);
			db.ApplicationSettings.Remove(delete);
		}

		
		void CheckIfLinkExist(GridPageDto newitem)
		{
			var linkExist = AvailableGrids().Any(x => x.Resource.Value == newitem.ResourceDto.Value);
			if (linkExist)
			{
				throw new ArgumentException("link exists");
			}
			
		}
		
		public override GridPageDto Add(GridPageDto newitem)
		{
			var item = newitem.ToGrid();
			if (newitem.ResourceDto == null)
			{
				newitem.ResourceDto = new ResourceDto { Value = item.Name.Replace(" ", string.Empty) };
			}

			CheckIfLinkExist(newitem);

			CurrentApplication.Grids.Add(item);
			db.Grids.Add(item);

			var res = db.Resources.Add(newitem.ResourceDto.ToResource());
			res.Owner = item.Id;
			item.Resource = res;

			db.SaveChanges();
			return item.ToGridPageDto();
		}

		public override GridElement AddGridElementToGrid(GridElement newitem, Guid gridId)
		{
			var grid = GetGrid(gridId);
			newitem.Grid.Add(grid);

			db.GridElements.Add(newitem);

			if (newitem.Resources != null)
			{
				foreach (var resource in newitem.Resources)
				{
					if (resource.Id != 0)
					{
						db.Resources.Attach(resource);
					}
					else
					{
						resource.Owner = newitem.Id;
						db.Resources.Add(resource);
					}
				}
			}

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