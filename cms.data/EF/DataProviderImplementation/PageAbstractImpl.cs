using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using cms.data.DataProvider;
using cms.data.Dtos;
using cms.data.EF.RepositoryImplementation;
using cms.data.Extensions;
using cms.data.Repository;
using cms.data.Shared.Models;
using cms.shared;

namespace cms.data.EF.DataProviderImplementation
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

		IQueryable<Grid> AvailablePages
		{
			get
			{
				return db.Grids.Where(x => x.ApplicationSettings.Id == CurrentApplication.Id && x.Category == CategoryEnum.Page);
			}
		}

		IQueryable<Grid> AvailableGrids
		{
			get
			{
				return db.Grids.Where(x => x.ApplicationSettings.Id == CurrentApplication.Id);
			}
		}

		public override IEnumerable<GridPageDto> List()
		{
			var a = AvailablePages.ToList();
			return a.Select(grid => grid.ToGridPageDto()).ToList();
		}

		public override GridPageDto Get(Guid id)
		{
			return GetFromDb(id).ToGridPageDto();
		}

		//TODO: pokud nenanjde melo by o vracet homepage
		public override GridPageDto Get(string linkValue)
		{
				Func<Grid, bool> aa = grid => grid.Resources.ContainsKeyValue(SpecialResourceEnum.Link, linkValue, null);
				var a = AvailablePages.Include(x=>x.Resources).ToList().FirstOrDefault(aa);
				if (a == null)
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
			
			var grid = GetFromDb(item.Id);
			
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
			var delete = AvailablePages.SingleOrDefault(x => x.Id == guid);
			if (delete == null)
			{
				throw new ObjectNotFoundException(string.Format(" '{0}' not found", guid));
			}

			db.Remove(delete);
			db.SaveChanges();
		}

		bool CheckIfLinkExist(string linkValue)
		{
			Func<Grid, bool> aa = grid => grid.Resources.ContainsKeyValue(SpecialResourceEnum.Link, linkValue, null);
			return AvailableGrids.Include(x => x.Resources).ToList().Any(aa);
		}

		void ValidateLink(string linkValue)
		{
			if (CheckIfLinkExist(linkValue))
			{
				throw new Exception(string.Format("item with link '{0}' allready Exists", linkValue));
			}
		}

		private Grid GetFromDb(Guid id)
		{
			var grid = AvailablePages.Single(x => x.Id == id);
			return grid;
		}

	}
}