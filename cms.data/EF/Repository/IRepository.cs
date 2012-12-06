using System.Linq;
using cms.data.Shared.Models;

namespace cms.data.EF.Repository
{
	public interface IRepository
	{
		IQueryable<Resource> Resources { get; }
		IQueryable<Grid> Grids { get; }
		IQueryable<ApplicationSetting> ApplicationSettings { get; }
		ApplicationSetting Add(ApplicationSetting item);
		Resource Add(Resource itemToAttach);
		Grid Add(Grid item);
		void Remove(Grid item);
		void SaveChanges();
	}
}