using cms.data.Repository;
using cms.data.Shared.Models;

namespace cms.data.tests._Common
{
	public static class ApplicationSettingsHelper
	{
		public static ApplicationSetting WithGrid(this ApplicationSetting item, Grid grid )
		{
			grid.ApplicationSettings = item;
			item.Grids.Add(grid);
			return item;
		}

		public static ApplicationSetting WithUser(this ApplicationSetting item, UserProfile user)
		{
			item.Users.Add(user);
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

		public static GridElement AddTo(this GridElement item, IRepository repository)
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

		public static Resource AddTo(this Resource item, IRepository repository)
		{
			repository.Add(item);
			repository.SaveChanges();
			return item;
		}
	}
}