using System.Threading;
using WebMatrix.WebData;

namespace cms.data.EF
{
	public sealed class SecurityProvider 
	{
		private static SimpleMembershipInitializer _initializer;
		private static object _initializerLock = new object();
		private static bool _isInitialized;

		public static void EnsureInitialized()
		{
			LazyInitializer.EnsureInitialized(ref _initializer, ref _isInitialized, ref _initializerLock);
		}

		private class SimpleMembershipInitializer
		{
			public SimpleMembershipInitializer()
			{
				WebSecurity.InitializeDatabaseConnection("EfContext", "UserProfile", "Id", "UserName", autoCreateTables: true);
			}
		}
	}
}