using System;
using System.Data.Entity;

namespace cms.data.EF
{
	public class SessionProvider : IDataProvider
	{
		Lazy<JsonDataEf> db;


		public SessionProvider(Guid applicationId)
		{
//			var x = new Action<string>(() => new JsonDataEf(applicationId));

			db = new Lazy<JsonDataEf>(() => new JsonDataEf(applicationId));
		}

		public SessionProvider(string applicationName)
		{
			db = new Lazy<JsonDataEf>(() => new JsonDataEf(applicationName));
		}

		public SessionProvider(string applicationName, IDatabaseInitializer<EfContext> initializer)
			: this(applicationName)
		{
			Database.SetInitializer(initializer);
		}

		public SessionProvider(Guid applicationId, IDatabaseInitializer<EfContext> initializer)
			: this(applicationId)
		{
			Database.SetInitializer(initializer);
		}

		public JsonDataEf CreateSession { get { return new JsonDataEf("test1"); } }

		public void Dispose()
		{
			if(db.IsValueCreated)
			{
				db.Value.Dispose();
			}
		}
	}
}