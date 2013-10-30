using System;
using System.Linq;
using cms.data.Shared.Models;

namespace cms.data.Repository
{
	public class UserRepository : IRepository
	{
		public IRepository Repository { get; set; }
		public int UserId { get; set; }

		protected UserRepository(IRepository repository, int userId)
		{
			Repository = repository;
			UserId = userId;
		}

		public virtual IQueryable<ApplicationSetting> ApplicationSettings { get { return Repository.ApplicationSettings.Where(x => x.Users.Any(user => user.Id == UserId)); } }
		public virtual IQueryable<UserData> UserData { get { return Repository.UserData.Where(x => x.User.Id == UserId); } }
		public IQueryable<GridElementGroup> GridElementGroup { get; private set; }
		public virtual IQueryable<UserProfile> UserProfile { get { return Repository.UserProfile.Where(x => x.Id == UserId); } }
	
		public IQueryable<Resource> Resources { get; private set; }
		public IQueryable<Grid> Grids { get; private set; }
		public IQueryable<ApplicationSettingStorage> ApplicationSettingStorage { get; private set; }
		public IQueryable<GridElement> GridElements { get; private set; }
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

		public void Dispose()
		{
			throw new NotImplementedException();
		}
	}
}