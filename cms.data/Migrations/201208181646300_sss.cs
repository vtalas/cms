namespace cms.data.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class sss : DbMigration
    {
        public override void Up()
        {
			//CreateTable(
			//    "Resources",
			//    c => new
			//        {
			//            Id = c.Int(nullable: false, identity: true),
			//            Culture = c.String(),
			//            Key = c.String(),
			//            Value = c.String(),
			//            Owner = c.Guid(nullable: false),
			//        })
			//    .PrimaryKey(t => t.Id);
            
			//CreateTable(
			//    "GridElementResources",
			//    c => new
			//        {
			//            GridElement_Id = c.Guid(nullable: false),
			//            Resource_Id = c.Int(nullable: false),
			//        })
			//    .PrimaryKey(t => new { t.GridElement_Id, t.Resource_Id })
			//    .ForeignKey("GridElements", t => t.GridElement_Id, cascadeDelete: true)
			//    .ForeignKey("Resources", t => t.Resource_Id, cascadeDelete: true)
			//    .Index(t => t.GridElement_Id)
			//    .Index(t => t.Resource_Id);
            
			//AddColumn("ApplicationSettings", "DefaultLanguage", c => c.String());
			//AddColumn("Grids", "Resource_Id", c => c.Int());
			//AlterColumn("Grids", "Id", c => c.Guid(nullable: false));
			//AlterColumn("GridElements", "Id", c => c.Guid(nullable: false));
			//AlterColumn("GridElementGrids", "GridElement_Id", c => c.Guid(nullable: false));
			//AlterColumn("GridElementGrids", "Grid_Id", c => c.Guid(nullable: false));
			//AddForeignKey("Grids", "Resource_Id", "Resources", "Id");
			//CreateIndex("Grids", "Resource_Id");
			//DropColumn("Grids", "Link");
        }
        
        public override void Down()
        {
            AddColumn("Grids", "Link", c => c.String());
            DropIndex("GridElementResources", new[] { "Resource_Id" });
            DropIndex("GridElementResources", new[] { "GridElement_Id" });
            DropIndex("Grids", new[] { "Resource_Id" });
            DropForeignKey("GridElementResources", "Resource_Id", "Resources");
            DropForeignKey("GridElementResources", "GridElement_Id", "GridElements");
            DropForeignKey("Grids", "Resource_Id", "Resources");
            AlterColumn("GridElementGrids", "Grid_Id", c => c.Int(nullable: false));
            AlterColumn("GridElementGrids", "GridElement_Id", c => c.Int(nullable: false));
            AlterColumn("GridElements", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("Grids", "Id", c => c.Int(nullable: false, identity: true));
            DropColumn("Grids", "Resource_Id");
            DropColumn("ApplicationSettings", "DefaultLanguage");
            DropTable("GridElementResources");
            DropTable("Resources");
        }
    }
}
