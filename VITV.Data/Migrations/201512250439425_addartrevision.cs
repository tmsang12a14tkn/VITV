namespace VITV.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addartrevision : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.EmployeeVideos", newName: "VideoEmployees");
            DropPrimaryKey("dbo.VideoEmployees");
            CreateTable(
                "dbo.ArticleRevisions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ArticleId = c.Int(nullable: false),
                        ChangedDate = c.DateTime(nullable: false),
                        ArticleContent = c.String(),
                        Order = c.Int(nullable: false),
                        CreateUserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Articles", t => t.ArticleId, cascadeDelete: true)
                .ForeignKey("dbo.ApplicationUsers", t => t.CreateUserId)
                .Index(t => t.ArticleId)
                .Index(t => t.CreateUserId);
            
            AddPrimaryKey("dbo.VideoEmployees", new[] { "Video_Id", "Employee_Id" });
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ArticleRevisions", "CreateUserId", "dbo.ApplicationUsers");
            DropForeignKey("dbo.ArticleRevisions", "ArticleId", "dbo.Articles");
            DropIndex("ArticleRevisions", new[] { "CreateUserId" });
            DropIndex("ArticleRevisions", new[] { "ArticleId" });
            DropPrimaryKey("dbo.VideoEmployees");
            DropTable("dbo.ArticleRevisions");
            AddPrimaryKey("dbo.VideoEmployees", new[] { "Employee_Id", "Video_Id" });
            RenameTable(name: "dbo.VideoEmployees", newName: "EmployeeVideos");
        }
    }
}
