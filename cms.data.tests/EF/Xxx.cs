using cms.data.EF;

namespace cms.data.tests.EF
{
	public class Xxx
	{
		public static void DeleteDatabaseDataGenereateSampleData()
		{
			using (var db = new EfContext())
			{
				var a = new data.EF.Initializers.SampleData(db);
				db.Database.ExecuteSqlCommand("EXEC sp_MSforeachtable @command1 = \"ALTER TABLE ? NOCHECK CONSTRAINT all\"");
				db.Database.ExecuteSqlCommand("EXEC sp_MSForEachTable @command1 = \" DELETE FROM ?\" ");
				db.Database.ExecuteSqlCommand("EXEC sp_MSForEachTable @command1 = \" ALTER TABLE ? WITH CHECK CHECK CONSTRAINT all\" ");
				//db.Database.ExecuteSqlCommand("EXEC sp_MSForEachTable @command1 = \" IF OBJECTPROPERTY(object_id(''?''), ''TableHasIdentity'') = 1 DBCC CHECKIDENT (''?'', RESEED, 0)\" ");
				a.Generate();
			}
		}

		public static void DeleteDatabaseData()
		{
			using (var db = new EfContext())
			{
				db.Database.ExecuteSqlCommand("EXEC sp_MSforeachtable @command1 = \"ALTER TABLE ? NOCHECK CONSTRAINT all\"");
				db.Database.ExecuteSqlCommand("EXEC sp_MSForEachTable @command1 = \" DELETE FROM ?\" ");
				db.Database.ExecuteSqlCommand("EXEC sp_MSForEachTable @command1 = \" ALTER TABLE ? WITH CHECK CHECK CONSTRAINT all\" ");
			}

		}
	}
}