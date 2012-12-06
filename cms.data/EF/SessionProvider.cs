using System;
using System.Data.Entity;
using cms.data.DataProvider;

namespace cms.data.EF
{
	public class SessionProvider : IDataProvider
	{
		private readonly Func<DataProviderAbstract> _createInstanceFunction;

		public SessionProvider(Func<DataProviderAbstract> createInstanceFunction )
		{
			_createInstanceFunction = createInstanceFunction;
		}

		public SessionProvider(Func<DataProviderAbstract> createInstanceFunction, IDatabaseInitializer<EfContext> initializer)
			:this(createInstanceFunction)
		{
			Database.SetInitializer(initializer);
		}

		public  DataProviderAbstract CreateSession { get { return _createInstanceFunction.Invoke(); } }
	}
}