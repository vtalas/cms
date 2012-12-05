using System.Data.Entity;
using log4net;

namespace cms.data.EF.Initializers
{
	public class DeleteAllDataAndSampleData : IDatabaseInitializer<EfContext>
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (DropAndCreateAlwaysForce));
	
		public void InitializeDatabase(EfContext context)
		{
			if (context.Database.Exists())
			{
				Database.SetInitializer<EfContext>(null);
				context.Database.ExecuteSqlCommand("ALTER DATABASE " + context.Database.Connection.Database +" SET SINGLE_USER WITH ROLLBACK IMMEDIATE");
				context.Database.Delete();
				Log.Info("Database deleted");
			}

			context.Database.Create();

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