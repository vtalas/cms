using System.Linq;
using cms.data.Shared.Models;

namespace cms.data.EF.RepositoryImplementation
{
	public class EfRepository : IRepository
	{
		private EfContext db { get; set; }

		public EfRepository(EfContext db)
		{
			this.db = db;
		}

		public IQueryable<Resource> Resources{ get { return db.Resources;}}
		public IQueryable<Grid> Grids { get { return db.Grids; } }
		public IQueryable<ApplicationSetting> ApplicationSettings { get { return db.ApplicationSettings;} }

		public ApplicationSetting Add(ApplicationSetting item)
		{
			return db.ApplicationSettings.Add(item);
		}

		public Resource Add(Resource itemToAdd)
		{
			return db.Resources.Add(itemToAdd);
		}

		public Grid Add(Grid item)
		{
			return db.Grids.Add(item);
		}

		public void Remove(Grid item)
		{
			db.Grids.Remove(item);
		}

		public void SaveChanges()
		{
			db.SaveChanges();
		}

	}
}