using System.Data.Entity;

namespace cms.data.EF.Initializers
{
	public class DropAndCreate : IDatabaseInitializer<EfContext>
	{
		#region IDatabaseInitializer<EfContext> Members

		public void InitializeDatabase(EfContext context)
		{
			if (context.Database.Exists())
			{
				Database.SetInitializer<EfContext>(null);
				context.Database.ExecuteSqlCommand("ALTER DATABASE " + context.Database.Connection.Database +" SET SINGLE_USER WITH ROLLBACK IMMEDIATE");
				context.Database.Delete();
			}

			context.Database.Create();

			Seed(context);
			context.SaveChanges();
		}

		#endregion

		protected virtual void Seed(EfContext context)
		{
			var sampledata = new SampleData(context);
			sampledata.Generate();
		}
	}
}