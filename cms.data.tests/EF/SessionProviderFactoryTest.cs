using System;
using WebMatrix.WebData;
using cms.data.EF;

namespace cms.data.tests.EF
{
	public class SessionProviderFactoryTest
	{
		public static Guid DefaultAppId = new Guid("c78ee05e-1115-480b-9ab7-a3ab3c0f6643");

		public static SessionProvider GetSessionProvider()
		{
			var userId = WebSecurity.GetUserId("admin");
			return new SessionProvider(DefaultAppId, userId);
		}
	}
}