using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using cms.data.Dtos;
using cms.data.Shared.Models;
using cms.shared;

namespace cms.data.EF.DataProvider
{
	public class DataEf : data.DataProvider
	{
		private UserProfile CurrentUser { get; set; }
		private EfContext db { get; set; }

		public override sealed ApplicationSetting CurrentApplication { get; protected set; } 

		private DataEf(ApplicationSetting app, EfContext context, int userId) : base(app.Id, userId)
		{
			db = context;
			ApplicationId = app.Id;
			ApplicationName = app.Name;
			CurrentApplication = app;
			CurrentUser = db.UserProfile.Single(x => x.Id == userId);
		}

		public DataEf(string applicationName, EfContext context, int userId)
			: this(GetApp(applicationName, context, userId), context, userId) { }

		public DataEf(Guid applicationId, EfContext context, int userId)
			: this(GetApp(applicationId, context, userId), context, userId){}

		public DataEf(string applicationName, int userId) : this(applicationName, new EfContext(), userId) { }

		public DataEf(Guid applicationId, int userId) : this(applicationId, new EfContext(), userId) { }

		private static ApplicationSetting GetApp(Guid appId, EfContext context, int userId)
		{
			return context.ApplicationSettings.SingleOrDefault(x => x.Id == appId && x.Users.Any(u => u.Id == userId));
		}

		private static ApplicationSetting GetApp(string appName, EfContext context, int userId)
		{
			return context.ApplicationSettings.SingleOrDefault(x => x.Name == appName && x.Users.Any(u => u.Id == userId));
		}
		
		public override IEnumerable<ApplicationSettingDto> Applications()
		{
			return db.ApplicationSettings.ToList().ToDtos();
		}

		IQueryable<Grid> AvailableGridsPage()
		{
			var a = db.Grids.Where(x => x.ApplicationSettings.Id == ApplicationId && x.Category == CategoryEnum.Page);
			return a;
		}

		private  ApplicationSetting GetApplication(Guid id)
		{
			return db.ApplicationSettings.SingleOrDefault(x => x.Id == id);
		}

		public override Grid GetGrid(Guid id)
		{
			return AvailableGridsPage().Single(x => x.Id == id);
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

		public override GridElement GetGridElement(Guid guid)
		{
			var item = db.GridElements.Get(guid, ApplicationId);
			var localizedResources = item.Resources.Where(x => x.Culture == CurrentCulture || x.Culture == null);
			item.Resources = localizedResources.ToList();
			return item;
		}

		public override GridElementDto Update(GridElementDto item)
		{
			if (item.ResourcesLoc != null)
			{
				JsonDataEfHelpers.UpdateResource(item, db, CurrentCulture, ApplicationId);
			}

			var el = db.GridElements.Get(item.Id, ApplicationId);
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
					db.Resources.Single(x => x.Id == item.ResourceDto.Id).Value = item.ResourceDto.Value;
				}
			}
			var grid = GetGrid(item.Id);

			grid.Name = item.Name;
			grid.Home = item.Home;

			db.Entry(grid).State = EntityState.Modified;
			db.SaveChanges();
			return grid.ToGridPageDto();
		}

		public override MenuAbstract Menu
		{
			get { return new MenuAbstractImpl(ApplicationId, db); }
		}

		public override PageAbstract Page
		{
			get { return new PageAbstractImpl(ApplicationId, db); }
		}

		public override IEnumerable<GridListDto> Grids()
		{
			var a = db.Grids.Where(x => x.ApplicationSettings.Id == ApplicationId).Select(dto => new GridListDto
				{
					Id = dto.Id,
					Category = dto.Category,
					Name = dto.Name,
					Home = dto.Home,
					ResourceDto = new ResourceDtoLoc()
						              {
							              Id = dto.Resource.Id,
										  Value = dto.Resource.Value
						              }
				});
			return a.ToList();
		}

		public override void DeleteApplication(Guid id)
		{
			var delete = GetApplication(id);
			db.ApplicationSettings.Remove(delete);
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

		public override ApplicationSetting Add(ApplicationSetting newitem, int userId)
		{
			var user = db.UserProfile.Single(x => x.Id == userId);
			newitem.Users.Add(user);

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

		public override void Dispose()
		{
			if (db != null) db.Dispose();
		}
	}
}