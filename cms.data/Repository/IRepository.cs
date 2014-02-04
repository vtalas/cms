using System;
using System.Collections.Generic;
using System.Linq;
using cms.data.Shared.Models;

namespace cms.data.Repository
{
	public interface IRepository : IDisposable
	{
		IQueryable<Resource> Resources { get; }
		IQueryable<Grid> Grids { get; }
		IQueryable<ApplicationSetting> ApplicationSettings { get; }
		IQueryable<ApplicationSettingStorage> ApplicationSettingStorage { get; }
		IQueryable<GridElement> GridElements { get; }
		IQueryable<UserProfile> UserProfile { get; }
		IQueryable<UserData> UserData { get; }
		IQueryable<GridElementGroup> GridElementGroup { get; }
		
		
		ApplicationSettingStorage Add(ApplicationSettingStorage item);
		ApplicationSetting Add(ApplicationSetting item);
		UserData Add(UserData item);
		GridElement Add(GridElement item);
		Resource Add(Resource itemToAttach);
		Grid Add(Grid item);
		void Remove(ApplicationSetting item);
		void Remove(UserData item);
		void Remove(GridElement item);
		void Remove(Resource item);
		void Remove(Grid item);
		void SaveChanges();
	}

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

	public class RepositoryInMemory : IRepository
	{
		public RepositoryInMemory()
		{
			Resources = new List<Resource>().AsQueryable();
			GridElementGroup = new List<GridElementGroup>().AsQueryable();
		}

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