using System;
using cms.data.EF;
using cms.data.EF.RepositoryImplementation;
using cms.data.Repository;

namespace cms.data.Shared
{
	public class RepositoryFactory: IDisposable
	{
		public EfRepository repo { get; set; }

		public IRepository Create{ get
		{
			repo = new EfRepository(new EfContext());
			return repo;
		}}

		public void Dispose()
		{
			if (repo != null)
			{
				repo.Dispose();
			}
		}
	}
}