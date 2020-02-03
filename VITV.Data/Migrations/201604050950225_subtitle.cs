namespace VITV.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class subtitle : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Subtitles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Language = c.String(),
                        Source = c.String(),
                        VideoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Videos", t => t.VideoId, cascadeDelete: true)
                .Index(t => t.VideoId);
            
            AddColumn("dbo.VideoCategories", "HasYoutube", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Subtitles", "VideoId", "dbo.Videos");
            DropIndex("Subtitles", new[] { "VideoId" });
            DropColumn("dbo.VideoCategories", "HasYoutube");
            DropTable("dbo.Subtitles");
        }
    }
}
