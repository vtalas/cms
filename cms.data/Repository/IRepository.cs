using System;
using System.Linq;
using cms.data.Shared.Models;

namespace cms.data.Repository
{
	public interface IRepository : IDisposable
	{
		IQueryable<Resource> Resources { get; }
		IQueryable<Grid> Grids { get; }
		IQueryable<ApplicationSetting> ApplicationSettings { get; }
		IQueryable<GridElement> GridElements { get; }
		IQueryable<UserProfile> UserProfile { get; }
		ApplicationSetting Add(ApplicationSetting item);
		GridElement Add(GridElement item);
		Resource Add(Resource itemToAttach);
		Grid Add(Grid item);
		void Remove(ApplicationSetting item);
		void Remove(GridElement item);
		void Remove(Resource item);
		void Remove(Grid item);
		void SaveChanges();
	}
}