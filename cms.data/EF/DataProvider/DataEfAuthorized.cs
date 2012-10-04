using System;
using System.Collections.Generic;
using System.Linq;
using cms.data.Dtos;
using cms.data.Shared.Models;
using cms.shared;

namespace cms.data.EF.DataProvider
{
	public class DataEfAuthorized : data.DataProvider
	{
		public EfContext db { get; set; }
		private IQueryable<ApplicationSetting> AvailableApplications { get { return db.ApplicationSettings.Where(x => x.Users.Any(a => a.Id == UserId)); } }

		public DataEfAuthorized(Guid applicationId, EfContext context, int userId)
			: base(applicationId, userId)
		{
			db = context;
		}

		public DataEfAuthorized(string applicationName, EfContext context, int userId)
			: base(applicationName, userId)
		{
			db = context;
		}

		public DataEfAuthorized(Guid applicationId, int userId) : this(applicationId, new EfContext(), userId) { }

		public DataEfAuthorized(string applicationName, int userId) : this(applicationName, new EfContext(), userId) { }

		public override sealed ApplicationSetting CurrentApplication
		{
			get
			{
				if (!ApplicationId.IsEmpty())
				{
					var aaa = db.ApplicationSettings.SingleOrDefault(x => x.Id == ApplicationId && x.Users.Any(u => u.Id == UserId));
					if (aaa == null)
						throw new Exception("Cannot get aplication");
					return aaa;

				}
				if (!String.IsNullOrEmpty(ApplicationName))
				{
					var aaa = db.ApplicationSettings.SingleOrDefault(x => x.Id == ApplicationId && x.Users.Any(u => u.Id == UserId));
					if (aaa == null)
						throw new Exception("Cannot get aplication");
					return aaa;

				}
				throw new Exception("Cannot get aplication");
			}
		}

		public override IEnumerable<ApplicationSetting> Applications()
		{
			return AvailableApplications.ToList();
		}

		public override MenuAbstract Menu
		{
			get { return new MenuAbstractImpl(CurrentApplication, db); }
		}

		public override PageAbstract Page
		{
			get { return new PageAbstractImpl(CurrentApplication, db); }
		}

		public override IEnumerable<GridListDto> Grids()
		{
			var a = db.Grids.Where(x => x.ApplicationSettings.Id == CurrentApplication.Id).Select(dto => new GridListDto
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
			var delete = AvailableApplications.Single(x => x.Id == id);
			db.ApplicationSettings.Remove(delete);
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

		////
		/// 
		/// TOHLE PUJDE JINAM 
		/// 

		IQueryable<Grid> AvailableGridsPage()
		{
			var a = db.Grids.Where(x => x.ApplicationSettings.Id == CurrentApplication.Id && x.Category == CategoryEnum.Page);
			return a;
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
			var item = db.GridElements.Get(guid, CurrentApplication.Id);
			var localizedResources = item.Resources.Where(x => x.Culture == CurrentCulture || x.Culture == null);
			item.Resources = localizedResources.ToList();
			return item;
		}
		public override GridElementDto Update(GridElementDto item)
		{
			if (item.ResourcesLoc != null)
			{
				JsonDataEfHelpers.UpdateResource(item, db, CurrentCulture, CurrentApplication.Id);
			}

			var el = db.GridElements.Get(item.Id, CurrentApplication.Id);
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

	}

}