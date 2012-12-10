using System;
using cms.data.EF.RepositoryImplementation;
using cms.data.Repository;

namespace cms.data.tests._Common
{
	public class InjectableBase_Test : Base_Test
	{
		protected IRepository Repository;
		protected readonly Guid _gridId = new Guid("0000005e-1115-480b-9ab7-a3ab3c0f6643");

		protected Func<IRepository> RepositoryCreator { get; set; }

		public InjectableBase_Test(Func<IRepository> repositoryCreator)
		{
			RepositoryCreator = repositoryCreator;
		}

	}
}