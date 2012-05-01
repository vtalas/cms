using System.Data.Entity;
using System.Linq;
using cms.data.Models;

namespace cms.data.EF
{
	public class EfDataProvider : IDataProvider
	{
		private EfContext db { get; set; }
		public EfDataProvider()
		{
			db = new EfContext();
			
		}
		public EfDataProvider(IDatabaseInitializer<EfContext> initializer)
		{
			db = new EfContext();
			Database.SetInitializer(initializer);
		}

		public IQueryable<ApplicationSetting> ApplicationSettings
		{
			get { return db.ApplicationSettings;}
		}

		public ApplicationSetting Add(ApplicationSetting newitem)
		{
			throw new System.NotImplementedException();
		}

		public ApplicationSetting Get(int id)
		{
			throw new System.NotImplementedException();
		}

		public ApplicationSetting Update(ApplicationSetting item)
		{
			throw new System.NotImplementedException();
		}

		public ApplicationSetting Find(string name)
		{
			throw new System.NotImplementedException();
		}
	}
}