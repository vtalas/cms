using System;
using System.Data.Entity;
using cms.data.EF.Bootstrap;
using cms.data.Shared.Models;
using System.Linq;

namespace cms.data.EF
{
	public class EfContext :DbContext, IBootStrapGenerator
	{
		public EfContext(string nameOrConnectionString) : base(nameOrConnectionString)
		{
		}

		public EfContext()
		{
		}
		
		public DbSet<ApplicationSetting> ApplicationSettings { get; set; }
		public DbSet<Grid> Grids { get; set; }
		public DbSet<GridElement> GridElements { get; set; }
		public DbSet<TemplateType> TemplateTypes { get; set; }
		public DbSet<Resource> Resources { get; set; }
		//public DbSet<Bootstrapgenerator> Bootstrapgenerators { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			//modelBuilder.Entity<Bootstrapgenerator>().Property(p => p.StatusData).HasColumnName("sm");
			//modelBuilder.Entity<Bootstrapgenerator>().Ignore(p => p.Status);
		}
	}

	public static class EfContextExt
	{
		public static bool Exist(this DbSet<Resource> context, int id  )
		{
			return context.Any(x => x.Id == id);
		}

		public static GridElement Get(this DbSet<GridElement> context, Guid guid, Guid applicationId)
		{
			var item = context.Single(x => x.Id == guid);
			
			if(item.Grid.Any(x=>x.ApplicationSettings.Id != applicationId))
				throw new InvalidOperationException();
			
			return item;
		}

		public static Resource Get(this DbSet<Resource> context, int id)
		{
			return context.Single(x => x.Id == id);
		}

	}
}