using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using cms.data.DataProvider;
using cms.data.Dtos;
using cms.data.Extensions;
using cms.data.Repository;
using cms.data.Shared.Models;
using cms.shared;

namespace cms.data.EF.DataProviderImplementation
{
	public class PageAbstractImpl : PageAbstract
	{
		public PageAbstractImpl(ApplicationSetting application, IRepository context)
			: base(application, context)
		{
			db = context;
		}

		public override IEnumerable<GridPageDto> List()
		{
			var a = AvailableGrids.ToList();
			return a.Select(grid => grid.ToGridPageDto()).ToList();
		}

		public override GridPageDto Get(Guid id)
		{
			var grid = GetGrid(id);
			if (grid == null || grid.Category != CategoryEnum.Page)
			{
				throw new ObjectNotFoundException(string.Format("'{0}' not found", id.ToString()));
			}

			return grid.ToGridPageDto();
		}

		//TODO: pokud nenanjde melo by o vracet homepage
		public override GridPageDto Get(string linkValue)
		{
			var a = GetGrid(linkValue);
			if (a == null || a.Category != CategoryEnum.Page)
			{
				throw new ObjectNotFoundException(string.Format("'{0}' not found", linkValue));
			}
			return a.ToGridPageDto();
		}

		public override GridPageDto Add(GridPageDto newitem)
		{
			var item = newitem.ToGrid();

			ValidateLink(newitem.Link);

			CurrentApplication.Grids.Add(item);
			db.Add(item);
			db.SaveChanges();
			return item.ToGridPageDto();
		}

		public override GridPageDto Update(GridPageDto item)
		{
			ValidateLink(item.Link);

			var grid = GetGrid(item.Id);

			var resLink = grid.Resources.GetByKey(SpecialResourceEnum.Link, null);
			var resName = grid.Resources.GetByKey("name", CurrentCulture);

			grid.Home = item.Home;
			resLink.Value = item.Link;
			resName.Value = item.Name;

			db.SaveChanges();
			return grid.ToGridPageDto();
		}

		public override void Delete(Guid guid)
		{
			var delete = AvailableGrids.SingleOrDefault(x => x.Id == guid);
			if (delete == null)
			{
				throw new ObjectNotFoundException(string.Format(" '{0}' not found", guid));
			}

			db.Remove(delete);
			db.SaveChanges();
		}



	}
}