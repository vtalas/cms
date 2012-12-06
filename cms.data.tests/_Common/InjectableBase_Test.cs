using System;
using cms.data.EF.DataProvider;

namespace cms.data.tests._Common
{
	public class InjectableBase_Test : Base_Test
	{
		protected IRepository Repository;

		protected Func<IRepository> RepositoryCreator { get; set; }

		public InjectableBase_Test(Func<IRepository> repositoryCreator)
		{
			RepositoryCreator = repositoryCreator;
		}

	}
}