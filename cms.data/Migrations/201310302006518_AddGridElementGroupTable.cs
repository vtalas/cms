
namespace cms.data.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AddGridElementGroupTable : DbMigration
    {
        public override void Up()
        {
//            CreateTable(
//                "dbo.GridElementGroups",
//                c => new
//                    {
//                        Id = c.Int(nullable: false, identity: true),
//                        Name_Id = c.Int(),
//                        GridElement_Id = c.Guid(),
//                    })
//                .PrimaryKey(t => t.Id)
//                .ForeignKey("dbo.Resources", t => t.Name_Id)
//                .ForeignKey("dbo.GridElements", t => t.GridElement_Id)
//                .Index(t => t.Name_Id)
//                .Index(t => t.GridElement_Id);
            
//            CreateTable(
//                "dbo.UserDatas",
//                c => new
//                    {
//                        Id = c.Int(nullable: false, identity: true),
//                        Key = c.String(),
//                        Value = c.String(),
//                        Created = c.DateTime(nullable: false),
//                        User_Id = c.Int(),
//                    })
//                .PrimaryKey(t => t.Id)
//                .ForeignKey("dbo.UserProfile", t => t.User_Id)
//                .Index(t => t.User_Id);
//            
//            AddColumn("dbo.Grids", "Authorize", c => c.Boolean(nullable: false));
//            AddColumn("dbo.UserProfile", "ApplicationUser", c => c.Int());
//            AddColumn("dbo.UserProfile", "AccessToken", c => c.String());
//            AddColumn("dbo.UserProfile", "Expire", c => c.Long());
//            AddColumn("dbo.UserProfile", "RefreshToken", c => c.String());
        }
        

        public override void Down()
        {
//            DropIndex("dbo.UserDatas", new[] { "User_Id" });
//            DropIndex("dbo.GridElementGroups", new[] { "GridElement_Id" });
//            DropIndex("dbo.GridElementGroups", new[] { "Name_Id" });
//  //          DropForeignKey("dbo.UserDatas", "User_Id", "dbo.UserProfile");
//            DropForeignKey("dbo.GridElementGroups", "GridElement_Id", "dbo.GridElements");
//            DropForeignKey("dbo.GridElementGroups", "Name_Id", "dbo.Resources");
//            DropColumn("dbo.UserProfile", "RefreshToken");
//            DropColumn("dbo.UserProfile", "Expire");
//            DropColumn("dbo.UserProfile", "AccessToken");
//            DropColumn("dbo.UserProfile", "ApplicationUser");
//            DropColumn("dbo.Grids", "Authorize");
//            DropTable("dbo.UserDatas");
//            DropTable("dbo.GridElementGroups");
        }
    }
}
