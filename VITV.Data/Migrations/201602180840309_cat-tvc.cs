namespace VITV.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cattvc : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.VideoTVCVideoCategories",
                c => new
                    {
                        VideoTVC_Id = c.Int(nullable: false),
                        VideoCategory_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.VideoTVC_Id, t.VideoCategory_Id })
                .ForeignKey("dbo.VideoTVCs", t => t.VideoTVC_Id, cascadeDelete: true)
                .ForeignKey("dbo.VideoCategories", t => t.VideoCategory_Id, cascadeDelete: true)
                .Index(t => t.VideoTVC_Id)
                .Index(t => t.VideoCategory_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VideoTVCVideoCategories", "VideoCategory_Id", "dbo.VideoCategories");
            DropForeignKey("dbo.VideoTVCVideoCategories", "VideoTVC_Id", "dbo.VideoTVCs");
            DropIndex("VideoTVCVideoCategories", new[] { "VideoCategory_Id" });
            DropIndex("VideoTVCVideoCategories", new[] { "VideoTVC_Id" });
            DropTable("dbo.VideoTVCVideoCategories");
        }
    }
}
