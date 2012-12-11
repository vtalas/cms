using System;
using System.Linq;
using cms.data.Dtos;
using cms.data.DataProvider;
using cms.data.Extensions;
using cms.data.Repository;
using cms.data.Shared.Models;
using cms.shared;

namespace cms.data.EF.DataProviderImplementation
{
	public class GridElementAbstractImpl : GridElementAbstract
	{
		IRepository db { get; set; }

		public GridElementAbstractImpl(ApplicationSetting application) : base(application){}
		public GridElementAbstractImpl(ApplicationSetting application, IRepository context) : base(application)
		{
			db = context;
		}

		public override GridElement AddToGrid(GridElement gridElement, Guid gridId)
		{
			var grid = GetGridFromDb(gridId);
			gridElement.Grid.Add(grid);

			db.Add(gridElement);

			if (gridElement.Resources != null)
			{
				foreach (var resource in gridElement.Resources)
				{
					if (resource.Id != 0)
					{
						//db.Resources.Attach(resource);
					}
					else
					{
						resource.Owner = gridElement.Id;
						db.Add(resource);
					}
				}
			}
			db.SaveChanges();
			return gridElement;
		}

		public override void Delete(Guid id, Guid gridid)
		{
			var grid = GetGridFromDb(gridid);
			var delete = grid.GridElements.Single(x => x.Id == id);
			db.Remove(delete);

			var deletedPosition = delete.Position;

			if (grid.GridElements.All(x => x.Position != deletedPosition))
			{
				foreach (var item in grid.GridElements.OrderBy(x => x.Position))
				{
					if (item.Position > deletedPosition && item.Position > 0) item.Position--;
				}
			}
			db.SaveChanges();
		}

		public override GridElementDto Update(GridElementDto item)
		{
			var el = db.GridElements.Get(item.Id, CurrentApplication.Id);

			el.UpdateResourceList(item.ResourcesLoc, CurrentCulture, db);
			
			//TODO:nahovno, udelat lip
			el.Position = item.Position;
			el.Skin = item.Skin;
			el.Type = item.Type;
			el.Width = item.Width;
			el.Content = item.Content;

			db.SaveChanges();
			return item;
		}

		public override GridElementDto Get(Guid guid)
		{
			var item = db.GridElements.Get(guid, CurrentApplication.Id);
			var localizedResources = item.Resources.Where(x => x.Culture == CurrentCulture || x.Culture == null);
			item.Resources = localizedResources.ToList();
			return item.ToDto();
		}

		private Grid GetGridFromDb(Guid gridId)
		{
			return AvailableGridsPage.Single(x=>x.Id == gridId);
		}

		private IQueryable<Grid> AvailableGridsPage
		{
			get
			{
				return db.Grids.Where(x => x.ApplicationSettings.Id == CurrentApplication.Id && x.Category == CategoryEnum.Page);
			}
		}

	}
}