using System;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
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
		IQueryable<OAuthCms> OAuthCms { get; }
		IQueryable<UserProfile> UserProfile { get; }
		IQueryable<UserData> UserData { get; }
		ApplicationSettingStorage Add(ApplicationSettingStorage item);
		ApplicationSetting Add(ApplicationSetting item);
		UserData Add(UserData item);
		OAuthCms Add(OAuthCms item);
		GridElement Add(GridElement item);
		Resource Add(Resource itemToAttach);
		Grid Add(Grid item);
		void Remove(ApplicationSetting item);
		void Remove(UserData item);
		void Remove(GridElement item);
		void Remove(Resource item);
		void Remove(Grid item);
		void Remove(OAuthCms item);
		void SaveChanges();
	}
}