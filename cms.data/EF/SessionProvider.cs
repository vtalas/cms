using System;
using System.Data.Entity;

namespace cms.data.EF
{
	public class SessionProvider : IDataProvider
	{
		private readonly Func<JsonDataProvider> _createInstanceFunction;

		public SessionProvider(Func<JsonDataProvider> createInstanceFunction )
		{
			_createInstanceFunction = createInstanceFunction;
		}

		public SessionProvider(Func<JsonDataProvider> createInstanceFunction, IDatabaseInitializer<EfContext> initializer)
			:this(createInstanceFunction)
		{
			Database.SetInitializer(initializer);
		}

		public JsonDataProvider CreateSession { get { return _createInstanceFunction.Invoke(); } }

	}
}