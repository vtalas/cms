using System;
using System.Data.Entity;
using cms.data.DataProvider;
using cms.shared;

namespace cms.data.EF
{
	public class SessionProvider : IXxx
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
				_instance.Dispose();
			}
		}

		public IKeyValueStorage CreateKeyValueSession
		{
			get
			{
				_instance = _createInstanceFunction.Invoke();
				return _instance;
			}
		}
	}

	public interface IXxx : IDisposable
	{
		IKeyValueStorage CreateKeyValueSession { get; }
	}
}