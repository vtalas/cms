using System;
using System.Data.Entity;

namespace cms.data.EF
{
	public class SessionProvider : IDataProvider
	{
		Lazy<JsonDataEf> db;

		public SessionProvider(Guid applicationId)
		{
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

		JsonDataEf Instance { get { return db.Value; } }

		public JsonDataEf CreateSession { get { return Instance; } }

		public void Dispose()
		{
			if(Instance != null)
			{
				Instance.Dispose();
			}
		}
	}
}