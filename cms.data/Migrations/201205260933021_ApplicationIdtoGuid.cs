namespace cms.data.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class ApplicationIdtoGuid : DbMigration
    {
        public override void Up()
        {
            AlterColumn("ApplicationSettings", "Id", c => c.Guid(nullable: false));
            AlterColumn("Grids", "ApplicationSettings_Id", c => c.Guid());
        }
        
        public override void Down()
        {
            AlterColumn("Grids", "ApplicationSettings_Id", c => c.Int());
            AlterColumn("ApplicationSettings", "Id", c => c.Int(nullable: false, identity: true));
        }
    }
}
