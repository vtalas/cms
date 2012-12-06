using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using cms.data.Dtos;
using cms.data.Shared.Models;
using cms.shared;
using System.Data.Entity;

namespace cms.data.EF.DataProvider
{
	public class PageAbstractImpl : PageAbstract
	{
		private IRepository db { get; set; }

		public PageAbstractImpl(ApplicationSetting application) : base(application) { }
		public PageAbstractImpl(ApplicationSetting application, IRepository context)
			: base(application)
		{
			db = context;
		}

		IQueryable<Grid> AvailableGrids
		{
			get
			{
				return db.Grids.Where(x => x.ApplicationSettings.Id == CurrentApplication.Id && x.Category == CategoryEnum.Page);
			}
		}

		public override IEnumerable<GridPageDto> List()
		{
			var a = AvailableGrids.ToList();
			return a.Select(grid => grid.ToGridPageDto()).ToList();
		}

		public override GridPageDto Get(Guid id)
		{
			var grid = AvailableGrids.Single(x => x.Id == id);
			return grid.ToGridPageDto();
		}

		//TODO: pokud nenanjde melo by o vracet homepage
		public override GridPageDto Get(string linkValue)
		{
				Func<Grid, bool> aa = grid => grid.Resources.ContainsKeyValue("link", linkValue, CurrentCulture);
				//var a = AvailableGrids().FirstOrDefault(x => x.Resources.ToList().GetByKey("link", CurrentCulture) != null );
				var a = AvailableGrids.Include(x=>x.Resources).ToList().FirstOrDefault(aa);
				if (a == null)
				{
					throw new ObjectNotFoundException(string.Format("'{0}' not found", linkValue));
				}
				return a.ToGridPageDto();
		}

		void CheckIfLinkExist(GridPageDto newitem)
		{

		}

		public override GridPageDto Add(GridPageDto newitem)
		{
			var item = newitem.ToGrid();
			CheckIfLinkExist(newitem);
			CurrentApplication.Grids.Add(item);
			db.Add(item);
			db.SaveChanges();
			return item.ToGridPageDto();
		}

		public override GridPageDto Update(GridPageDto item)
		{
			//if (item.ResourceDto != null)
			//{
			//	if (item.ResourceDto.Id != 0)
			//	{
			//		db.Resources.Single(x => x.Id == item.ResourceDto.Id).Value = item.ResourceDto.Value;
			//	}
			//}
			var grid = Get(item.Id);

			grid.Name = item.Name;
			grid.Home = item.Home;

			db.SaveChanges();
			return grid;
		}

		public override void Delete(Guid guid)
		{
			var delete = AvailableGrids.Single(x => x.Id == guid);
			db.Remove(delete);
			db.SaveChanges();
		}
	}
}