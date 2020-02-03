namespace VITV_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class videoaccess : DbMigration
    {
        public override void Up()
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
                .PrimaryKey(t => new { t.VideoId, t.IPAdress, t.Date })
                .ForeignKey("dbo.Videos", t => t.VideoId, cascadeDelete: true)
                .Index(t => t.VideoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VideoAccesses", "VideoId", "dbo.Videos");
            DropIndex("VideoAccesses", new[] { "VideoId" });
            DropTable("dbo.VideoAccesses");
        }
    }
}
