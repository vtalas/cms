using System.Threading;
using WebMatrix.WebData;

namespace cms.data.EF
{
	public sealed class SecurityProvider 
	{
		private static SimpleMembershipInitializer _initializer;
		private static object _initializerLock = new object();
		private static bool _isInitialized;
		private static bool _createTables;

		public static void EnsureInitialized(bool createTables = false)
		{
			_createTables = createTables;
			LazyInitializer.EnsureInitialized(ref _initializer, ref _isInitialized, ref _initializerLock);
		}

		public static void Destroy()
		{
			_isInitialized = false;
		}

		private class SimpleMembershipInitializer
		{
			public SimpleMembershipInitializer()
			{
				WebSecurity.InitializeDatabaseConnection("EfContext", "UserProfile", "Id", "UserName", _createTables);
			}
		}
	}
}