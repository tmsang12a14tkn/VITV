namespace VITV.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatevideoaccess : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.VideoAccesses", "VideoId", "dbo.Videos");
            DropIndex("VideoAccesses", new[] { "VideoId" });
            CreateTable(
                "dbo.PageAccesses",
                c => new
                    {
                        PageUrl = c.String(nullable: false, maxLength: 128),
                        IPAdress = c.String(nullable: false, maxLength: 128),
                        Date = c.DateTime(nullable: false),
                        Thumbnail = c.String(),
                        Count = c.Int(nullable: false),
                        LastAccess = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.PageUrl, t.IPAdress, t.Date })
                .ForeignKey("dbo.Pages", t => t.PageUrl, cascadeDelete: true)
                .Index(t => t.PageUrl);
            
            CreateTable(
                "dbo.Pages",
                c => new
                    {
                        Url = c.String(nullable: false, maxLength: 128),
                        Thumbnail = c.String(),
                    })
                .PrimaryKey(t => t.Url);
            
            DropTable("dbo.VideoAccesses");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.VideoAccesses",
                c => new
                    {
                        VideoId = c.Int(nullable: false),
                        IPAdress = c.String(nullable: false, maxLength: 128),
                        Date = c.DateTime(nullable: false),
                        Count = c.Int(nullable: false),
                        LastAccess = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.VideoId, t.IPAdress, t.Date });
            
            DropForeignKey("dbo.PageAccesses", "PageUrl", "dbo.Pages");
            DropIndex("PageAccesses", new[] { "PageUrl" });
            DropTable("dbo.Pages");
            DropTable("dbo.PageAccesses");
            CreateIndex("dbo.VideoAccesses", "VideoId");
            AddForeignKey("dbo.VideoAccesses", "VideoId", "dbo.Videos", "Id", cascadeDelete: true);
        }
    }
}
