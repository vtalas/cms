using System;
using System.Linq;
using cms.data.Repository;
using cms.data.Shared.Models;

namespace cms.data.EF.RepositoryImplementation
{
	public class EfRepositoryApplication : IRepository
	{
		private EfContext db { get; set; }
		public Guid ApplictionId { get; set; }

		public EfRepositoryApplication(EfContext db, Guid applictionId)
		{
			this.db = db;
			ApplictionId = applictionId;
		}

		public EfRepositoryApplication(Guid applicationId)
			: this(new EfContext(), applicationId)
		{
		}

		public IQueryable<Resource> Resources{ get { return db.Resources;}}
		public IQueryable<Grid> Grids { get { return db.Grids.Where(x=>x.ApplicationSettings.Id == ApplictionId); } }
		public virtual IQueryable<ApplicationSetting> ApplicationSettings { get { return db.ApplicationSettings.Where(x=>x.Id == ApplictionId);} }
		public IQueryable<ApplicationSettingStorage> ApplicationSettingStorage { get { return db.ApplicationSettingStorage.Where(x=>x.AppliceSetting.Id == ApplictionId); } }
		public IQueryable<GridElement> GridElements { get { return db.GridElements.Where(x=>x.AppliceSetting.Id == ApplictionId); } }
		public virtual IQueryable<UserProfile> UserProfile { get { return db.UserProfile.Where(x=>x.Applications.Any(app=>app.Id == ApplictionId)); } }
		public virtual IQueryable<UserData> UserData { get { return db.UserData.Where(x=>x.User.Applications.Any(app=>app.Id == ApplictionId)); } }
		public IQueryable<GridElementGroup> GridElementGroup { get { return db.GridElementGroup;}}

		public virtual ApplicationSettingStorage Add(ApplicationSettingStorage item)
		{
			return db.ApplicationSettingStorage.Add(item);
		}

		public ApplicationSetting Add(ApplicationSetting item)
		{
			return db.ApplicationSettings.Add(item);
		}

		public UserData Add(UserData item)
		{
			return db.UserData.Add(item);
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