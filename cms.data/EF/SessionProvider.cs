using System;
using System.Data.Entity;
using cms.data.DataProvider;
using cms.data.EF.DataProviderImplementation;
using cms.data.Repository;
using cms.data.Shared;
using cms.shared;

namespace cms.data.EF
{
	public class SessionProvider : IDisposable
	{
		private readonly Guid _applicationId;
		private readonly int _currentUserId;
		private Func<DataProviderAbstract> zx;
		private IRepository Repo { get; set; }

		public SessionProvider(Guid applicationId, int currentUserId)
		{
			_applicationId = applicationId;
			_currentUserId = currentUserId;
			zx = () => new DataEfAuthorized(_applicationId, Repo, _currentUserId);
		}

		public SessionProvider(Func<DataProviderAbstract> fnc ) 
		{
			zx = fnc;
		}

		public SessionProvider(Guid applicationId, int currentUserId, IDatabaseInitializer<EfContext> initializer)
			: this(applicationId, currentUserId)
		{
			Database.SetInitializer(initializer);
		}

		public DataProviderAbstract Session
		{
			get { return new DataEfAuthorized(_applicationId, Repo, _currentUserId); }
		}

		public SessionProvider CreateSession()
		{
			Repo = new RepositoryFactory().Create;
			return this;
		}

		public void Dispose()
		{
			if (Repo != null)
			{
				Repo.Dispose();
			}
		}
	}

}