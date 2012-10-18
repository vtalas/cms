using System;
using System.Data.Entity;

namespace cms.data.EF
{
	public class SessionProvider : IDataProvider
	{
		private readonly Func<data.DataProvider> _createInstanceFunction;

		public SessionProvider(Func<data.DataProvider> createInstanceFunction )
		{
			_createInstanceFunction = createInstanceFunction;
		}

		public SessionProvider(Func<data.DataProvider> createInstanceFunction, IDatabaseInitializer<EfContext> initializer)
			:this(createInstanceFunction)
		{
			Database.SetInitializer(initializer);
		}

		public data.DataProvider CreateSession { get { return _createInstanceFunction.Invoke(); } }

	}
}