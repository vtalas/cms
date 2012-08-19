using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace cms.data.EF.Initializers
{
	public class MigrateInitalizer : MigrateDatabaseToLatestVersion<EfContext, Migrations.Configuration>
	{

	}

}