using System.Linq;
using Newtonsoft.Json.Linq;
using cms.data.Models;

namespace cms.data
{
	public interface IDataProvider
	{
		IQueryable<ApplicationSetting> ApplicationSettings { get; }
		ApplicationSetting Add(ApplicationSetting newitem);
		ApplicationSetting Get(int id);
		ApplicationSetting Update(ApplicationSetting item);
		ApplicationSetting Find(string name);

	}
}