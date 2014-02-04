using System;
using System.Linq;
using cms.data.Repository;
using cms.data.Shared.Models;

namespace cms.data.EF.RepositoryImplementation
{
	public class AdoDbRepository : IRepository
	{
		public void Dispose()
		{
			throw new NotImplementedException();
		}

		public IQueryable<Resource> Resources { get; private set; }
		public IQueryable<Grid> Grids { get; private set; }
		public IQueryable<ApplicationSetting> ApplicationSettings { get; private set; }
		public IQueryable<ApplicationSettingStorage> ApplicationSettingStorage { get; private set; }
		public IQueryable<GridElement> GridElements { get; private set; }
		public IQueryable<UserProfile> UserProfile { get; private set; }
		public IQueryable<UserData> UserData { get; private set; }
		public IQueryable<GridElementGroup> GridElementGroup { get; private set; }
		public ApplicationSettingStorage Add(ApplicationSettingStorage item)
		{
			throw new NotImplementedException();
		}

		public ApplicationSetting Add(ApplicationSetting item)
		{
			throw new NotImplementedException();
		}

		public UserData Add(UserData item)
		{
			throw new NotImplementedException();
		}

		public GridElement Add(GridElement item)
		{
			throw new NotImplementedException();
		}

		public Resource Add(Resource itemToAttach)
		{
			throw new NotImplementedException();
		}

		public Grid Add(Grid item)
		{
			throw new NotImplementedException();
		}

		public void Remove(ApplicationSetting item)
		{
			throw new NotImplementedException();
		}

		public void Remove(UserData item)
		{
			throw new NotImplementedException();
		}

		public void Remove(GridElement item)
		{
			throw new NotImplementedException();
		}

		public void Remove(Resource item)
		{
			throw new NotImplementedException();
		}

		public void Remove(Grid item)
		{
			throw new NotImplementedException();
		}

		public void SaveChanges()
		{
			throw new NotImplementedException();
		}
	}
}