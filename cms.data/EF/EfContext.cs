using System.Data.Entity;
using cms.data.Models;

namespace cms.data.EF
{
	public class EfContext :DbContext
	{
		public EfContext(string nameOrConnectionString) : base(nameOrConnectionString)
		{
		}

		public EfContext()
		{
		}
		
		public DbSet<ApplicationSetting> ApplicationSettings { get; set; }
		public DbSet<Grid> Grids { get; set; }
		public DbSet<GridElement> GridElements { get; set; }
		public DbSet<TemplateType> TemplateTypes { get; set; }
	}
}