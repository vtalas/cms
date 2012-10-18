using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace cms.data.EF.Initializers
{
	public class DropAndCreateTables : IDatabaseInitializer<EfContext>
	{
		public void InitializeDatabase(EfContext context)
		{
		
			bool modelvalid = false;
			try
			{
				modelvalid = context.Database.CompatibleWithModel(true);
			}
			catch (Exception)
			{
			}

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

			context.SaveChanges();

		}

		protected virtual void Seed(EfContext context)
		{
			var sampledata = new SampleData(context);
			sampledata.Generate();
		}
	}
}