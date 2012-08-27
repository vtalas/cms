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

		public override IEnumerable<MenuDto> List()
		{
			var a = AvailableGridsMenu().Where(x=>x.Category == CategoryEnum.Menu).ToList();
			return a.Select(grid => grid.ToMenuDto()).ToList();
		}
	}
}