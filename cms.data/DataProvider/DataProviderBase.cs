using System;
using System.Data.Entity;
using System.Linq;
using cms.data.Extensions;
using cms.data.Repository;
using cms.data.Shared.Models;
using cms.shared;

namespace cms.data.DataProvider
{
	public class DataProviderBase
	{
		protected ApplicationSetting CurrentApplication { get; set; }

		protected string CurrentCulture { get { return SharedLayer.Culture; } }

		protected IRepository db { get; set; }

		protected DataProviderBase(ApplicationSetting application )
		{
			CurrentApplication = application;
		}

		protected IQueryable<Grid> AvailableGrids
		{
			get
			{
				var a = db.Grids.Where(x => x.ApplicationSettings.Id == CurrentApplication.Id );
				return a;
			}
		}

		protected Grid GetGrid(string linkValue)
		{
			Func<Grid, bool> aa = grid => grid.Resources.ContainsKeyValue(SpecialResourceEnum.Link, linkValue, null);
			var a = AvailableGrids.Include(x => x.Resources).ToList().FirstOrDefault(aa);
			return a;
		}

		protected Grid GetGrid(Guid id)
		{
			var grid = AvailableGrids.SingleOrDefault(x => x.Id == id);
			return grid;
		}

		protected bool LinkExist(string linkValue)
		{
			Func<Grid, bool> aa = grid => grid.Resources.ContainsKeyValue(SpecialResourceEnum.Link, linkValue, null);
			return AvailableGrids.Include(x => x.Resources).ToList().Any(aa);
		}

		protected void ValidateLink(string linkValue)
		{
			if (LinkExist(linkValue))
			{
				throw new Exception(string.Format("item with link '{0}' allready Exists", linkValue));
			}
		}

	}
}