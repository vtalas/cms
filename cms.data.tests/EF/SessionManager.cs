using System;
using cms.data.EF;
using cms.data.EF.DataProvider;

namespace cms.data.tests.EF
{
	public class SessionManager : IDisposable
	{
		private static DataEfAuthorized Context { get; set; }

		public static DataEfAuthorized CreateSessionWithSampleData
		{
			get
			{
				Context = new DataEfAuthorized(new Guid("c78ee05e-1115-480b-9ab7-a3ab3c0f6643"), 1);
				return Context;
			}
		}

		public static DataEfAuthorized CreateSessionWithInvalidUser
		{
			get
			{
				Context = new DataEfAuthorized(new Guid("c78ee05e-1115-480b-9ab7-a3ab3c0f6643"), 0);
				return Context;
			}
		}

		public void Dispose()
		{
			Context.Dispose();
		}
	}
}