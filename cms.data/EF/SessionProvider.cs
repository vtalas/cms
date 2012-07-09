using System.Data.Entity;

namespace cms.data.EF
{
	public class SessionProvider : IDataProvider 
	{
		private static EfContext db { get; set; }
		
		public SessionProvider(EfContext context)
		{
			db = context;
		}

		public SessionProvider():this (new EfContext())
		{
		}

		public SessionProvider(IDatabaseInitializer<EfContext> initializer)
		{
			db = new EfContext();
			Database.SetInitializer(initializer);
		}


		public static  EfContext CreateSession(EfContext context)
		{
			db = context;
			return context;
		}

		public static EfContext CreateSession()
		{
			return CreateSession(new EfContext());
		}

		public EfContext Context
		{
			get { return db; }
		}


		public void Dispose()
		{
			if(db != null)
			{
				db.Dispose();
			}
		}
	}
}