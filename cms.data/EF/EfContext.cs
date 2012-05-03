using System.Data.Entity;
using cms.data.Models;

namespace cms.data.EF
{
	public class EfContext :DbContext
	{
		public DbSet<ApplicationSetting> ApplicationSettings { get; set; }
		public DbSet<Grid> Grids { get; set; }
		public DbSet<GridElement> GridElements { get; set; }
	}
}