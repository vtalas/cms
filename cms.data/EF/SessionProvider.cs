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
		private readonly Func<DataProviderAbstract> _dataProviderConstruct;
		private IRepository Repo { get; set; }

		public SessionProvider(Guid applicationId, int currentUserId)
		{
			_applicationId = applicationId;
			_currentUserId = currentUserId;
			_dataProviderConstruct = () => new DataEfAuthorized(_applicationId, Repo, _currentUserId);
		}

		public SessionProvider(Func<DataProviderAbstract> fnc ) 
		{
			_dataProviderConstruct = fnc;
		}

		public SessionProvider(Guid applicationId, int currentUserId, IDatabaseInitializer<EfContext> initializer)
			: this(applicationId, currentUserId)
		{
			Database.SetInitializer(initializer);
		}

		public DataProviderAbstract Session
		{
			get { return _dataProviderConstruct.Invoke(); }
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