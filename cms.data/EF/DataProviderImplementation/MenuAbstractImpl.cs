using System;
using System.Collections.Generic;
using System.Linq;
using cms.data.DataProvider;
using cms.data.Dtos;
using cms.data.Extensions;
using cms.data.Repository;
using cms.data.Shared.Models;
using cms.shared;

namespace cms.data.EF.DataProviderImplementation
{
	public class MenuAbstractImpl : MenuAbstract
	{
		private IRepository db { get; set; }

		public MenuAbstractImpl(ApplicationSetting application) : base(application) { }
		public MenuAbstractImpl(ApplicationSetting application, IRepository context)
			: base(application)
		{
			db = context;
		}

		private IQueryable<Grid> AvailableGrids
		{
			get
			{
				var a = db.Grids.Where(x => x.ApplicationSettings.Id == CurrentApplication.Id && x.Category == CategoryEnum.Menu);
				return a;
			}
		}

		public override void Delete(Guid guid)
		{
			var delete = AvailableGrids.Single(x => x.Id == guid);
			db.Remove(delete);
			db.SaveChanges();
		}

		public override MenuDto Get(Guid guid)
		{
			var grid = AvailableGrids.Single(x => x.Id == guid);
			return grid.ToMenuDto();
		}

		public override MenuDto Add(MenuDto newitem)
		{
			throw new NotImplementedException();
		}

		public override MenuItemDto UpdateMenuItem(MenuItemDto menuItem)
		{
			throw new NotImplementedException();
		}

		public override MenuItemDto AddMenuitem(MenuItemDto menuItem)
		{
			throw new NotImplementedException();
		}

		public override Guid AddMenuItem(MenuItemDto item, Guid gridId)
		{
			var grid = AvailableGrids.Single(x => x.Id == gridId);

			grid.UpdateResourceList(item.ResourcesLoc, CurrentCulture, db);

			var a = new GridElement
						{
							Position = item.Position,
							Content = item.Content,
							Type = item.Type,
							Skin = item.Skin,
							Parent = db.GridElements.Single(x => x.Id == new Guid(item.ParentId)),
						};
			grid.GridElements.Add(a);
			db.SaveChanges();
			return a.Id;
		}

		public override IEnumerable<MenuDto> List()
		{
			var a = AvailableGrids.Where(x => x.Category == CategoryEnum.Menu).ToList();
			return a.Select(grid => grid.ToMenuDto()).ToList();
		}
	}
}