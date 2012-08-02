using System;
using cms.data.EF;

namespace cms.data.tests.EF
{
	public class SessionManager : IDisposable
	{
		private static JsonDataEf Context { get; set; }

		public static JsonDataEf CreateSession
		{
			get
			{
				Context = new JsonDataEf(new Guid("c78ee05e-1115-480b-9ab7-a3ab3c0f6643"));
				return Context;
			}
		}

		public void Dispose()
		{
			Context.Dispose();
		}
	}
}