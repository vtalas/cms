using System;
using cms.data.EF;
using cms.data.EF.DataProvider;

namespace cms.data.tests.EF
{
	public class SessionManager : IDisposable
	{
		private static DataEf Context { get; set; }

		public static DataEf CreateSession
		{
			get
			{
				Context = new DataEf(new Guid("c78ee05e-1115-480b-9ab7-a3ab3c0f6643"), 1);
				return Context;
			}
		}

		public void Dispose()
		{
			Context.Dispose();
		}
	}
}