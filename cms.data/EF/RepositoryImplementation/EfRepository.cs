using System.Linq;
using System.Reflection.Emit;
using cms.data.Repository;
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

		public EfRepository() :this(new EfContext())
		{
		}

		public IQueryable<Resource> Resources{ get { return db.Resources;}}
		public IQueryable<Grid> Grids { get { return db.Grids; } }
		public virtual IQueryable<ApplicationSetting> ApplicationSettings { get { return db.ApplicationSettings;} }
		public IQueryable<ApplicationSettingStorage> ApplicationSettingStorage { get { return db.ApplicationSettingStorage; } }
		public IQueryable<GridElement> GridElements { get { return db.GridElements; } }
		public IQueryable<OAuthCms> OAuthCms { get; private set; }
		public virtual IQueryable<UserProfile> UserProfile { get { return db.UserProfile; } }
		public virtual IQueryable<UserData> UserData { get { return db.UserData; } }

		public ApplicationSettingStorage Add(ApplicationSettingStorage item)
		{
			return db.ApplicationSettingStorage.Add(item);
		}

		public ApplicationSetting Add(ApplicationSetting item)
		{
			return db.ApplicationSettings.Add(item);
		}

		public UserData Add(UserData item)
		{
			throw new System.NotImplementedException();
		}

		public OAuthCms Add(OAuthCms item)
		{
			throw new System.NotImplementedException();
		}

		public GridElement Add(GridElement item)
		{
			return db.GridElements.Add(item);
		}

		public Resource Add(Resource itemToAdd)
		{
			return db.Resources.Add(itemToAdd);
		}

		public Grid Add(Grid item)
		{
			return db.Grids.Add(item);
		}

		public void Remove(ApplicationSetting item)
		{
			throw new System.NotImplementedException();
		}

		public void Remove(UserData item)
		{
			throw new System.NotImplementedException();
		}

		public void Remove(GridElement item)
		{
			db.GridElements.Remove(item);
		}

		public void Remove(Resource item)
		{
			db.Resources.Remove(item);
		}

		public void Remove(Grid item)
		{
			db.Grids.Remove(item);
		}

		public void Remove(OAuthCms item)
		{
			throw new System.NotImplementedException();
		}

		public void SaveChanges()
		{
			db.SaveChanges();
		}

		public void Dispose()
		{
			if (db != null )
			{
				db.Dispose();
			}
		}
	}
}