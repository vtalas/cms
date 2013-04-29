using System;
using System.Data.Entity;
using cms.data.DataProvider;
using cms.shared;

namespace cms.data.EF
{
	public class SessionProvider : IKeuValueStoreageProvider
	{
		private readonly Func<DataProviderAbstract> _createInstanceFunction;
		private DataProviderAbstract _instance;

		public SessionProvider(Func<DataProviderAbstract> createInstanceFunction)
		{
			_createInstanceFunction = createInstanceFunction;
		}

		public SessionProvider(Func<DataProviderAbstract> createInstanceFunction, IDatabaseInitializer<EfContext> initializer)
			: this(createInstanceFunction)
		{
			Database.SetInitializer(initializer);
		}

		public DataProviderAbstract CreateSession
		{
			get
			{
				_instance = _createInstanceFunction.Invoke();
				return _instance;
			}
		}

		public void Dispose()
		{
			if (_instance != null)
			{
				//_instance.Dispose();
			}
		}

		public IKeyValueStorage CreateKeyValueSession
		{
			get
			{
				return _createInstanceFunction.Invoke().Settings;
			}
		}
	}

	public interface IKeuValueStoreageProvider : IDisposable
	{
		IKeyValueStorage CreateKeyValueSession { get; }
	}
}