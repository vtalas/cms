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
		public MenuAbstractImpl(ApplicationSetting application, IRepository context)
			: base(application, context)
		{
			db = context;
		}

		public override void Delete(Guid guid)
		{
			var delete = GetGrid(guid);
			db.Remove(delete);
			db.SaveChanges();
		}

		public override MenuDto Get(Guid guid)
		{
			var grid = GetGrid(guid);
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

		public override MenuItemDto AddMenuItem(MenuItemDto item, Guid gridId)
		{
			var grid = GetGrid(gridId);

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
			return a.ToMenuItemDto(grid.GridElements);
		}

		public override IEnumerable<MenuDto> List()
		{
			var a = AvailableGrids.Where(x => x.Category == CategoryEnum.Menu).ToList();
			return a.Select(grid => grid.ToMenuDto()).ToList();
		}
	}
}