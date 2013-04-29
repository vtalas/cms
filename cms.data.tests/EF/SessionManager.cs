using System;
using WebMatrix.WebData;
using cms.data.EF.DataProviderImplementation;
using cms.data.Repository;
using cms.data.Shared;

namespace cms.data.tests.EF
{
	public class SessionManager : IDisposable
	{
		private readonly IRepository _repo;
		public static Guid DefaultAppId = new Guid("c78ee05e-1115-480b-9ab7-a3ab3c0f6643");

		public SessionManager()
		{
			_repo = new RepositoryFactory().Create;
		}

		public DataEfAuthorized Session
		{
			get
			{
				var userId = WebSecurity.GetUserId("admin");
				return new DataEfAuthorized(DefaultAppId, _repo, userId);;
			}
		}

		public DataEfAuthorized CreateSessionWithInvalidUser
		{
			get
			{
				return new DataEfAuthorized(DefaultAppId, _repo, 0);
			}
		}

		public void Dispose()
		{
			_repo.Dispose();
		}
	}
}