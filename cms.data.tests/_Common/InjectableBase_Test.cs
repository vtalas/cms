using System;
using cms.data.Repository;

namespace cms.data.tests._Common
{
	public class InjectableBase_Test : Base_Test
	{
		protected readonly Guid GridId = Guid.NewGuid();
		protected readonly Guid GridElementId = Guid.NewGuid();

		public Func<IRepository> RepositoryCreator { get; set; }

		public InjectableBase_Test(Func<IRepository> repositoryCreator)
		{
			RepositoryCreator = repositoryCreator;
		}

	}
}