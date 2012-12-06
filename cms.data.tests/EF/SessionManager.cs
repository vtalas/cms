using System;
using WebMatrix.WebData;
using cms.data.EF.DataProvider;

namespace cms.data.tests.EF
{
	public class SessionManager : IDisposable
	{
		private DataEfAuthorized Context { get; set; }

		public static Guid DefaultAppId = new Guid("c78ee05e-1115-480b-9ab7-a3ab3c0f6643");
		public DataEfAuthorized CreateSession
		{
			get
			{
				var userId = WebSecurity.GetUserId("admin");
				Context = new DataEfAuthorized(DefaultAppId, userId);
				return Context;
			}
		}

		public DataEfAuthorized CreateSessionWithInvalidUser
		{
			get
			{
				Context = new DataEfAuthorized(DefaultAppId, 0);
				return Context;
			}
		}

		public void Dispose()
		{
			Context.Dispose();
		}
	}
}