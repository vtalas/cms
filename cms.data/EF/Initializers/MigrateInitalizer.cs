using System;
using System.Collections.Generic;
using System.Data.Entity;
using WebMatrix.WebData;

namespace cms.data.EF.Initializers
{
	public class MigrateInitalizer : MigrateDatabaseToLatestVersion<EfContext, Migrations.Configuration>
	{
		public MigrateInitalizer()
		{
		}
	}

}