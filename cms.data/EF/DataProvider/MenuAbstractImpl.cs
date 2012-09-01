using System;
using System.Collections.Generic;
using System.Linq;
using cms.data.Dtos;
using cms.data.Shared.Models;
using cms.shared;

namespace cms.data.EF.DataProvider
{
	public class MenuAbstractImpl : MenuAbstract
	{
		private EfContext db { get; set; }
		
		public MenuAbstractImpl(Guid applicationId) : base(applicationId){}
		public MenuAbstractImpl(Guid applicationId, EfContext context) : base(applicationId)
		{
			db = context;
		}

		IQueryable<Grid> AvailableGridsMenu()
		{
			var a = db.Grids.Where(x => x.ApplicationSettings.Id == ApplicationId && x.Category == CategoryEnum.Menu);
			return a;
		}

		public override MenuDto Get(Guid guid)
		{
			var grid = AvailableGridsMenu().Single(x => x.Id == guid);
			return grid.ToMenuDto();
		}

		public override MenuDto Add(MenuDto newitem)
		{
			throw new NotImplementedException();
		}

		public override Guid AddMenuItem(MenuItemDto item, Guid gridId)
		{
			JsonDataEfHelpers.UpdateResource(item,db,CurrentCulture,ApplicationId);
			var grid = AvailableGridsMenu().Single(x => x.Id == gridId);
			var a = new GridElement
				        {
					        Line = item.Line,
							Content = item.Content,
							Type = item.Type,
							Skin = item.Skin,
							Parent = db.GridElements.Single(x=>x.Id ==  new Guid(item.ParentId)),
				        };
			grid.GridElements.Add(a);
			db.SaveChanges();
			return a.Id;
		}

		public override IEnumerable<MenuDto> List()
		{
			var a = AvailableGridsMenu().Where(x=>x.Category == CategoryEnum.Menu).ToList();
			return a.Select(grid => grid.ToMenuDto()).ToList();
		}
	}
}