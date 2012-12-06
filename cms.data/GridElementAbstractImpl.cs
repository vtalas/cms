using System;
using System.Linq;
using cms.data.Dtos;
using cms.data.DtosExtensions;
using cms.data.EF;
using cms.data.EF.Repository;
using cms.data.Shared.Models;
using cms.shared;

namespace cms.data
{
	public class GridElementAbstractImpl : GridElementAbstract
	{
		EfContext db { get; set; }
		IRepository Respository { get; set; }

		public GridElementAbstractImpl(ApplicationSetting application) : base(application){}
		public GridElementAbstractImpl(ApplicationSetting application, EfContext context) : base(application)
		{
			db = context;
			Respository = new EfRepository(db);
		}

		public override GridElement AddToGrid(GridElement gridElement, Guid gridId)
		{
			var grid = GetGrid(gridId);
			gridElement.Grid.Add(grid);

			db.GridElements.Add(gridElement);

			if (gridElement.Resources != null)
			{
				foreach (var resource in gridElement.Resources)
				{
					if (resource.Id != 0)
					{
						db.Resources.Attach(resource);
					}
					else
					{
						resource.Owner = gridElement.Id;
						db.Resources.Add(resource);
					}
				}
			}
			db.SaveChanges();
			return gridElement;
		}

		public override void Delete(Guid id, Guid gridid)
		{
			var grid = GetGrid(gridid);
			var delete = grid.GridElements.Single(x => x.Id == id);
			db.GridElements.Remove(delete);
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

			el.UpdateResourceList(item.ResourcesLoc, CurrentCulture, Respository);
			
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

		private Grid GetGrid(Guid gridId)
		{
			return AvailableGridsPage().Single(x => x.Id == gridId);
		}

		IQueryable<Grid> AvailableGridsPage()
		{
			var a = db.Grids.Where(x => x.ApplicationSettings.Id == CurrentApplication.Id && x.Category == CategoryEnum.Page);
			return a;
		}

	}
}