using System;
using System.Data.Entity;
using cms.data.DataProvider;
using cms.data.EF.DataProviderImplementation;
using cms.data.EF.Initializers;
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
			//Database.SetInitializer(new MigrateInitalizer());
			_dataProviderConstruct = () => new DataEfAuthorized(_applicationId, Repo, _currentUserId);
		}

		public SessionProvider(Func<DataProviderAbstract> fnc ) 
		{
			//Database.SetInitializer(new MigrateInitalizer());
			_dataProviderConstruct = fnc;
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