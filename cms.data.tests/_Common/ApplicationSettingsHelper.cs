using System;
using cms.data.EF.DataProvider;
using cms.data.Shared.Models;
using cms.shared;

namespace cms.data.tests._Common
{
	public static class ApplicationSettingsHelper
	{
		private static readonly string CurrentCulture = SharedLayer.Culture;
		
		public static ApplicationSetting WithGrid(this ApplicationSetting item, Grid grid )
		{
			grid.ApplicationSettings = item;
			item.Grids.Add(grid);
			return item;
		}

		public static ApplicationSetting WithGrid(this ApplicationSetting item, Grid grid, IRepository repository)
		{
			item.WithGrid(grid);
			repository.Add(grid);
			return item;
		}

		public static ApplicationSetting AddTo(this ApplicationSetting item, IRepository repository)
		{
			repository.Add(item);
			repository.SaveChanges();
			return item;
		}

		public static Grid AddTo(this Grid item, IRepository repository)
		{
			repository.Add(item);
			repository.SaveChanges();
			return item;
		}


	}
}