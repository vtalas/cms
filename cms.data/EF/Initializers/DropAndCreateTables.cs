using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using WebMatrix.WebData;

namespace cms.data.EF.Initializers
{
	public class DropAndCreateTables : IDatabaseInitializer<EfContext>
	{
		public void InitializeDatabase(EfContext context)
		{
			var modelvalid = context.Database.CompatibleWithModel(true);
			if (!modelvalid)
			{
				//TODO: obcas to spadne - DB is in use
				//	context.Database.Delete();
			}

			if (!context.Database.Exists())
			{
				context.Database.Create();
			}
	
			// remove all tables
			context.Database.ExecuteSqlCommand("EXEC sp_MSforeachtable @command1 = \"DROP TABLE ?\"");

			// create all tables
			var dbCreationScript = ((IObjectContextAdapter)context).ObjectContext.CreateDatabaseScript();
			context.Database.ExecuteSqlCommand(dbCreationScript);

			Seed(context);

			WebSecurity.InitializeDatabaseConnection("EfContext", "UserProfile", "Id", "UserName", autoCreateTables: true);

			context.SaveChanges();

		}

		protected virtual void Seed(EfContext context)
		{
			var sampledata = new SampleData(context);
			sampledata.Generate();
		}
	}
}