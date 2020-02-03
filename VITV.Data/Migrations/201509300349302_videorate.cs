namespace VITV.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class videorate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.VideoRates",
                c => new
                    {
                        VideoId = c.Int(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                        Rate1 = c.Double(nullable: false),
                        Rate2 = c.Double(nullable: false),
                        Rate3 = c.Double(nullable: false),
                    })
                .PrimaryKey(t => new { t.VideoId, t.UserId })
                .ForeignKey("dbo.ApplicationUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Videos", t => t.VideoId, cascadeDelete: true)
                .Index(t => t.VideoId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VideoRates", "VideoId", "dbo.Videos");
            DropForeignKey("dbo.VideoRates", "UserId", "dbo.ApplicationUsers");
            DropIndex("VideoRates", new[] { "UserId" });
            DropIndex("VideoRates", new[] { "VideoId" });
            DropTable("dbo.VideoRates");
        }
    }
}
