using System.Data.Entity;

using cms.data.Shared.Models;

namespace cms.data.EF
{
	public class EfContext :DbContext
	{
		public EfContext(string nameOrConnectionString) : base(nameOrConnectionString){}

		public EfContext(){}

		public virtual IDbSet<ApplicationSetting> ApplicationSettings { get; set; }
		public virtual IDbSet<ApplicationSettingStorage> ApplicationSettingStorage { get; set; }
		public virtual IDbSet<Grid> Grids { get; set; }
		public virtual IDbSet<GridElement> GridElements { get; set; }
		public virtual IDbSet<TemplateType> TemplateTypes { get; set; }
		public virtual IDbSet<Resource> Resources { get; set; }
		public virtual IDbSet<UserProfile> UserProfile { get; set; }
		public virtual IDbSet<OAuthCms> OAuthCms { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			//modelBuilder.Entity<Bootstrapgenerator>().Property(p => p.StatusData).HasColumnName("sm");
			//modelBuilder.Entity<Bootstrapgenerator>().Ignore(p => p.Status);
		}

	}

}